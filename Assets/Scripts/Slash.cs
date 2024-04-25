using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public Animator animator;
    public Collider2D atkCollider;
    public ContactFilter2D filter;
    SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void AttackTrigger(Entity from, float damage)
    {
        bool flipY = transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270;
        sprite.flipY = flipY;

        animator.SetTrigger("slash");
        var colliderObjects = new List<Collider2D>();
        Physics2D.OverlapCollider(atkCollider, filter, colliderObjects);
        if (colliderObjects.Count == 0) return;

        foreach (var item in colliderObjects)
        {
            item.GetComponent<Enemy>().OnDamage(from, damage);
        }
    }
}
