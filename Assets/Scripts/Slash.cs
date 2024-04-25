using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public Animator animator;
    public Collider2D atkCollider;
    public ContactFilter2D filter;

    public void AttackTrigger(float damage)
    {
        animator.SetTrigger("slash");
        var colliderObjects = new List<Collider2D>();
        Physics2D.OverlapCollider(atkCollider, filter, colliderObjects);
        if (colliderObjects.Count == 0) return;

        foreach (var item in colliderObjects)
        {
            // item.GetComponent<Enemy>().OnDamage(damage);
        }
    }
}
