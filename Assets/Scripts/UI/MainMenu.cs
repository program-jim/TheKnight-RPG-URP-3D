using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    Button newGameButton;
    Button continueGameButton;
    Button quitGameButton;

    private void Awake()
    {
        newGameButton = transform.GetChild(1).GetComponent<Button>();
        continueGameButton = transform.GetChild(2).GetComponent<Button>();
        quitGameButton = transform.GetChild(3).GetComponent<Button>();

        newGameButton.onClick.AddListener(NewGame);
        continueGameButton.onClick.AddListener(ContinueGame);
        quitGameButton.onClick.AddListener(QuitGame);
    }

    public void NewGame()
    {
        // Delete All PlayerPrefs data
        PlayerPrefs.DeleteAll();
        Debug.Log("Deleted All PlayerPrefs data !!!");

        // Scene switching


    }

    public void ContinueGame()
    {
        // Scene switching and read PlayerPrefs data.

    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
