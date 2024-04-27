using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public float moveSpeed = 3f;

    protected int damage = 1;
    protected bool isInit = false;
    protected bool isAlive;

    protected Transform target;
    protected SpriteRenderer sprite;
    protected Action<Enemy> retireAction;

    public virtual void InitEnemy(Transform _target, Action<Enemy> retire)
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

        MoveLogic();
    }

    protected virtual void MoveLogic()
    {
        var dist = Vector3.Distance(target.position, transform.position);
        if (dist < 0.5f)
        {
            return;
        }

        var dir = (target.position - transform.position).normalized;
        transform.Translate(dir * Time.deltaTime * moveSpeed);

        sprite.sortingOrder = InGameManager.Instance.GetDrawOrder((int)transform.position.y);
        sprite.flipX = dir.x > 0;
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (!isAlive) return;

        if (collision.CompareTag("Player"))
        {
            TriggerStayWithPlayer(collision);
        }
    }

    protected virtual void TriggerStayWithPlayer(Collider2D collision)
    {
        // TODO : Replace yan to player class when it's ready
        var player = collision.GetComponent<Player>();
        player.OnDamage(this, damage);
    }

    public override void OnDamage(Entity from, int damage)
    {
        if (!isAlive) return;

        hp -= damage;
        int dir = transform.position.x > from.transform.position.x ? 1 : -1;
        DamageTextManager.Instance.PrintText(transform.position, (int)damage, dir);

        if (hp <= 0)
        {
            isAlive = false;
            retireAction?.Invoke(this);
        }
    }
}
