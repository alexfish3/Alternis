using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [SerializeField] bool inDialogue;
    [SerializeField] bool isProtag;
    [SerializeField] bool isTyping;

    [Header("Dialogue Info")]
    [SerializeField] int currentDialoguePos;
    [SerializeField] float delay;
    [SerializeField] DialogueBlock[] currentDialogue;

    [Header("Drag In Assets")]
    [SerializeField] GameObject fullDialgoueBox;
    [SerializeField] GameObject protagDialogueBox;
    [SerializeField] GameObject opponentDialgoueBox;
    [SerializeField] GameObject protagImage;
    [SerializeField] GameObject opponentImage;
    [SerializeField] TMP_Text protagName;
    [SerializeField] TMP_Text opponentName;
    [SerializeField] TMP_Text protagDialogue;
    [SerializeField] TMP_Text opponentDialogue;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (inDialogue == false)
            {
                inDialogue = true;
                currentDialoguePos = 0;
                fullDialgoueBox.SetActive(true);
                enterNewDialogue();
            }
            else if(inDialogue == true)
            {
                nextDialogue();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Reset dialogue
            currentDialoguePos = 0;
            enterNewDialogue();
        }
    }

    public void enterNewDialogue()
    {
        // Is protag at startup
        if (currentDialogue[currentDialoguePos].isProtag)
        {
            protagDialogueBox.SetActive(true);
            opponentDialgoueBox.SetActive(false);
            StartCoroutine(typeText(currentDialogue[currentDialoguePos].dialogueOfPerson, protagDialogue, protagImage));
        }
        // Is opponent at startup
        else
        {
            protagDialogueBox.SetActive(false);
            opponentDialgoueBox.SetActive(true);
            StartCoroutine(typeText(currentDialogue[currentDialoguePos].dialogueOfPerson, opponentDialogue, opponentImage));
        }
    }

    public void nextDialogue()
    {
        // Reached end of the dialogue
        if(currentDialoguePos == currentDialogue.Length - 1)
        {
            protagDialogueBox.SetActive(false);
            opponentDialgoueBox.SetActive(false);
            fullDialgoueBox.SetActive(false);
            inDialogue = false;
        }
        // Can still continue dialogue
        else
        {
            currentDialoguePos++;
            updateDialgoueUI();
        }
    }

    public void updateDialgoueUI()
    {
        // Is protag at startup
        if (currentDialogue[currentDialoguePos].isProtag)
        {
            protagDialogueBox.SetActive(true);
            opponentDialgoueBox.SetActive(false);
            StartCoroutine(typeText(currentDialogue[currentDialoguePos].dialogueOfPerson, protagDialogue, protagImage));
        }
        // Is opponent at startup
        else
        {
            protagDialogueBox.SetActive(false);
            opponentDialgoueBox.SetActive(true);
            StartCoroutine(typeText(currentDialogue[currentDialoguePos].dialogueOfPerson, opponentDialogue, opponentImage));
        }
    }

    public IEnumerator typeText(string textToType, TMP_Text textBox, GameObject headshot)
    {
        isTyping = true;
        textBox.text = "";
        headshot.GetComponent<Animator>().SetTrigger("Talking");
        char[] splitUp = textToType.ToCharArray();

        foreach(char c in splitUp)
        {
            textBox.text += c;
            yield return new WaitForSeconds(delay);
        }
        headshot.GetComponent<Animator>().SetTrigger("Finsihed");
        isTyping = false;
    }
}
