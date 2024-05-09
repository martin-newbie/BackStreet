using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StaticEnemySheet", menuName = "SheetData/StaticEnemySheet", order = int.MinValue)]
public class StaticEnemySheet : SheetDataBase
{
    protected override string gid => "142739972";

    protected override string range => "C3:I13";

    public List<EnemyData> datas;

    protected override void SetData(string data)
    {
        datas = new List<EnemyData>();

        var splitData = data.Split('\n');
        foreach (var item in splitData)
        {
            datas.Add(new EnemyData(item.Split('\t')));
        }
    }
}

[System.Serializable]
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

    public EnemyData(string[] args)
    {
        int idx = 0;

        monsterModel = int.Parse(args[idx++]);
        movementPattern = int.Parse(args[idx++]);
        colliderRadius = float.Parse(args[idx++]);
        moveSpeed = float.Parse(args[idx++]);
        atkDamage = int.Parse(args[idx++]);
        maxHp = int.Parse(args[idx++]);
        dropExp = int.Parse(args[idx++]);
    }
}