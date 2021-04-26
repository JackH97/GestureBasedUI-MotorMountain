using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;
using System.IO;
using System.Text;  // for stringbuilder

// Includes the Voice reconigiction methods to work from the .xml files

public class MainMenu : MonoBehaviour
{
     private GrammarRecognizer gr;

    private string KeyWord = "";

    private void Awake()
    {
        gr = new GrammarRecognizer(Path.Combine(Application.streamingAssetsPath,
                                                "MainMenuGrammar.xml"),
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
        // Move pawn from C2 to C4 - Piece, Start, Finish
        // semantic meanings are returned as key/value pairs
        // Piece/"pawn", Start/"C2", Finish/"C4"
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
    private void Update()
    {
        // Keywords to recieve from the .xml file
        switch (KeyWord)
        {
            case "new":
                PlayGame();
                break;
            case "option":
                OptionsScreen();
                break;
            case "quit":
                QuitGame();
                break;
        }
    }

    private void OnApplicationQuit()
    {
        if (gr != null && gr.IsRunning)
        {
            gr.OnPhraseRecognized -= GR_OnPhraseRecognized;
            gr.Stop();
        }
    }


    // Start is called before the first frame update
    // the methods to change scenes or quit game
    public void PlayGame ()
    {
        SceneManager.LoadScene("Game");
    }
     public void OptionsScreen()
    {
        SceneManager.LoadScene("Options");
    }

    public void QuitGame()
    {
        Debug.Log("Game Quited");
        Application.Quit();
    }
}
