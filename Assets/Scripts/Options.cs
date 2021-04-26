using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;  // for stringbuilder
using UnityEngine.Windows.Speech;

public class Options : MonoBehaviour
{
    private bool isMuted;

    private GrammarRecognizer gr;

    private string KeyWord = "";

    private void Awake()
    {
        gr = new GrammarRecognizer(Path.Combine(Application.streamingAssetsPath,
                                                "OptionsMenuGrammar.xml"),
                                    ConfidenceLevel.Low);
        Debug.Log("Grammar loaded!");
        gr.OnPhraseRecognized += GR_OnPhraseRecognized;
        gr.Start();
        if (gr.IsRunning) Debug.Log("Recogniser running");
        KeyWord = "";
    }

    private void GR_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        StringBuilder message = new StringBuilder();
        Debug.Log("Recognised a phrase");
        // read the semantic meanings from the args passed in.
        SemanticMeaning[] meanings = args.semanticMeanings;
        // 
        // use foreach to get all the meanings.
        foreach (SemanticMeaning meaning in meanings)
        {
            string keyString = meaning.key.Trim();
            string valueString = meaning.values[0].Trim();
            message.Append("Key: " + keyString + ", Value: " + valueString + " ");
            KeyWord = valueString;
        }
        // use a string builder to create the string and out put to the user
        Debug.Log(message);
    }

    private void OnApplicationQuit()
    {
        if (gr != null && gr.IsRunning)
        {
            gr.OnPhraseRecognized -= GR_OnPhraseRecognized;
            gr.Stop();
        }
    }

    private void Update()
    {
        switch (KeyWord)
        {
            case "mute":
                MutePressed();
                break;
            case "exit":
                MainMenuScene();
                break;
        }
    }
    

    void Start()
    {
        isMuted = PlayerPrefs.GetInt("MUTED") == 1;
        AudioListener.pause = isMuted;
    }

    // Used to mute the audio when pressing the toggle button in options menu
    public void MutePressed()
    {
        isMuted = !isMuted;
        AudioListener.pause = isMuted;
        PlayerPrefs.SetInt("MUTED", isMuted ? 1 : 0);
    }

    // Used to return to the Main Menu Scene when hitting the exit button
    public void MainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
