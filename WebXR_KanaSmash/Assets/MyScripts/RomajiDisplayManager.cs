using UnityEngine;
using TMPro;
public class RomajiDisplayManager : MonoBehaviour
{
    [Header("Romaji Display")]
    [SerializeField] private Syllabaries syllabariesReference;

    private TextMeshProUGUI romajiText;
    [SerializeField] private GameObject currentRomajiCanvas;

    private void Start()
    {
        // Instantiate the prefab
        //currentRomajiCanvas = Instantiate(romajiCanvasPrefab, transform.position, Quaternion.identity);

        
        romajiText = currentRomajiCanvas.transform.Find("MainPanel/Body/RomajiText").GetComponent<TextMeshProUGUI>();
    }

    public TextMeshProUGUI GetRomajiTextComponent()
    {
        return romajiText;
    }

    public string SetRandomRomaji()
    {
        if (romajiText != null && syllabariesReference != null)
        {
            int randomIndex = Random.Range(0, syllabariesReference.romajiList.Count);
            string chosenRomaji = syllabariesReference.romajiList[randomIndex];
            romajiText.text = chosenRomaji;
            return chosenRomaji;
        }
        return null;
    }
}
