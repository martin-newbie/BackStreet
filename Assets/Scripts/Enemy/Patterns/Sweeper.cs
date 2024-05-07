using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Sweeper : Enemy
{
    public RuntimeAnimatorController[] animVariation;

    public override void InitEnemy(EnemyData _enemyData, Transform _target, Action<Enemy> retire)
    {
        base.InitEnemy(_enemyData, _target, retire);
        animator.runtimeAnimatorController = animVariation[Random.Range(0, animVariation.Length)];
    }
}
