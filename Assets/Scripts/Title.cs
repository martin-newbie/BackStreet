using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{

    public static Title Instance;
    private void Awake()
    {
        Instance = this;
    }

    public Text loadingText;

    int focusIndex;
    public RectTransform buttonFocus;
    public RectTransform[] buttonsRect;

    public GameObject Loading;
    public GameObject Buttons;

    public bool isDataInit;

    string[] loadingTextStr = new string[]
    {
        ".",
        "..",
        "...",
    };

    private void Update()
    {
        if (isDataInit)
        {
            ButtonMovement();
        }
    }

    public IEnumerator DataLoading()
    {
        int idx = 0;
        while (true)
        {
            loadingText.text = "데이터를 로딩중입니다" + loadingTextStr[idx % 3];
            idx++;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void ShowButtons()
    {
        isDataInit = true;
        Loading.SetActive(false);
        Buttons.SetActive(true);
    }

    void ButtonMovement()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            focusIndex--;
            if (focusIndex < 0)
            {
                focusIndex += buttonsRect.Length;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            focusIndex++;
        }

        int clampedFocusIdx = Mathf.Abs(focusIndex % buttonsRect.Length);
        var targetButton = buttonsRect[clampedFocusIdx];
        buttonFocus.anchoredPosition = Vector2.Lerp(buttonFocus.anchoredPosition, targetButton.anchoredPosition, Time.deltaTime * 25f);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Insert))
        {
            switch (clampedFocusIdx)
            {
                case 0:
                    GameStart();
                    break;
                case 1:
                    break;
                case 2:
                    GameQuit();
                    break;
                default:
                    break;
            }
        }
    }

    public void GameStart()
    {
        SceneManager.LoadScene("Main");
    }
    public void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
