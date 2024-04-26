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

    public void AttackTrigger(Entity from, int damage)
    {
        StartCoroutine(AttackRoutine(from, damage));
    }

    IEnumerator AttackRoutine(Entity from, int damage)
    {
        yield return null;
        bool flipY = transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270;
        sprite.flipY = flipY;

        animator.SetTrigger("slash");

        var result = new List<Collider2D>();
        if (atkCollider.OverlapCollider(filter, result) == 0) yield break;

        foreach (var item in result)
        {
            item.GetComponent<Entity>().OnDamage(from, damage);
        }
    }
}
