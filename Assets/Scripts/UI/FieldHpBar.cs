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

    Vector2 gaugeFull => new Vector2(gaugeWidth , gaugeHeight);
    Vector2 gaugeEmpty => new Vector2(0, gaugeHeight);

    Coroutine gaugeRoutine;

    public void StartHpMove(float max, float prev, float cur)
    {
        gameObject.SetActive(true);
        float fill = cur / max;
        gauge.size = Vector2.Lerp(gaugeEmpty, gaugeFull, fill);

        if (gaugeRoutine != null) StopCoroutine(gaugeRoutine);
        gaugeRoutine = StartCoroutine(GaugeGetLow(fill));
    }

    IEnumerator GaugeGetLow(float targetFill)
    {
        var startSize = gaugeMoving.size;
        var endSize = Vector2.Lerp(gaugeEmpty, gaugeFull, targetFill);

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
