using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [HideInInspector] public int x, y;

    public void InitMap(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
}
