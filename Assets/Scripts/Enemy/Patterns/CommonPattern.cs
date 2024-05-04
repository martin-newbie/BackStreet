using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonPattern : IMovementPattern
{
    public void Init(Enemy subject)
    {
        subject.animator.runtimeAnimatorController = ResourceManager.Instance.GetEnemyAnim(subject.enemyData.monsterModel);
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
}
