using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StaticWaveSheet", menuName = "SheetData/StaticWaveSheet", order = int.MinValue)]
public class StaticWaveSheet : SheetDataBase
{
    protected override string gid => "1779530673";

    protected override string range => "C3:H15";

    public List<WaveData> datas;

    protected override void SetData(string data)
    {
        datas = new List<WaveData>();
        var splitData = data.Split('\n');
        foreach (var item in splitData)
        {
            datas.Add(new WaveData(item.Split('\t')));
        }
    }
}

[System.Serializable]
public class WaveData
{
    public int idx;
    public int enemyIdx;

    public float spawnTime;
    public float endTime;
    public float spawnProba; // 확률
    public bool isBoss;

    // from runtime
    public WaveData(int idx, int enemyIdx, float spawnTime, float endTime, float spawnProba, int isBoss)
    {
        this.idx = idx;
        this.enemyIdx = enemyIdx;
        this.spawnTime = spawnTime;
        this.endTime = endTime;
        this.spawnProba = spawnProba;
        this.isBoss = isBoss == 1;
    }

    // from sheet
    public WaveData(string[] args)
    {
        int i = 0;

        idx = int.Parse(args[i++]);
        enemyIdx = int.Parse(args[i++]);
        spawnTime = float.Parse(args[i++]);
        endTime = float.Parse(args[i++]);
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
