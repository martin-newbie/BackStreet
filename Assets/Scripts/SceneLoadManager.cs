using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager Instance;
    public string moveSceneName;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static void LoadScene(string sceneName, Action onComplete = null)
    {
        Instance.moveSceneName = sceneName;
        Instance.StartCoroutine(Instance.LoadSceneAsync(onComplete));
    }

    public IEnumerator LoadSceneAsync(Action onComplete)
    {
        var loadData = SceneManager.LoadSceneAsync(moveSceneName);
        loadData.allowSceneActivation = false;
        while (!loadData.isDone)
        {
            // scene progress
            float progress = loadData.progress;
            if (progress >= 0.9f)
            {
                break;
            }
            yield return null;
        }

        loadData.allowSceneActivation = true;
        yield return new WaitUntil(() => SceneManager.GetSceneByName(moveSceneName).isLoaded);
        onComplete?.Invoke();

        yield break;
    }
}
