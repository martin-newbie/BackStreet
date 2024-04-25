using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InGameManager : MonoBehaviour
{

    public static InGameManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public Transform playerPos;
    public Enemy enemyPrefab;
    List<Enemy> curEnemy = new List<Enemy>();

    int maximumCount = 100;
    float spawnTime = 1f;

    void Start()
    {
        StartCoroutine(EnemySpawnLogic());
    }

    IEnumerator EnemySpawnLogic()
    {
        while (true)
        {
            var spawnPos = playerPos.position + (Vector3)(Random.insideUnitCircle.normalized * 15f);
            var enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            enemy.InitEnemy(playerPos);
            curEnemy.Add(enemy);
            yield return new WaitForSeconds(spawnTime);
            yield return new WaitUntil(() => curEnemy.Count < maximumCount);
        }
    }

    public Transform FindNearestTarget(Vector3 pos)
    {
        return curEnemy.OrderBy((item) => Vector3.Distance(item.transform.position, pos)).ElementAt(0).transform;
    }
}
