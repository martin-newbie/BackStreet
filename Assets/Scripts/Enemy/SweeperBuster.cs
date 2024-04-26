using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweeperBuster : Enemy
{
    Vector3 moveDir;
    public BusterExplosion explosive;

    public override void InitEnemy(Transform _target, Action<Enemy> retire)
    {
        base.InitEnemy(_target, retire);
        moveDir = (target.position - transform.position).normalized;
        retireAction += RetireExplosion;
    }

    protected override void MoveLogic()
    {
        transform.Translate(moveDir * Time.deltaTime * moveSpeed);
        sprite.flipX = moveDir.x > 0;
    }

    protected override void TriggerStayWithPlayer(Collider2D collision)
    {
        retireAction?.Invoke(this);
    }

    void RetireExplosion(Entity subject)
    {
        isAlive = false;
        var expl = Instantiate(explosive, transform.position, Quaternion.identity);
        expl.SetExplosion(this, damage);
    }
}
