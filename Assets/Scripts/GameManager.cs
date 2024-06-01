using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneLoadManager.LoadScene("Title", () =>
        {
            StartCoroutine(Load());
        });
    }

    IEnumerator Load()
    {
        var routine = StartCoroutine(Title.Instance.DataLoading());
        yield return StaticDataManager.Instance.InitDatas();
        Title.Instance.ShowButtons();
        StopCoroutine(routine);
    }
}
