using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextManager : MonoBehaviour
{

    public static DamageTextManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public TextMesh damageText;
    Stack<TextMesh> textPool = new Stack<TextMesh>();

    private void Start()
    {
        int allocCount = 100;
        for (int i = 0; i < allocCount; i++)
        {
            var text = Instantiate(damageText, transform);
            text.gameObject.SetActive(false);
            text.GetComponent<MeshRenderer>().sortingOrder = 100;
            textPool.Push(text);
        }
    }

    public void PrintText(Vector3 pos, int damage, int dir)
    {
        var text = textPool.Pop();
        text.gameObject.SetActive(true);
        text.text = damage.ToString("N0");
        StartCoroutine(TextMove(text, pos, dir));
    }

    IEnumerator TextMove(TextMesh obj, Vector3 pos, int dir)
    {
        Vector3 targetPos = pos + new Vector3(0.5f, 0, 0) * dir;
        float timer = 0f;
        float dur = 0.5f;
        while (timer < dur)
        {
            obj.transform.position = GetParabola(pos, targetPos, 0.5f, timer / dur);
            timer += Time.deltaTime;
            yield return null;
        }
        obj.gameObject.SetActive(false);
        textPool.Push(obj);
        yield break;
    }

    public Vector3 GetParabola(Vector3 a, Vector3 b, float height, float time)
    {
        float target_X = a.x + (b.x - a.x) * time;
        float target_Y = a.y + ((b.y - a.y)) * time + height * (1 - (Mathf.Abs(0.5f - time) / 0.5f) * (Mathf.Abs(0.5f - time) / 0.5f));
        return new Vector3(target_X, target_Y);
    }

}
