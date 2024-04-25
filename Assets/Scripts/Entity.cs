using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public float hp;

    public abstract void OnDamage(Entity from, float damage);
}
