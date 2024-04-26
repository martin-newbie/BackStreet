using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpGaugeBox : MonoBehaviour
{
    public Image hpGauge;

    public void SetGauge(int cur, int max)
    {
        hpGauge.fillAmount = (float)cur / (float)max;
    }
}
