using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCleaners : IStage
{
    int[] spawnMonsters = new int[3] { 0, 1, 2 };
    MonsterSpawner manager;
    List<Enemy> curEnemy;

    public void Init(MonsterSpawner manager)
    {
        this.manager = manager;

        curEnemy = new List<Enemy>();
    }

    public void Spawn(float gameTime)
    {
        foreach (var idx in spawnMonsters)
        {
            var monsterData = StaticDataManager.GetMonsterData(idx);
            if (monsterData.GetSpawnProb(gameTime))
            {
                var enemy = manager.SpawnEnemy(monsterData.index);
                enemy.InitEnemy(monsterData, InGameManager.Instance.curPlayer.transform, CommonRetire);
                curEnemy.Add(enemy);
            }
        }
    }

    void CommonRetire(Enemy subject)
    {
        var pos = subject.transform.position;
        InGameManager.Instance.SpawnExpItem(pos);
        curEnemy.Remove(subject);
        Object.Destroy(subject.gameObject);
    }
}
