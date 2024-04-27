using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweeperBuster : Enemy
{
    Vector3 moveDir;
    public BusterExplosion explosive;
    int moveState;

    float rangeInside = 8f;
    float rangeOutside = 11f;

    public override void InitEnemy(Transform _target, Action<Enemy> retire)
    {
        base.InitEnemy(_target, retire);
    }

    protected override void MoveLogic()
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
        if(Vector3.Distance(transform.position, target.position) > rangeOutside)
        {
            moveState = 0;
        }
    }

    protected override void TriggerStayWithPlayer(Collider2D collision)
    {
        retireAction?.Invoke(this);
        RetireExplosion();
    }

    void RetireExplosion()
    {
        isAlive = false;
        var expl = Instantiate(explosive, transform.position, Quaternion.identity);
        expl.SetExplosion(this, damage);
    }
}
