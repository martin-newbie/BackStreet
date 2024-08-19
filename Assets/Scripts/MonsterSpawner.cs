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

    Enemy[] enemyPrefabs;
    bool gameActive = false;

    public IStage stageBehaviour;

    public void StartWave(int stageIdx)
    {
        // 333... 1978
        int count = StaticDataManager.Instance.waveData.datas.Count;
        enemyPrefabs = new Enemy[count];
        for (int i = 0; i < count; i++)
        {
            enemyPrefabs[i] = Resources.Load<Enemy>($"Prefabs/Enemies/{i}");
        }

        switch (stageIdx)
        {
            case 0:
                stageBehaviour = new StageCleaners();
                break;
            default:
                break;
        }

        stageBehaviour.Init(this);
        gameActive = true;
    }

    void Update()
    {
        if (!gameActive) return;

        SpawnLogic(InGameManager.Instance.playTime);
    }

    void SpawnLogic(float gameTime)
    {
        stageBehaviour.Spawn(gameTime);
    }

    public Enemy SpawnEnemy(int idx)
    {
        var randomPos = InGameManager.Instance.curPlayer.transform.position + (Vector3)(Random.insideUnitCircle * 15f);
        return Instantiate(enemyPrefabs[idx], randomPos, Quaternion.identity);
    }

    public void StopSpawn()
    {
        gameActive = false;
    }
}

public interface IStage
{
    void Init(MonsterSpawner manager);
    void Spawn(float gameTime);
}