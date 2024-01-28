using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    [SerializeField] int punsNeeded;
    int punsCollected = 1;

    public bool fighting = false;
    public bool beaten = false;

    [Header("Dialogue")]
    [SerializeField] GameObject dialogueBox;
    [SerializeField] TMP_Text nameTag;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] List<Sentence> defaultDialogue;
    [SerializeField] List<Sentence> fightingDialogue;
    bool showDialogue = false;
     int currentLine;

   
  
    void OnTriggerEnter(Collider other)
    {
        showDialogue = true;
    }
    private void OnTriggerExit(Collider other)
    {
        showDialogue = false;
    }
    private void Update()
    {
        if (PunManager.Instance)
        {
            punsCollected = PunManager.Instance.PunsFound;

        }
        //ShowDialogue(playerInRange);
    }
    private void Start()
    {
        ShowDialogue(true);
        var punObjects = GameObject.FindGameObjectsWithTag("PunObject");
        punsNeeded = punObjects.Length;
    }

    public void ShowDialogue(bool show) {
        dialogueBox.SetActive(show);
        if(!beaten) nextDialogue(0);
    }

    void nextDialogue(int current) {
        Debug.Log("new");
        currentLine = current;
        nextDialogue();
    }
    public void nextDialogue() {
        Debug.Log(currentLine +" start");
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
            Debug.Log("fighting start " + currentLine);
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

            Debug.Log("fighting end " + currentLine);
        }

        Debug.Log(currentLine + " end");

    }
}
