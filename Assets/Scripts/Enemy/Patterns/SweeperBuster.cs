using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweeperBuster : Enemy
{
    public BusterExplosion explosion;

    int moveState = 0;
    Vector2 moveDir;

    float rangeInside = 8f;
    float rangeOutside = 11f;

    public override void InitEnemy(EnemyData _enemyData, Transform _target, Action<Enemy> retire)
    {
        base.InitEnemy(_enemyData, _target, retire);
        retireAction += RetireExplosion;
    }

    public void Movement()
    {
        switch (moveState)
        {
            case 0:
                BeforeInside();
                break;
            case 1:
                InsideRange();
                break;
        }

        transform.Translate(moveDir * Time.deltaTime * moveSpeed);
        sprite.flipX = moveDir.x > 0;
    }

    void BeforeInside()
    {
        moveDir = (target.position - transform.position).normalized;

        if (Vector3.Distance(transform.position, target.position) < rangeInside)
        {
            moveState = 1;
        }
    }

    void InsideRange()
    {
        if (Vector3.Distance(transform.position, target.position) > rangeOutside)
        {
            moveState = 0;
        }
    }

    public override void OnDamage(Entity from, int damage)
    {
        base.OnDamage(from, damage);
        retireAction?.Invoke(this);
    }

    void RetireExplosion(Enemy subject)
    {
        isAlive = false;
        Instantiate(explosion, subject.transform.position, Quaternion.identity);
    }
}
