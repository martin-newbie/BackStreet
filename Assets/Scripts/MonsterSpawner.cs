using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{

    public Enemy[] enemies;

    List<Enemy> curEnemy = new List<Enemy>();
    SpawnData commonWave;

    void Start()
    {
        // 333... 1978
        commonWave = new SpawnData(0, 0, 0, 80f, 20f, 5f, 80f);
    }

    void Update()
    {
        SpawnLogic(InGameManager.Instance.playTime);
    }

    void SpawnLogic(float gameTime)
    {
        if (commonWave.GetSpawnProba(gameTime))
        {
            var obj = enemies[commonWave.enemyIdx];

            var spawnPos = InGameManager.Instance.curPlayer.transform.position + (Vector3)(Random.insideUnitCircle.normalized * 15f);
            var enemy = Instantiate(obj, spawnPos, Quaternion.identity);
            enemy.InitEnemy(InGameManager.Instance.curPlayer.transform, CommonEnemyRetireAction);
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

}

public class SpawnData
{
    public int idx;
    public int enemyIdx;
    
    public float spawnTime;
    public float spawnProba; // 확률

    public float increaseDelay;
    public float increaseProba;
    public float increaseMax;

    // from runtime
    public SpawnData(int idx, int enemyIdx, float spawnTime, float spawnProba, float increaseDelay, float increaseProba, float increaseMax)
    {
        this.idx = idx;
        this.enemyIdx = enemyIdx;
        this.spawnTime = spawnTime;
        this.spawnProba = spawnProba;
        this.increaseDelay = increaseDelay;
        this.increaseProba = increaseProba;
        this.increaseMax = increaseMax;
    }

    // from sheet
    public SpawnData(string[] args)
    {
        int i = 0;

        idx = int.Parse(args[i++]);
        enemyIdx = int.Parse(args[i++]);
        spawnTime = float.Parse(args[i++]);
        spawnProba = float.Parse(args[i++]);
        increaseDelay = float.Parse(args[i++]);
        increaseProba = float.Parse(args[i++]);
        increaseMax = float.Parse(args[i++]);
    }

    public bool GetSpawnProba(float gameTime)
    {
        if(spawnTime > gameTime)
        {
            return false;
        }

        float proba = spawnProba;
        float overTime = gameTime - spawnTime;
        float increase = Mathf.Floor(overTime / increaseDelay);

        proba += increase * increaseProba;
        proba = Mathf.Clamp(proba, spawnProba, spawnProba + increaseMax);
        proba *= Time.deltaTime;
        bool spawnAble = Random.Range(0f, 100f) < proba;

        return spawnAble;
    }
}