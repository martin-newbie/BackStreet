using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{

    public static ResourceManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public RuntimeAnimatorController[] commonEnemy;
    public RuntimeAnimatorController[] sweeperVariation;

    public RuntimeAnimatorController GetEnemyAnim(int idx)
    {
        return commonEnemy[idx];
    }

    public RuntimeAnimatorController GetSweeperAnim()
    {
        return sweeperVariation[Random.Range(0, sweeperVariation.Length)];
    }
}
