using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public static MonsterSpawner Instance = null;
    private void Awake()
    {
        Instance = this;
    }

    public Enemy[] enemyPrefabs;

    List<Enemy> curEnemy = new List<Enemy>();
    List<SpawnData> waveDatas = new List<SpawnData>();

    void Start()
    {
        // 333... 1978
        waveDatas.Add(new SpawnData(0, 0, 0, 120f, 150f, 0));
        waveDatas.Add(new SpawnData(1, 1, 15f, 120f, 100f, 0));
        waveDatas.Add(new SpawnData(2, 2, 120f, 120f, 100f, 1));
    }

    void Update()
    {
        for (int i = 0; i < waveDatas.Count; i++)
        {
            SpawnLogic(InGameManager.Instance.playTime, waveDatas[i]);
        }
    }

    void SpawnLogic(float gameTime, SpawnData spawnData)
    {
        bool bossWaveAble = spawnData.isBoss && spawnData.spawnTime <= gameTime;

        if (spawnData.GetSpawnProba(gameTime) || bossWaveAble)
        {
            var enemyData = GetEnemeyData(spawnData.enemyIdx);
            var obj = enemyPrefabs[enemyData.movementPattern];
            var spawnPos = InGameManager.Instance.curPlayer.transform.position + (Vector3)(Random.insideUnitCircle.normalized * 15f);
            var enemy = Instantiate(obj, spawnPos, Quaternion.identity);
            enemy.InitEnemy(enemyData, InGameManager.Instance.curPlayer.transform, CommonEnemyRetireAction);
            curEnemy.Add(enemy);

            if (spawnData.isBoss)
            {
                waveDatas.Remove(spawnData);
                enemy.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
            }
        }
    }

    void CommonEnemyRetireAction(Enemy subject)
    {
        var pos = subject.transform.position;
        InGameManager.Instance.SpawnExpItem(pos);
        curEnemy.Remove(subject);
        Destroy(subject.gameObject);
    }

    EnemyData GetEnemeyData(int enemyIdx)
    {
        switch (enemyIdx)
        {
            case 0:
                return new EnemyData(0, 1, 0.4f, 3f, 1, 3, 1);
            case 1:
                return new EnemyData(0, 2, 0.4f, 5f, 3, 2, 1);
            case 2:
                return new EnemyData(1, 0, 0.6f, 3f, 2, 150, 2);
            default:
                return null;
        }
    }
}