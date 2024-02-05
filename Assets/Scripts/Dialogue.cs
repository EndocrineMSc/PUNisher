
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class Sentence
{
    public string name;
    [TextArea(3, 5)]public string text;
    public GameObject image;
    public Sprite moodSprite;

    public Sentence(string name, string text, GameObject image, Sprite mood)
    {
        this.name = name;
        this.text = text;
        this.image = image;
        this.moodSprite = mood;
    }
}

public class Dialogue : MonoBehaviour
{
    #region Fields and Properties

    [SerializeField] bool debug = false;
    int punsNeeded;
    int punsCollected = 0;
    bool playerInRange = false;
    bool firstDojo;

    public bool fighting = false;
    public bool beaten = false;
    [SerializeField] SceneName nextScene; 

    [Header("Dialogue")]
    [SerializeField] GameObject dialogueBox;
    [SerializeField] TMP_Text nameTag;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] int currentLine = 0;
    [SerializeField] List<Sentence> defaultDialogue;
    [SerializeField] List<Sentence> fightingDialogue;

   

   #endregion

   #region Methods
  
    void OnTriggerEnter2D(Collider2D other)
    {
        playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        playerInRange = false;
    }

    private void Start()
    {
        if (debug) {
            ShowDialogue(true);
        }
        firstDojo = SceneManager.GetActiveScene().name.Equals("FirstDojo");
        var punObjects = GameObject.FindGameObjectsWithTag("PunObject");
        punsNeeded = punObjects.Length;
    }

    private void Update()
    {
        if (PunManager.Instance) {
            punsCollected = PunManager.Instance.PunsFound;
        }

        if (!debug && playerInRange)
            ShowDialogue(true);

        if (!beaten) PlayerStopHandler.dialogueEngaged = dialogueBox.activeInHierarchy;
        else PlayerStopHandler.dialogueEngaged = true;

        if (PlayerStopHandler.dialogueEngaged && Input.GetKeyDown(KeyCode.Mouse0))
        {
            NextDialogue();
        }

        if (firstDojo && currentLine > defaultDialogue.Count) 
            FindObjectOfType<SceneLoader>().LoadScene(nextScene);
        if (beaten && currentLine > fightingDialogue.Count)
            FindObjectOfType<SceneLoader>().LoadScene(nextScene);

    }

    public void ShowDialogue(bool show) {
        if(!beaten) currentLine = 0;
        playerInRange = false;

        dialogueBox.SetActive(show);
        NextDialogue();
    }

    public void NextDialogue() {

        if (!fighting)
        {
            AdvanceDefaultDialogue();
        }
        else
        {
            AdvanceFightingDialogue();
        }
    }

    GameObject currentImage;
    private void AdvanceDefaultDialogue() 
    {
        if (currentLine <= defaultDialogue.Count - 1)
        {
            var currentSentence = defaultDialogue[currentLine];
            currentImage = currentSentence.image;

            if (currentLine != 0)
            {
                var previousImage = defaultDialogue[currentLine - 1].image;

                if (previousImage != currentImage)
                {
                    previousImage.SetActive(false);
                }
            }

            currentImage.SetActive(true);

            if (currentSentence.moodSprite != null)
            {
                currentImage.GetComponent<Image>().sprite = currentSentence.moodSprite;
            }

            dialogueText.text = currentSentence.text;
            nameTag.text = currentSentence.name;
        }
        //else if (currentLine == defaultDialogue.Count - 1) { 
        //    //if(!firstDojo && punsCollected == punsNeeded)  
        //    //{
                

        //    //    return;
        //    //}
            
        //}
        else if (currentLine == defaultDialogue.Count)
        {
            if (firstDojo)
            {
                currentImage.SetActive(false);
                dialogueBox.SetActive(false);
                PlayerStopHandler.dialogueEngaged = false;
            }
            else
            {
                if (punsCollected != punsNeeded)
                {
                    dialogueText.text = "You still have a lot to learn. Come back if you're ready, Young Pundawan.";
                    //return;
                }
                else
                {
                    fighting = true;
                    currentLine = 0;
                    NextDialogue();
                    return;
                }
            }
        }
        else if (currentLine > defaultDialogue.Count)
        {
           currentImage.SetActive(false);
            currentLine = 0;
            dialogueBox.SetActive(false);
            PlayerStopHandler.dialogueEngaged = false;
            return;
        }

        currentLine++;
    }

    private void AdvanceFightingDialogue() {
        if (currentLine == 0) {
            dialogueText.text = "Show me what you've got!";
        }
        else if (currentLine <= fightingDialogue.Count - 1)
        {
            var currentSentence = fightingDialogue[currentLine];
            var currentImage = currentSentence.image;

            if (currentLine != 0 && !beaten) {
                dialogueText.text = currentSentence.text; 
            }

            // set previous image inactive
            if (currentImage != fightingDialogue[currentLine - 1].image)
            {
                fightingDialogue[currentLine - 1].image.SetActive(false);
            }

            if (currentImage)
                currentImage.SetActive(true);

            if (currentSentence.moodSprite != null) {
                currentImage.GetComponent<Image>().sprite = currentSentence.moodSprite;
            }
            nameTag.text = currentSentence.name;
        }
        
        if (currentLine == fightingDialogue.Count - 1) {
            beaten = true;
        } 
        
        if(currentLine == fightingDialogue.Count) {
            dialogueBox.SetActive(false);
        } 

        currentLine++;
    }

    #endregion
}
