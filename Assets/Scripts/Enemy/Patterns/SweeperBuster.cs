using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweeperBuster : IMovementPattern
{
    Vector3 moveDir;
    BusterExplosion explosive;
    int moveState;

    float rangeInside = 8f;
    float rangeOutside = 11f;

    Enemy subject;

    public void Init(Enemy subject)
    {
        this.subject = subject;
        subject.retireAction += RetireExplosion;
        subject.animator.runtimeAnimatorController = ResourceManager.Instance.GetEnemyAnim(subject.enemyData.monsterModel);
        explosive = ResourceManager.Instance.explosion;
    }

    public void Movement(Transform target, Transform transform, SpriteRenderer sprite, float moveSpeed)
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
        moveDir = (subject.target.position - subject.transform.position).normalized;

        if (Vector3.Distance(subject.transform.position, subject.target.position) < rangeInside)
        {
            moveState = 1;
        }
    }

    void InsideRange()
    {
        if (Vector3.Distance(subject.transform.position, subject.target.position) > rangeOutside)
        {
            moveState = 0;
        }
    }


    public void DamageTo(Player player)
    {
        subject.retireAction?.Invoke(subject);
    }


    void RetireExplosion(Enemy subject)
    {
        subject.isAlive = false;
        var expl = Object.Instantiate(explosive, subject.transform.position, Quaternion.identity);
        expl.SetExplosion(subject, subject.damage);
    }
}
