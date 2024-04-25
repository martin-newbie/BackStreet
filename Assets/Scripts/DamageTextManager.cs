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
        Vector3 targetPos = pos + Vector3.right * dir;
        float timer = 0f;
        float dur = 1f;
        while (timer < dur)
        {
            obj.transform.position = Vector3.Lerp(pos, targetPos, 1 - Mathf.Pow(1 - (timer / dur), 3));
            timer += Time.deltaTime;
            yield return null;
        }
        obj.gameObject.SetActive(false);
        textPool.Push(obj);
        yield break;
    }
}
