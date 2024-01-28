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
    [SerializeField] bool debug = false;
    [SerializeField] int punsNeeded;
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
    bool showDialogue = false;
     int currentLine;

   
  
    void OnTriggerEnter2D(Collider2D other)
    {
        playerInRange = true;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        playerInRange = false;
    }
    private void Update()
    {
        if (PunManager.Instance)
        {
            punsCollected = PunManager.Instance.PunsFound;

        }
        if(!debug)ShowDialogue(playerInRange);

        Debug.Log(nextScene.ToString());

        if(SceneManager.GetActiveScene().name.Equals("FirstDojo") && currentLine == defaultDialogue.Count) 
            SceneLoader.Instance.LoadScene(nextScene);
    }
    private void Start()
    {
        if(debug) ShowDialogue(true);
        var punObjects = GameObject.FindGameObjectsWithTag("PunObject");
        punsNeeded = punObjects.Length;
    }

    public void ShowDialogue(bool show) {
        PlayerStopHandler.dialogueEngaged = show;

        dialogueBox.SetActive(show);

        if (!beaten) nextDialogue(0);
        else SceneLoader.Instance.LoadScene(nextScene);
    }

    void nextDialogue(int current) {
        currentLine = current;
        nextDialogue();
    }
    public void nextDialogue() {

        if (!fighting)
        {
            if (currentLine <= defaultDialogue.Count-1)
            {
                if (currentLine != 0 && defaultDialogue[currentLine].image != defaultDialogue[currentLine - 1].image)
                {
                    defaultDialogue[currentLine - 1].image.SetActive(false);
                }
                defaultDialogue[currentLine].image.SetActive(true);
                if (defaultDialogue[currentLine].moodSprite != null)
                    defaultDialogue[currentLine].image.GetComponent<Image>().sprite = defaultDialogue[currentLine].moodSprite;

                dialogueText.text = defaultDialogue[currentLine].text;
                nameTag.text = defaultDialogue[currentLine].name;
            }
            
            if (currentLine == defaultDialogue.Count)
            {
                
                if (gameObject.tag.Equals("Enemy") && fightingDialogue.Count != 0) { 
                    fighting = true;

                    defaultDialogue[currentLine-1].image.SetActive(false);
                    nextDialogue(0);
                    return;
                } 
                else ShowDialogue(false);

            }
             if (currentLine < defaultDialogue.Count) currentLine++;
        }
        else
        {
            if (currentLine == 0)
            {
                if (punsCollected < punsNeeded)
                {
                    dialogueText.text = "You still have a lot to learn. Come back if you're ready, Young Pundawan.";
                }

                else dialogueText.text = "Show me what you've got!";
            }

            // close if not enough puns collected 
            if (currentLine == 1 && punsCollected < punsNeeded)
            {
                ShowDialogue(false);

            }

            if(currentLine != 0 && !beaten) dialogueText.text = fightingDialogue[currentLine].text;

            if (currentLine <= fightingDialogue.Count - 1)
            {
                // set previous image inactive
                if (currentLine != 0 && fightingDialogue[currentLine].image != fightingDialogue[currentLine - 1].image)
                {
                    fightingDialogue[currentLine - 1].image.SetActive(false);
                }
                // set current image active and set sprite
                fightingDialogue[currentLine].image.SetActive(true);
                if (fightingDialogue[currentLine].moodSprite != null)
                    fightingDialogue[currentLine].image.GetComponent<Image>().sprite = fightingDialogue[currentLine].moodSprite;

                nameTag.text = fightingDialogue[currentLine].name;
            }

            
            if (currentLine == fightingDialogue.Count-1) beaten = true;

            if(currentLine == fightingDialogue.Count) ShowDialogue(false);

            // increasing currentLine
            if (currentLine < fightingDialogue.Count) currentLine++;

        }


    }
}
