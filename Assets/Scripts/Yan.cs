using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yan : MonoBehaviour
{

    public Slash slash;

    SpriteRenderer sprite;
    Animator animator;

    bool atkAble = true;
    float atkTimer = 2f;
    float damage = 1f;
    float moveSpeed = 5f;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (atkAble)
        {
            AttackLogic();
        }

        MovementLogic();
    }

    void MovementLogic()
    {
        float axisX = Input.GetAxisRaw("Horizontal");
        float axisY = Input.GetAxisRaw("Vertical");
        bool isMove = axisX != 0 || axisY != 0;
        animator.SetBool("move", isMove);
        transform.Translate(new Vector3(axisX, axisY, 0) * moveSpeed * Time.deltaTime);

        if (axisX != 0)
        {
            bool flipX = axisX > 0;
            sprite.flipX = flipX;
        }
    }

    void AttackLogic()
    {
        atkAble = false;
        slash.AttackTrigger(damage);
        StartCoroutine(AttackCoolDown());
    }

    IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(atkTimer);
        atkAble = true;
        yield break;
    }
}
