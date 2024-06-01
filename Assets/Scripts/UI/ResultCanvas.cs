using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultCanvas : MonoBehaviour
{

    public Text killCountTxt;
    public Text playTimeTxt;

    public void OpenPopup()
    {
        gameObject.SetActive(true);
        int killCnt = InGameManager.Instance.killCount;
        float playSec = InGameManager.Instance.playTime;

        killCountTxt.text = killCnt.ToString("N0");
        TimeSpan time = TimeSpan.FromSeconds(playSec);
        playTimeTxt.text = time.ToString("hh:mm:ss");
    }

    public void OnBackToTitle()
    {

    }

    public void OnReplay()
    {

    }
}
