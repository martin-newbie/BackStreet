using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sweeper : IMovementPattern
{
    Enemy subject;

    public void Init(Enemy _subject)
    {
        subject = _subject;
        subject.animator.runtimeAnimatorController = ResourceManager.Instance.GetSweeperAnim();
    }

    public void Movement(Transform target, Transform transform, SpriteRenderer sprite, float moveSpeed)
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

    public void DamageTo(Player player)
    {
        player.OnDamage(subject, subject.damage);
    }
}
