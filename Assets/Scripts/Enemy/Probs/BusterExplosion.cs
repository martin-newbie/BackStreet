using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusterExplosion : MonoBehaviour
{

    public Collider2D hitCollider;
    public ContactFilter2D filter;

    public void SetExplosion(Entity from, int damage)
    {
        var result = new List<Collider2D>();
        hitCollider.OverlapCollider(filter, result);

        foreach (var item in result)
        {
            item.GetComponent<Entity>().OnDamage(from, damage);
        }
    }

    void DestroyFrame()
    {
        Destroy(gameObject);
    }
}
