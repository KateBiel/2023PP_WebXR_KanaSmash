using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// Needed for Button and Text

public class KanaManager : MonoBehaviour
{
    public Syllabaries syllabariesData;
    public List<TextMeshProUGUI> allButtons;


    // Start is called before the first frame update
    void Start()
    {

    }

    public void AssignCharactersToButtons(WackManger.GameMode mode) // Reference the GameMode enum from WackManager here
    {
        List<string> sourceList = new List<string>(); // This list will hold the characters we want to use, either Katakana or Hiragana.

        switch (mode)
        {
            case WackManger.GameMode.Katakana: // And here...
                sourceList.AddRange(syllabariesData.katakanaList);
                break;
            case WackManger.GameMode.Hiragana: // ...and here.
                sourceList.AddRange(syllabariesData.hiraganaList);
                break;
            default:
                return; // Handle other modes or default situations if needed.
        }

        // Shuffle the sourceList to make it random
        for (int i = 0; i < sourceList.Count; i++)
        {
            string temp = sourceList[i];
            int randomIndex = Random.Range(i, sourceList.Count);
            sourceList[i] = sourceList[randomIndex];
            sourceList[randomIndex] = temp;
        }

        // Now assign the characters to the buttons
        for (int i = 0; i < allButtons.Count; i++)
        {
            TextMeshProUGUI buttonText = allButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = sourceList[i];
        }
    }
}