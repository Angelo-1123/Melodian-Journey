using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Ink.Runtime;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum GameState {GAMEPLAY, GAMEOVER, NEXTLEVEL} // spawn the board, wait for the answer, verify the answer and end game.
    public GameState state;
    public Transform[] sizeDisplayer;
    public Transform[] spawnPoints;
    public Transform[] splitSpawnPoints;
    public GameObject musicNote;
    public GameObject displayNotes;
    public Transform split;
    public int correctSplits; //number of correct splits.
    public int markedSplits; //number of correct splits marked by the player.
    public int wrongSplits; //number of wrong splits marked by the player.
    public Material[] noteMaterials;
    public Material highlight;
    public int size;
    public int sizeLeft; // used to avoid randomizing a note that wouldn't fit.
    public int noteSize;

    // Dialogue variables
    [Header("Dialogue (Optional)")]
    [SerializeField] private string dialogueKnotName;
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;
    private bool dialoguePlaying = false;
    private Story currentStory;


    public static GameManager Instance
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
        dialoguePlaying = false;
        state = GameState.GAMEPLAY;
        dialogueKnotName = "Intro";
        EnterDialogueMode(inkJSON, dialogueKnotName);

        CreateBoard();
    }

    // Update is called once per frame
    void Update()
    {

        if(state == GameState.GAMEOVER)
        {
            if(markedSplits == correctSplits && wrongSplits == 0)
            {
                state = GameState.NEXTLEVEL;
                Debug.Log("Congrats, you won!");
                dialogueKnotName = "Split_Game_Victory";
                EnterDialogueMode(inkJSON, dialogueKnotName);
            }
            else
            {
                Debug.Log("Incorrect");
                dialogueKnotName = "Incorrect_Spell";
                state = GameState.GAMEPLAY;
            }
        }

        if(state == GameState.NEXTLEVEL)
        {    
            if(Input.GetButtonDown("Interact"))
            {
            ContinueStory();
            }

            if(!dialoguePlaying)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        if(state == GameState.GAMEPLAY)
        {
            if(Input.GetButtonDown("Interact"))
            {
                if(!dialoguePlaying)
                {
                    dialogueKnotName = "Split_Game_Rules";
                    EnterDialogueMode(inkJSON, dialogueKnotName);
                }
                else
                {
                    ContinueStory();
                }
            }
        }
    }

    void SpawnNote(GameObject note, int count, int noteSize)
    {   
        note.GetComponent<MusicNote>().value = noteSize;

        note.GetComponent<MusicNote>().noteMaterial = noteMaterials[noteSize];
        note.GetComponent<MeshRenderer>().material = noteMaterials[noteSize];

        Instantiate(note.transform, spawnPoints[count].position, spawnPoints[count].rotation);
    }
    void SpawnSplit(Transform split, int count)
    {
        split.GetComponent<Split>().isCorrect = false;
        Instantiate(split, splitSpawnPoints[count].position, splitSpawnPoints[count].rotation);
    }
    void SpawnCorrectSplit(Transform split, int count)
    {
        split.GetComponent<Split>().isCorrect = true;
        Instantiate(split, splitSpawnPoints[count].position, splitSpawnPoints[count].rotation);
    }
     public void MarkSplit(int ammount, bool isCorrect)
    {
        if(isCorrect)
        {
            markedSplits += ammount;
            Debug.Log("MarkedSplits: " + markedSplits);
        }
        else
        {
            wrongSplits += ammount;
            Debug.Log("WrongSplits: " + wrongSplits);
        }
    }

    public void CreateBoard()
    {
        size = UnityEngine.Random.Range(2,5); // upper bound is exclusive
        sizeLeft = size;
        correctSplits = 0;


        for (int i = 0; i < size; i++)
        {
            displayNotes.GetComponent<MeshRenderer>().material = noteMaterials[1];
            Instantiate(displayNotes.transform, sizeDisplayer[i].position, sizeDisplayer[i].rotation);
        }

        for (int i = 0; i < 8; i++)
        {
            if(i <= 6)
            {
                Debug.Log("SizeLeft" + i + ":" + sizeLeft);
                noteSize = UnityEngine.Random.Range(1, (sizeLeft+1)); // upper bound is exclusive
                Debug.Log("noteSize:" + i + ":" + noteSize);
                sizeLeft -= noteSize;
                Debug.Log("SizeLeft:" + i + ":" + sizeLeft);
                SpawnNote(musicNote, i, noteSize);
                Debug.Log("splitSpawnPoints: " + splitSpawnPoints.Length);
            }
            else
            {
                SpawnNote(musicNote, i, sizeLeft);
            }

            if(i < splitSpawnPoints.Length)
            {
                
                if (sizeLeft == 0)
                {
                    correctSplits++;
                    sizeLeft = size;
                    SpawnCorrectSplit(split, i);
                }
                else
                {
                    SpawnSplit(split, i);
                }
            }
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
            ExitDialogueMode();
        }
    }

    private void ExitDialogueMode()
    {
        dialoguePlaying = false;
        dialogueText.text = "";
    }

    public void EndGame()
    {
        state = GameState.GAMEOVER;
    }

}
