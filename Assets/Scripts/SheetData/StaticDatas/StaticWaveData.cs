using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StaticWaveData", menuName = "SheetData/StaticWaveData", order = int.MinValue)]
public class StaticWaveData : SheetDataBase
{
    protected override string gid => "1779530673";

    protected override string range => "C3:H15";

    public List<SpawnData> datas;

    protected override void SetData(string data)
    {
        datas = new List<SpawnData>();
        var splitData = data.Split('\n');
        foreach (var item in splitData)
        {
            datas.Add(new SpawnData(item.Split('\t')));
        }
    }
}

[System.Serializable]
public class SpawnData
{
    public int idx;
    public int enemyIdx;

    public float spawnTime;
    public float endTime;
    public float spawnProba; // 확률
    public bool isBoss;

    // from runtime
    public SpawnData(int idx, int enemyIdx, float spawnTime, float endTime, float spawnProba, int isBoss)
    {
        this.idx = idx;
        this.enemyIdx = enemyIdx;
        this.spawnTime = spawnTime;
        this.endTime = endTime;
        this.spawnProba = spawnProba;
        this.isBoss = isBoss == 1;
    }

    // from sheet
    public SpawnData(string[] args)
    {
        int i = 0;

        idx = int.Parse(args[i++]);
        enemyIdx = int.Parse(args[i++]);
        spawnTime = float.Parse(args[i++]);
        spawnProba = float.Parse(args[i++]);
        isBoss = int.Parse(args[i++]) == 1;
    }

    public bool GetSpawnProba(float gameTime)
    {
        if (spawnTime > gameTime || gameTime > endTime)
        {
            return false;
        }

        float proba = spawnProba;
        proba *= Time.deltaTime;
        bool spawnAble = Random.Range(0f, 100f) < proba;

        return spawnAble;
    }
}
