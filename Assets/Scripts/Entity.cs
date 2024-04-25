using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    float hp;

    public abstract void OnDamage(float damage);
}
