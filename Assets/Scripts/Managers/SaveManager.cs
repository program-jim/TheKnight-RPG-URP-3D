using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : SingletonMono<SaveManager>
{
    public bool isJsonPretty = false;
    private string sceneName = null;
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        
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
