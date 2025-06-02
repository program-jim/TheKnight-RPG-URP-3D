using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SceneController : SingletonMono<SceneController>, IEndGameObserver
{
    public GameObject playerPrefab;
    private GameObject player;
    public SceneFader sceneFaderPrefab;
    private NavMeshAgent playerAgent;

    public string firstSceneName = "Game";
    public Vector3 startGameScenePlayerPosition = new Vector3(-30.1f, 0f, -30.72f);
    public Vector3 startGameScenePlayerRotationEulers = Vector3.zero;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void TransitionToFirstLevel()
    {
        StartCoroutine(LoadLevel(firstSceneName));
    }

    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        switch (transitionPoint.transitionType)
        {
            case TransitionType.SameScene:
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationType));
                break;

            case TransitionType.DifferentScene:
                StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationType));
                break;
        }
    }

    public void TransitionToMain()
    {
        StartCoroutine(LoadMainLevel());
    }

    public void TransitionToLoadGame()
    {
        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));
    }

    IEnumerator Transition(string sceneName, DestinationType destinationType)
    {
        // Save data.
        SaveManager.Instance.SavePlayerData();
        
        if (sceneName != SceneManager.GetActiveScene().name)
        {
            // FIXME: Can add SceneFader.
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return Instantiate(playerPrefab, GetTransitionDestination(destinationType).transform.position, GetTransitionDestination(destinationType).transform.rotation);
            
            // Load data.
            SaveManager.Instance.LoadPlayerData();

            yield break;
        }
        else
        {
            player = GameManager.Instance.playerStates.gameObject;
            playerAgent = player.GetComponent<NavMeshAgent>();

            // Turn off player's agent.
            playerAgent.enabled = false;
            player.transform.SetPositionAndRotation(GetTransitionDestination(destinationType).transform.position, GetTransitionDestination(destinationType).transform.rotation);

            // Turn on player's agent back.
            playerAgent.enabled = true;
            yield return null;
        }
    }

    IEnumerator LoadLevel(string scene)
    {
        if (scene == null)
        {
            yield break;
        }

        var fade = Instantiate(sceneFaderPrefab);
        yield return StartCoroutine(fade.FadeOut(2.5f));
        yield return SceneManager.LoadSceneAsync(scene);

        // TODO: The position to get can be replaced with GameManager.Instance.GetEntrance()
        yield return player = Instantiate(playerPrefab, startGameScenePlayerPosition, Quaternion.Euler(startGameScenePlayerRotationEulers));

        // Save player data
        SaveManager.Instance.SavePlayerData();
        yield return StartCoroutine(fade.FadeIn(2.5f));
        yield break;
    }

    IEnumerator LoadMainLevel()
    {
        var fade = Instantiate(sceneFaderPrefab);
        yield return StartCoroutine(fade.FadeOut(2.5f));
        yield return SceneManager.LoadSceneAsync("Main");
        yield return StartCoroutine(fade.FadeIn(2.5f));
        yield break;
    }

    private TransitionDestination GetTransitionDestination(DestinationType destinationType)
    {
        var entrances = FindObjectsOfType<TransitionDestination>();

        foreach (var item in entrances)
        {
            if (item.destinationType == destinationType)
            {
                return item;
            }
        }

        return null;
    }

    public void EndNotify()
    {
        throw new System.NotImplementedException();
    }
}
