using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;

    float damage = 1f;
    bool isInit = false;

    Transform target;
    SpriteRenderer sprite;

    public void InitEnemy(Transform _target)
    {
        target = _target;
        sprite = GetComponent<SpriteRenderer>();
        isInit = true;
    }

    private void Update()
    {
        if (!isInit) return;

        var dist = Vector3.Distance(target.position, transform.position);
        if (dist < 0.5f) return;

        var dir = (target.position - transform.position).normalized;
        transform.Translate(dir * Time.deltaTime * moveSpeed);

        sprite.flipX = dir.x > 0;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // TODO : Replace yan to player class when it's ready
            var player = collision.GetComponent<Yan>();
            // player.Ondamage(damage);
        }
    }
}
