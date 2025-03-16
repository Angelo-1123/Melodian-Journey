using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using TMPro;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;

public class MazeGameManager : MonoBehaviour
{
    public static MazeGameManager instance;

    public enum GameState {GAMESET, CHOICE, ANSWER, GAMEOVER}
    public GameState state;
    public enum mazePaths {WARM, COLD}
    public mazePaths correctPath;
    public int correctGuesses;
    public int uniqueSongs; // Currently 6 unique songs, 1-3 are in Am, 4-6 are in C
    public List<int> spentSongs;

    public GameObject warmBtn, coldBtn;

    // Audio Variables
    public EventReference mazeSongs;
    public EventInstance eventInstance;
    public int chosenSong;
    

    // Dialogue variables
    [Header("Dialogue (Optional)")]
    [SerializeField] private string dialogueKnotName;
    [Header("Dialogue UI")]
    [SerializeField] public GameObject dialoguePanel;
    [SerializeField] public TextMeshProUGUI dialogueText;
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;
    private bool dialoguePlaying;
    private Story currentStory;

        public static MazeGameManager Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        warmBtn.SetActive(false);
        coldBtn.SetActive(false);
        state = GameState.GAMESET;
        dialoguePlaying = false;
        EnterDialogueMode(inkJSON, "Maze_Entrance");
        eventInstance = RuntimeManager.CreateInstance(mazeSongs);
        correctGuesses = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(state == GameState.GAMESET)
        {
            if(Input.GetButtonDown("Interact"))
            {
                if(!dialoguePlaying)
                {
                    state = GameState.CHOICE;
                    SetChallenge();
                    EnterDialogueMode(inkJSON, "Paths");
                }
                else
                {
                    ContinueStory();
                }
            }
        }

        if(state == GameState.CHOICE)
        {
            if(Input.GetButtonDown("Interact"))
            {
                if(!dialoguePlaying)
                {
                    eventInstance.start();
                    warmBtn.SetActive(true);
                    coldBtn.SetActive(true);
                }
                else
                {
                    ContinueStory();
                }
            }
        }

        if(state == GameState.ANSWER)
        {
            SetChallenge();
            EnterDialogueMode(inkJSON, "Paths");
            state = GameState.CHOICE;
        }

        if(state == GameState.GAMEOVER)
        {
            Debug.Log("Congrats, you won!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    public void SetChallenge()
    {
        chosenSong = Random.Range(1,7); //upper bound is exclusive

        while(spentSongs.Contains(chosenSong))
        {
            chosenSong = Random.Range(1,7);
        }
        if(chosenSong < 4)
        {
            correctPath = mazePaths.COLD;
        }
        if(chosenSong > 3)
        {
            correctPath = mazePaths.WARM;
        }

        eventInstance.setParameterByName("songList", chosenSong);
    }

    public void ChooseCold()
    {
        if(correctPath == mazePaths.COLD)
        {
            correctGuesses++;
            EnterDialogueMode(inkJSON, "Right_Path");
        }
        else
        {
            correctGuesses = 0;
            spentSongs.Clear();
            EnterDialogueMode(inkJSON, "Lost");
        }
        warmBtn.SetActive(false);
        coldBtn.SetActive(false);

        if(correctGuesses == 3)
        {
            state = GameState.GAMEOVER;
        }
        else
        {
            state = GameState.ANSWER;
        }

    }

    public void ChooseWarm()
    {
        if(correctPath == mazePaths.WARM)
        {
            correctGuesses++;
            EnterDialogueMode(inkJSON, "Right_Path");
        }
        else
        {
            correctGuesses = 0;
            spentSongs.Clear();
            EnterDialogueMode(inkJSON, "Lost");
        }
        warmBtn.SetActive(false);
        coldBtn.SetActive(false);
        
        if(correctGuesses == 3)
        {
            state = GameState.GAMEOVER;
        }
        else
        {
            state = GameState.ANSWER;
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON, string knotName)
    {
        currentStory = new Story(inkJSON.text);
        currentStory.ChoosePathString(knotName);
        dialoguePlaying = true;
        
        ContinueStory();
    }

    private void ContinueStory()
    {
        if(currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
        }
        else
        {
            if( currentStory.currentChoices.Count > 0 )
            {
                for (int i = 0; i < currentStory.currentChoices.Count; ++i) {
                    Choice choice = currentStory.currentChoices [i];
                    Debug.Log("Choice " + (i + 1) + ". " + choice.text);
                }
            }
            else
            {
                ExitDialogueMode();
            }
        }
    }

    private void ExitDialogueMode()
    {
        dialoguePlaying = false;
        dialogueText.text = "";
    }
}