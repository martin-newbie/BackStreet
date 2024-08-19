using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StaticWaveSheet", menuName = "SheetData/StaticWaveSheet", order = int.MinValue)]
public class StaticMonsterSheet : SheetDataBase
{
    protected override string gid => "1779530673";

    protected override string range => "C3:M10";

    public List<Monsterdata> datas;

    protected override void SetData(string data)
    {
        datas = new List<Monsterdata>();
        var splitData = data.Split('\n');
        foreach (var item in splitData)
        {
            datas.Add(new Monsterdata(item.Split('\t')));
        }
    }
}

[System.Serializable]
public class Monsterdata
{
    public int index;
    public int stage;
    public bool isBoss;
    public float moveSpeed;
    public float damage;
    public float maxHp;
    public float dropExp;
    public float spawnProb;
    public float increaseProb;
    public float maxProb;
    public float spawnTime;

    public Monsterdata(string[] args)
    {
        int i = 0;

        index = int.Parse(args[i++]);
        stage = int.Parse(args[i++]);
        isBoss = int.Parse(args[i++]) == 1;
        moveSpeed = float.Parse(args[i++]);
        damage = float.Parse(args[i++]);
        maxHp = float.Parse(args[i++]);
        dropExp = float.Parse(args[i++]);
        spawnProb = float.Parse(args[i++]);
        increaseProb = float.Parse(args[i++]);
        maxProb = float.Parse(args[i++]);
    }

    public bool GetSpawnProb(float gameTime)
    {
        if (spawnTime > gameTime) return false;

        float proba = spawnProb + (increaseProb * (int)(gameTime / 60f));
        proba *= Time.deltaTime;
        bool spawnAble = Random.Range(0f, 100f) < proba;

        return spawnAble;
    }

}
