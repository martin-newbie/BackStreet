using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldHpBar : MonoBehaviour
{
    float gaugeWidth = 1.1f;
    float gaugeHeight = 0.09f;

    [Header("Draw")]
    public SpriteRenderer gauge;
    public SpriteRenderer gaugeMoving;

    Coroutine gaugeRoutine;

    public void StartHpMove(float max, float prev, float cur)
    {
        gameObject.SetActive(true);
        float fill = cur / max;
        float prevFill = prev / max;
        gauge.size = new Vector2(gaugeWidth * fill, gaugeHeight);
        // gaugeMoving.size = new Vector2(gaugeWidth * prevFill, gaugeHeight);

        if (gaugeRoutine != null) StopCoroutine(gaugeRoutine);
        gaugeRoutine = StartCoroutine(GaugeGetLow(fill));
    }

    IEnumerator GaugeGetLow(float targetFill)
    {
        var startSize = gaugeMoving.size;
        var endSize = new Vector2(gaugeWidth * targetFill, gaugeHeight);

        float dur = 0.5f;
        float timer = 0f;
        while (timer < dur)
        {
            gaugeMoving.size = Vector2.Lerp(startSize, endSize, timer / dur);
            timer += Time.deltaTime;
            yield return null;
        }

        gaugeMoving.size = endSize;
        yield return new WaitForSeconds(0.1f);
        yield break;
    }
}
