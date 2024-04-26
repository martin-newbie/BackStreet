using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Sweeper : Enemy
{
    public RuntimeAnimatorController[] sweeperVariation;
    public Animator animator;

    public override void InitEnemy(Transform _target, Action<Enemy> retire)
    {
        base.InitEnemy(_target, retire);
        animator.runtimeAnimatorController = sweeperVariation[Random.Range(0, sweeperVariation.Length)];
    }
}
