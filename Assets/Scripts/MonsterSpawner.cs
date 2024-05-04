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

    public Enemy[] enemies;

    List<Enemy> curEnemy = new List<Enemy>();
    SpawnData commonWave;
    SpawnData busterWave;

    void Start()
    {
        // 333... 1978
        commonWave = new SpawnData(0, 0, 0, 80f, 20f, 5f, 80f);
        busterWave = new SpawnData(1, 1, 60f, 50f, 30f, 5f, 70f);
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
            var obj = enemies[spawnData.enemyIdx];

            var spawnPos = InGameManager.Instance.curPlayer.transform.position + (Vector3)(Random.insideUnitCircle.normalized * 15f);
            var enemy = Instantiate(obj, spawnPos, Quaternion.identity);
            enemy.InitEnemy(GetEnemeyData(spawnData.enemyIdx), InGameManager.Instance.curPlayer.transform, CommonEnemyRetireAction);
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
                return new EnemyData(0, 0, 0.4f, 5f, 3, 2, 1);
            case 2:
                return new EnemyData(1, 0, 0.6f, 3f, 2, 15, 2);
            default:
                return null;
        }
    }

    public IMovementPattern GetMovementPattern(int idx)
    {
        switch (idx)
        {
            case 0:
                return new CommonPattern();
            case 1:
                return new Sweeper();
            case 2:
                return new SweeperBuster();
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