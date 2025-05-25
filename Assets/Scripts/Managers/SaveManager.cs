using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : SingletonMono<SaveManager>
{
    public bool isJsonPretty = false;
    private string sceneName = "Game";

    public string SceneName
    {
        get
        {
            return PlayerPrefs.GetString(sceneName);
        }
    }
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
    #if UNITY_EDITOR
        TestInput();
    #endif
    }

    public void TestInput()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            SavePlayerData();
            Debug.Log("Saved player data in editor");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadPlayerData();
            Debug.Log("Loaded player data in editor");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Transfering to Main Scene in editor");
            SceneController.Instance.TransitionToMain();
        }
    }

    public void Save(string key, object data)
    {
        var json = JsonUtility.ToJson(data, isJsonPretty);
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
    }

    public void Load(string key, object data)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }

    public void SavePlayerData()
    {
        Debug.Log("Save Player Data.");
        Save(GameManager.Instance.playerStates.characterData.name, GameManager.Instance.playerStates.characterData);
    }

    public void LoadPlayerData()
    {
        Debug.Log("Load Player Data.");
        Load(GameManager.Instance.playerStates.characterData.name, GameManager.Instance.playerStates.characterData);
    }
}
