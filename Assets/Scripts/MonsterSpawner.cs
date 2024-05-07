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
    SpawnData commonWave;
    SpawnData busterWave;

    void Start()
    {
        // 333... 1978
        commonWave = new SpawnData(0, 0, 0, 150f);
        busterWave = new SpawnData(1, 1, 15f, 100f);
    }

    void Update()
    {
        SpawnLogic(InGameManager.Instance.playTime, commonWave);
        SpawnLogic(InGameManager.Instance.playTime, busterWave);
    }

    void SpawnLogic(float gameTime, SpawnData spawnData)
    {
        if (spawnData.GetSpawnProba(gameTime))
        {
            var enemyData = GetEnemeyData(spawnData.enemyIdx);
            var obj = enemyPrefabs[enemyData.movementPattern];
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

    EnemyData GetEnemeyData(int enemyIdx)
    {
        switch (enemyIdx)
        {
            case 0:
                return new EnemyData(0, 1, 0.4f, 3f, 1, 3, 1);
            case 1:
                return new EnemyData(0, 2, 0.4f, 5f, 3, 2, 1);
            case 2:
                return new EnemyData(1, 0, 0.6f, 3f, 2, 15, 2);
            default:
                return null;
        }
    }
}

public class SpawnData
{
    public int idx;
    public int enemyIdx;
    
    public float spawnTime;
    public float spawnProba; // 확률

    // from runtime
    public SpawnData(int idx, int enemyIdx, float spawnTime, float spawnProba)
    {
        this.idx = idx;
        this.enemyIdx = enemyIdx;
        this.spawnTime = spawnTime;
        this.spawnProba = spawnProba;
    }

    // from sheet
    public SpawnData(string[] args)
    {
        int i = 0;

        idx = int.Parse(args[i++]);
        enemyIdx = int.Parse(args[i++]);
        spawnTime = float.Parse(args[i++]);
        spawnProba = float.Parse(args[i++]);
    }

    public bool GetSpawnProba(float gameTime)
    {
        if(spawnTime > gameTime)
        {
            return false;
        }

        float proba = spawnProba;
        proba *= Time.deltaTime;
        bool spawnAble = Random.Range(0f, 100f) < proba;

        return spawnAble;
    }
}

public class BossSpawnData
{
    public int idx;
    public int enemyIdx;
    public float spawnTime;
    public int maxHp;
    public int atkDamage;
    public int dropExp;
}

public class EnemyData
{
    public int monsterModel;
    public int movementPattern;
    public float colliderRadius;
    public float moveSpeed;
    public int atkDamage;
    public int maxHp;
    public int dropExp;

    public EnemyData(int monsterModel, int movementPattern, float colliderRadius, float moveSpeed, int atkDamage, int maxHp, int dropExp)
    {
        this.monsterModel = monsterModel;
        this.movementPattern = movementPattern;
        this.colliderRadius = colliderRadius;
        this.moveSpeed = moveSpeed;
        this.atkDamage = atkDamage;
        this.maxHp = maxHp;
        this.dropExp = dropExp;
    }
}