using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class MainMenu : MonoBehaviour
{
    Button newGameButton;
    Button continueGameButton;
    Button quitGameButton;

    PlayableDirector director;

    private void Awake()
    {
        newGameButton = transform.GetChild(1).GetComponent<Button>();
        continueGameButton = transform.GetChild(2).GetComponent<Button>();
        quitGameButton = transform.GetChild(3).GetComponent<Button>();

        director = FindObjectOfType<PlayableDirector>();
        director.stopped += NewGame;

        newGameButton.onClick.AddListener(PlayTimeline);
        continueGameButton.onClick.AddListener(ContinueGame);
        quitGameButton.onClick.AddListener(QuitGame);
    }

    public void NewGame(PlayableDirector director)
    {
        // Delete All PlayerPrefs data
        PlayerPrefs.DeleteAll();
        Debug.Log("Deleted All PlayerPrefs data !!!");
        // Scene switching
        Debug.Log("Transfering To First Level !!!");
        SceneController.Instance.TransitionToFirstLevel();
    }

    public void ContinueGame()
    {
        // Scene switching and read PlayerPrefs data.
        SceneController.Instance.TransitionToLoadGame();
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void PlayTimeline()
    {
        director.Play();
    }
}
