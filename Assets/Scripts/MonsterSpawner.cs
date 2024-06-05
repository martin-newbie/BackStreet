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
    List<WaveData> waveDatas = new List<WaveData>();

    bool gameActive = false;

    public void StartWave()
    {
        gameActive = true;
        // 333... 1978
        for (int i = 0; i < 13; i++)
        {
            waveDatas.Add(StaticDataManager.GetWaveData(i));
        }
    }

    void Update()
    {
        if (!gameActive) return;

        for (int i = 0; i < waveDatas.Count; i++)
        {
            SpawnLogic(InGameManager.Instance.playTime, waveDatas[i]);
        }
    }

    void SpawnLogic(float gameTime, WaveData spawnData)
    {
        if (spawnData.GetSpawnProba(gameTime))
        {
            var enemyData = StaticDataManager.GetEnemyData(spawnData.enemyIdx);
            var obj = enemyPrefabs[enemyData.prefabIndex];
            var spawnPos = InGameManager.Instance.curPlayer.transform.position + (Vector3)(Random.insideUnitCircle.normalized * 15f);
            var enemy = Instantiate(obj, spawnPos, Quaternion.identity);
            enemy.InitEnemy(enemyData, InGameManager.Instance.curPlayer.transform, CommonEnemyRetireAction);
            curEnemy.Add(enemy);
        }
    }

    void CommonEnemyRetireAction(Enemy subject)
    {
        var pos = subject.transform.position;
        InGameManager.Instance.SpawnExpItem(pos);
        curEnemy.Remove(subject);
        Destroy(subject.gameObject);
    }

    public void StopSpawn()
    {
        gameActive = false;
    }
}