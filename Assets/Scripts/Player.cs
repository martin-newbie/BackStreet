using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    public Slash slash;

    SpriteRenderer sprite;
    Animator animator;

    bool atkAble = true;
    bool damagedAble = true;
    float atkTimer = 2f;
    float damage = 1f;
    float moveSpeed = 5f;
    float damageCoolTime = 0.5f;

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

    protected void MovementLogic()
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

    protected void AttackLogic()
    {
        atkAble = false;
        SetAtkObjectRot();
        slash.AttackTrigger(this, damage);
        StartCoroutine(AttackCoolDown());
    }

    IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(atkTimer);
        atkAble = true;
        yield break;
    }

    void SetAtkObjectRot()
    {
        var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var dir = transform.position - target;
        var rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        slash.transform.rotation = Quaternion.Euler(0, 0, rot);
    }

    public override void OnDamage(Entity from, float damage)
    {
        if (!damagedAble) return;

        hp -= damage;

        int dir = transform.position.x > from.transform.position.x ? 1 : -1;
        DamageTextManager.Instance.PrintText(transform.position, (int)damage, dir);

        if(hp <= 0)
        {

        }

        damagedAble = false;
        StartCoroutine(DamageCool());
    }

    IEnumerator DamageCool()
    {
        yield return new WaitForSeconds(damageCoolTime);
        damagedAble = true;
    }
}
