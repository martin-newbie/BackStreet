using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public float moveSpeed = 3f;

    float damage = 1f;
    bool isInit = false;
    bool isAlive;

    Transform target;
    SpriteRenderer sprite;
    Action<Enemy> retireAction;

    public void InitEnemy(Transform _target, Action<Enemy> retire)
    {
        target = _target;
        sprite = GetComponent<SpriteRenderer>();
        isInit = true;
        isAlive = true;
        retireAction = retire;
    }

    private void Update()
    {
        if (!isInit) return;
        if (!isAlive) return;

        var dist = Vector3.Distance(target.position, transform.position);
        if (dist < 0.5f) return;

        var dir = (target.position - transform.position).normalized;
        transform.Translate(dir * Time.deltaTime * moveSpeed);

        sprite.flipX = dir.x > 0;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isAlive) return;

        if (collision.CompareTag("Player"))
        {
            // TODO : Replace yan to player class when it's ready
            var player = collision.GetComponent<Player>();
            player.OnDamage(this, damage);
        }
    }

    public override void OnDamage(Entity from, float damage)
    {
        if (!isAlive) return;

        hp -= damage;
        int dir = transform.position.x > from.transform.position.x ? 1 : -1;
        DamageTextManager.Instance.PrintText(transform.position, (int)damage, dir);
        
        if(hp <= 0)
        {
            isAlive = false;
            retireAction?.Invoke(this);
        }
    }
}
