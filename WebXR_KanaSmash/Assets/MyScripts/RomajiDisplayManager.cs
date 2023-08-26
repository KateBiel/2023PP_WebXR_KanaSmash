using UnityEngine;
using TMPro;
public class RomajiDisplayManager : MonoBehaviour
{
    [Header("Romaji Display")]
    [SerializeField] private Syllabaries syllabariesReference;

    private TextMeshProUGUI romajiText;
    [SerializeField] private GameObject currentRomajiCanvas;

    [Header("WhackManager Reference")]
    [SerializeField] private WackManger whackManager;


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

            PlayAudioForRomaji(chosenRomaji);

            return chosenRomaji;
        }
        return null;
    }

    private void PlayAudioForRomaji(string romaji)
    {
        AudioClip clipToPlay = GetAudioClipByName(romaji);
        if (clipToPlay != null)
        {
            whackManager.kanaAudioSource.clip = clipToPlay;
            whackManager.kanaAudioSource.Play();
        }
        else
        {
            Debug.LogWarning($"Audio clip for {romaji} not found!");
        }
    }

    private AudioClip GetAudioClipByName(string clipName)
    {
        foreach (AudioClip clip in whackManager.kanaAudioClip)
        {
            if (clip.name == clipName)
            {
                return clip;
            }
        }
        return null;
    }
}
