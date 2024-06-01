using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    int focusIndex;
    public RectTransform buttonFocus;
    public RectTransform[] buttonsRect;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            focusIndex--;
            if(focusIndex < 0)
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
