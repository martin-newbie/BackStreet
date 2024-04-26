using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public int hp;

    public abstract void OnDamage(Entity from, int damage);
}
