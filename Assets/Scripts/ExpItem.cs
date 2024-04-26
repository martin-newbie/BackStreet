using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpItem : MonoBehaviour
{
    float reachTime = 1f;
    bool isMove = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isMove && collision.CompareTag("Player"))
        {
            isMove = true;
            StartCoroutine(MoveToPlayer(collision.transform));
        }
    }

    IEnumerator MoveToPlayer(Transform target)
    {
        float timer = 0f;
        Vector3 startPos = transform.position;

        while (timer < reachTime)
        {
            // move to player position
            timer += Time.deltaTime;
            float x = timer / reachTime;
            transform.position = Vector3.Lerp(startPos, target.position, 1 - Mathf.Cos((x * Mathf.PI) / 2));

            if (Vector3.Distance(transform.position, target.position) < 0.5f)
            {
                // already near
                break;
            }

            yield return null;
        }

        // move end, reach to player
        InGameManager.Instance.IncreaseExp(1);
        Destroy(gameObject);
    }
}
