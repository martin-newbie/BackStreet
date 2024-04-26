using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpGaugeBox : MonoBehaviour
{
    public Text expText;
    public Image expGauge;

    public void SetGauge(int cur, int max)
    {
        expText.text = string.Format("{0}/{1}", cur.ToString("N0"), max.ToString("N0"));
        expGauge.fillAmount = (float)cur / (float)max;
    }
}
