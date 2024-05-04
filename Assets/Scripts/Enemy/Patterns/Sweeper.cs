using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sweeper : IMovementPattern
{
    public void Init(Enemy subject)
    {
        subject.animator.runtimeAnimatorController = ResourceManager.Instance.GetSweeperAnim();
    }

    public void Movement(Transform target, Transform transform, SpriteRenderer sprite, float moveSpeed)
    {
        throw new System.NotImplementedException();
    }
}
