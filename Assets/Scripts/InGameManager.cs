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

    public Transform camTr;
    public Player curPlayer;
    public Enemy[] enemyPrefabs;
    List<Enemy> curEnemy = new List<Enemy>();
    public Transform crossHair;
    public ExpItem expItemPrefab;

    int maximumCount = 100;
    float spawnTime = 1f;

    [Header("Interface")]
    public ExpGaugeBox expGauge;
    public HpGaugeBox hpGauge;

    void Start()
    {
        StartCoroutine(EnemySpawnLogic());
    }

    IEnumerator EnemySpawnLogic()
    {
        while (true)
        {
            var spawnPos = curPlayer.transform.position + (Vector3)(Random.insideUnitCircle.normalized * 15f);
            var enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPos, Quaternion.identity);
            enemy.InitEnemy(curPlayer.transform, CommonEnemyRetireAction);
            curEnemy.Add(enemy);
            yield return new WaitForSeconds(spawnTime);
            yield return new WaitUntil(() => curEnemy.Count < maximumCount);
        }
    }

    void CommonEnemyRetireAction(Enemy enemy)
    {
        var pos = enemy.transform.position;
        SpawnExpItem(pos);
        curEnemy.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    void Update()
    {
        CrossHairFollow();
        CameraFollow();
    }

    void CrossHairFollow()
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        crossHair.position = pos;
    }

    void CameraFollow()
    {
        var finalPos = curPlayer.transform.position;
        finalPos.z = -10;
        camTr.position = Vector3.Lerp(camTr.position, finalPos, Time.deltaTime * 10f);
    }

    public Transform FindNearestTarget(Vector3 pos)
    {
        return curEnemy.OrderBy((item) => Vector3.Distance(item.transform.position, pos)).ElementAt(0).transform;
    }

    public ExpItem SpawnExpItem(Vector3 pos)
    {
        var obj = Instantiate(expItemPrefab, pos, Quaternion.identity);
        return obj;
    }

    public void IncreaseExp(int exp)
    {
        curPlayer.IncreaseExp(exp);
    }
}
