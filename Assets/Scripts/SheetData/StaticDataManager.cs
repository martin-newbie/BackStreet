using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticDataManager : MonoBehaviour
{
    public static StaticDataManager Instance = null;

    public StaticMonsterSheet waveData;

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator InitDatas()
    {
        var datas = new SheetDataBase[]
        {
            waveData,
        };

        foreach (var item in datas)
        {
            yield return StartCoroutine(item.LoadDataCor());
        }
    }

    public static Monsterdata GetMonsterData(int i)
    {
        return Instance.waveData.datas[i];
    }

}
