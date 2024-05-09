using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticDataManager : MonoBehaviour
{
    public static StaticDataManager Instance = null;

    public StaticWaveSheet waveData;
    public StaticEnemySheet enemyData;

    private void Awake()
    {
        Instance = this;

        var datas = new SheetDataBase[]
        {
            waveData,
            enemyData,
        };

        foreach (var item in datas)
        {
            item.LoadData();
        }
    }

    public static WaveData GetWaveData(int i)
    {
        return Instance.waveData.datas[i];
    }

    public static EnemyData GetEnemyData(int i)
    {
        return Instance.enemyData.datas[i];
    }
}
