using System.Collections;
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
    int punsCollected = 1;
    bool playerInRange = false;

    public bool fighting = false;
    public bool beaten = false;
    [SerializeField] SceneName nextScene;

    [Header("Dialogue")]
    [SerializeField] GameObject dialogueBox;
    [SerializeField] TMP_Text nameTag;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] List<Sentence> defaultDialogue;
    [SerializeField] List<Sentence> fightingDialogue;

    int currentLine = 0;

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
            ShowDialogue();
        }

        var punObjects = GameObject.FindGameObjectsWithTag("PunObject");
        punsNeeded = punObjects.Length;
    }

    private void Update()
    {
        if (PunManager.Instance) {
            punsCollected = PunManager.Instance.PunsFound;
        }

        if (!debug && playerInRange) {
            ShowDialogue();
        }     

        if (SceneManager.GetActiveScene().name.Equals("FirstDojo") && currentLine == defaultDialogue.Count) 
            SceneLoader.Instance.LoadScene(nextScene);
    }

    public void ShowDialogue() {
        playerInRange = false;
        PlayerStopHandler.dialogueEngaged = true;

        dialogueBox.SetActive(true);

        if (!beaten) {
            NextDialogue();
        }
        else {
            SceneLoader.Instance.LoadScene(nextScene);
        }
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

    private void AdvanceDefaultDialogue() 
    {
        if (currentLine <= defaultDialogue.Count - 1)
        {
            var currentSentence = defaultDialogue[currentLine];
            var currentImage = currentSentence.image;
            
            if (currentLine != 0) 
            {
                var previousImage = defaultDialogue[currentLine - 1].image;

                if (previousImage != currentImage) {
                    previousImage.SetActive(false);
                }
            }

            currentImage.SetActive(true);

            if (currentSentence.moodSprite != null) {
                currentImage.GetComponent<Image>().sprite = currentSentence.moodSprite;
            }
    
            dialogueText.text = currentSentence.text;
            nameTag.text = currentSentence.name;
        }       
        else if (currentLine == defaultDialogue.Count)
        {        
            if (CompareTag("Enemy") && fightingDialogue.Count != 0 && punsCollected >= punsNeeded) { 
                currentLine = 0;
                fighting = true;           
                return;
            } 
            else {
                dialogueText.text = "You still have a lot to learn. Come back if you're ready, Young Pundawan.";
            }
        }
        else {
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
        
        if (currentLine >= fightingDialogue.Count - 1) {
            beaten = true;
        } 
        
        if(currentLine >= fightingDialogue.Count) {
            dialogueBox.SetActive(false);
            PlayerStopHandler.dialogueEngaged = false;
            return;
        } 

        currentLine++;
    }

    #endregion
}
