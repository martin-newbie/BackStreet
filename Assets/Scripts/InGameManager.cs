using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InGameManager : MonoBehaviour
{

    public static InGameManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public Transform camTr;
    public Player curPlayer;
    public Enemy enemyPrefab;
    public SweeperBuster buster;
    List<Enemy> curEnemy = new List<Enemy>();
    public Transform crossHair;
    public ExpItem expItemPrefab;

    int maximumCount = 100;
    float spawnTime = 1f;

    [Header("Interface")]
    public ExpGaugeBox expGauge;
    public HpGaugeBox hpGauge;
    public Text playTimeText;

    float playTime;
    TimeSpan prevTime;

    void Start()
    {
        StartCoroutine(EnemySpawnLogic());
        StartCoroutine(BusterSpawnLogic());
    }

    IEnumerator EnemySpawnLogic()
    {
        while (true)
        {
            var spawnPos = curPlayer.transform.position + (Vector3)(Random.insideUnitCircle.normalized * 15f);
            var enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            enemy.InitEnemy(curPlayer.transform, CommonEnemyRetireAction);
            curEnemy.Add(enemy);
            yield return new WaitForSeconds(spawnTime);
            yield return new WaitUntil(() => curEnemy.Count < maximumCount);
        }
    }

    IEnumerator BusterSpawnLogic()
    {
        while (true)
        {
            var spawnPos = curPlayer.transform.position + (Vector3)(Random.insideUnitCircle.normalized * 15f);
            var enemy = Instantiate(buster, spawnPos, Quaternion.identity);
            enemy.InitEnemy(curPlayer.transform, CommonEnemyRetireAction);
            curEnemy.Add(enemy);
            yield return new WaitForSeconds(2f);
            yield return new WaitUntil(() => curEnemy.Count < maximumCount);
        }
    }

    void CommonEnemyRetireAction(Enemy subject)
    {
        var pos = subject.transform.position;
        SpawnExpItem(pos);
        curEnemy.Remove(subject);
        Destroy(subject.gameObject);
    }

    void Update()
    {
        CrossHairFollow();
        CameraFollow();
        UpdatePlayTime();
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

    void UpdatePlayTime()
    {
        playTime += Time.deltaTime;
        TimeSpan time = TimeSpan.FromSeconds(playTime);
        if (prevTime.Seconds != time.Seconds)
        {
            playTimeText.text = time.ToString(@"mm\:ss");
        }
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

    public int GetDrawOrder(int yPos)
    {
        return (int)curPlayer.transform.position.y - yPos;
    }
}
