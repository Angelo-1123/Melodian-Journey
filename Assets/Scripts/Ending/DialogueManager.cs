using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ink.Runtime;
using TMPro;

public class DialogueManager : MonoBehaviour
{
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

    void Start()
    {
        dialoguePlaying = false;
        EnterDialogueMode(inkJSON, "Ending");
    }

    void Update()
    {
        if(Input.GetButtonDown("Interact"))
        {
            ContinueStory();
        }
        
        if(!dialoguePlaying)
        {
            SceneManager.LoadScene(0);
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
