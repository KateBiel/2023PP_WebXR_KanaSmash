using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WackManger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] List<WackButton> _wackButton;
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI _bestScoreText;
    [SerializeField] TextMeshProUGUI _timeText;

    [Header("Game Configuration")]
    [SerializeField] float _gameTime = 30;
    [SerializeField] float _roundDelay = 1;
   // [SerializeField] int _minActiveButtons = 1;
   // [SerializeField] int _maxActiveButtons = 3;
    

    int _activeButtons = 0;
    int _score = 0;
    int _bestScore = 0;
    bool _gameEnded = false;
    float _gameTimeLeft;
    Coroutine _timeTickCoroutine;

    //Katakana Hiragana

    [Header("Game Mode")]
    public GameMode currentMode;

    [Header("Syllabaries")]
    public Syllabaries syllabariesData;

    [Header("Romaji Display")]
    [SerializeField] private RomajiDisplayManager romajiDisplayManager;
    [SerializeField] private TextMeshProUGUI romajiText;
    private GameObject currentRomajiCanvas;

    [Header("KanaAudio")]
    public AudioSource kanaAudioSource;
    public List<AudioClip> kanaAudioClip;

    private string currentRomajiPrompt;
    private string currentCharacterPrompt;
    public enum GameMode
    {
        Katakana,
        Hiragana,
        Mix
    }


    private void Awake ()
    {
        romajiText = romajiDisplayManager.GetRomajiTextComponent();

    }

    public void OnEnable()
    {
        _wackButton.ForEach(wackButton =>
        {
            wackButton.OnHit.AddListener(_RegisterHit);
        });
        StartGame(); 
    }

    public void OnDisable()
    {
        _wackButton.ForEach(wackButton =>
        {
            wackButton.OnHit.RemoveListener(_RegisterHit);
        });
    }

    public void StartGame()
    {
        _gameEnded = false;
        _StartTimer(); 
        _ResetTable();
        _ResetScore();
        _ResetScore(); 
        StartCoroutine(_ActivateButtons());
    }
    public void EndGame()
    {
        _gameEnded = true;
        _ResetTable();  // Deactivate all buttons when the game ends
    }
    void _ResetTable()
    {
        _wackButton.ForEach(_wackButton =>
        {
            _wackButton.Deactivate(); 
        });
    }

    void _RegisterHit(WackButton hitButton)
    {
        if (_gameEnded) return;

        Debug.Log("Button hit: " + hitButton.Character);
        Debug.Log("Expected character: " + currentCharacterPrompt);

        int charIndex = syllabariesData.hiraganaList.IndexOf(hitButton.Character);
        if (charIndex == -1) // Not found in Hiragana
        {
            // Try the Katakana list:
            charIndex = syllabariesData.katakanaList.IndexOf(hitButton.Character);
        }

        // Play the sound
        if (charIndex >= 0 && charIndex < kanaAudioClip.Count) // Safety check
            kanaAudioSource.PlayOneShot(kanaAudioClip[charIndex]);

        if (hitButton.Character == currentCharacterPrompt)
        {
            // Player hit the correct button
            //Debug.Log($"Button whacked");
            _score += 10; // or however much you want to award for a correct hit
            _PrintScore();
            _CheckBestScore();

            _ResetTable();
            StartCoroutine(_ActivateButtons());

            // You could also play a success sound, show some positive feedback, etc. here
        }
        else
        {
            // Player hit the wrong button
            _score = 0; // Reset score if that's the mechanic you want
            _PrintScore();

            // You could also play an error sound, flash the screen, etc. here
        }



        /*
        _activeButtons--;
        if (_activeButtons == 0)
        {
            StartCoroutine(_ActivateButtons());
        }
       /*



        /*
        _score++;


        _activeButtons--;
        _PrintScore();
        _CheckBestScore();
        if (_activeButtons == 0)
        {
           
            
            StartCoroutine(_ActivateButtons()); 
        }
       */
    }
    /*
 IEnumerator _ActivateButtons()
 {
     if (!_gameEnded)
     {


         yield return new WaitForSeconds(_roundDelay);

         int amountButon = Random.Range(_minActiveButtons, _maxActiveButtons + 1);
         List<int> indexes = _RollIndex(amountButon);

         indexes.ForEach(index =>
         {
             _wackButton[index].Activate();
             _activeButtons++;
         });
         */

    IEnumerator _ActivateButtons()
    {
        if (!_gameEnded) 
        {
            yield return new WaitForSeconds(_roundDelay);

            // Get the chosen Romaji from RomajiDisplayManager and decide the character
            currentRomajiPrompt = romajiDisplayManager.SetRandomRomaji();
            int randomCharIndex = syllabariesData.romajiList.IndexOf(currentRomajiPrompt);

           

            // Continue with the rest of the method
            switch (currentMode)
            {
                case GameMode.Katakana:
                    currentCharacterPrompt = syllabariesData.katakanaList[randomCharIndex];
                    break;
                case GameMode.Hiragana:
                    currentCharacterPrompt = syllabariesData.hiraganaList[randomCharIndex];
                    break;
                case GameMode.Mix:
                    if (Random.value < 0.5f)
                    {
                        currentCharacterPrompt = syllabariesData.katakanaList[randomCharIndex];
                    }
                    else
                    {
                        currentCharacterPrompt = syllabariesData.hiraganaList[randomCharIndex];
                    }
                    break;
            }

            // Display Romaji on the canvas in front of the player
            

            // Assign the character to the correct button and activate it
            ActivateButtonWithCharacter(currentCharacterPrompt);

            // If you want to activate other distraction buttons, you can add that logic here...
        }
    }

    private void ActivateButtonWithCharacter(string character)
    {
        // Activate the correct button
        int correctButtonIndex = Random.Range(0, _wackButton.Count);
        _wackButton[correctButtonIndex].Character = character;
        _wackButton[correctButtonIndex].Activate();

    

        HashSet<int> activatedIndexes = new HashSet<int> { correctButtonIndex }; // Using HashSet to store unique indices

        List<string> distractionChars = new List<string>();
        switch (currentMode)
        {
            case GameMode.Katakana:
                distractionChars.AddRange(syllabariesData.katakanaList);
                break;
            case GameMode.Hiragana:
                distractionChars.AddRange(syllabariesData.hiraganaList);
                break;
            case GameMode.Mix:
                distractionChars.AddRange(syllabariesData.katakanaList);
                distractionChars.AddRange(syllabariesData.hiraganaList);
                break;
        }
        // Ensure the distraction characters don't include the correct answer.
        distractionChars.Remove(character);

        // Activate exactly 4 other buttons as "distractions"
        for (int i = 0; i < 4; i++)
        {
            int distractionIndex;
            string distractionChar;

            do
            {
                distractionIndex = Random.Range(0, _wackButton.Count);
                distractionChar = distractionChars[Random.Range(0, distractionChars.Count)]; // Pick a random character
            } while (activatedIndexes.Contains(distractionIndex) || distractionChar == character); // Ensure we don't choose an already activated button or the correct character

            activatedIndexes.Add(distractionIndex); // Add to our set
            _wackButton[distractionIndex].Character = distractionChar; // Assign random character
            _wackButton[distractionIndex].Activate();
        }

    }

    List<int> _RollIndex(int amount)
    {
        List<int> indexes = new List<int>();
        int rolledIndex; 

        while (indexes.Count < amount)
        {
            rolledIndex = Random.Range(0, _wackButton.Count);
            if (!indexes.Contains(rolledIndex))
            {
                indexes.Add(rolledIndex);
                
            }

        }

        return indexes; 
    }

    void _PrintScore()
    {
        _scoreText.text = _score.ToString();
    }
    void _CheckBestScore()
    {
        if (_score > _bestScore)
        {
            _bestScore = _score;
            _bestScoreText.text = _bestScore.ToString();

        }

    }

    void _ResetScore()
    {
        _score = 0;
        _activeButtons = 0;
        _PrintScore(); 
    }

   void _StartTimer()
    {
        _ResetTimer();
        _timeTickCoroutine = StartCoroutine(_TimeTick());
    }

    IEnumerator _TimeTick()
    {
        float tick = 1f;
        WaitForSeconds wait = new WaitForSeconds(tick);
        float startTime = Time.time; 

        while (!_gameEnded)
        {
            _gameTimeLeft = _gameTimeLeft - tick;
            _PrintTime();

            if (_gameTimeLeft <= 0)
            {
                EndGame(); 
            }
            yield return wait; 
        }
    }
    void _ResetTimer()
    {
        if (_timeTickCoroutine != null)
            StopCoroutine(_timeTickCoroutine);

        _gameTimeLeft = _gameTime;
        _PrintTime();
       
    }
    void _PrintTime()
    {
        _timeText.text = _gameTimeLeft.ToString("0");
    }


}
