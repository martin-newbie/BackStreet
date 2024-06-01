using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public Slash slash;
    public FieldHpBar hpBar;

    SpriteRenderer sprite;
    Animator animator;

    bool atkAble = true;
    bool damagedAble = true;
    float atkTimer = 2f;
    float moveSpeed = 5f;
    float damageCoolTime = 0.2f;
    int damage = 1;
    int curExp;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        hp = 100;
        InGameManager.Instance.expGauge.SetGauge(0, GetMaxExp());
        InGameManager.Instance.hpGauge.SetGauge(hp, 100);
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

    public override void OnDamage(Entity from, int damage)
    {
        if (!damagedAble) return;

        hpBar.StartHpMove(100, hp, hp - damage);
        hp -= damage;
        InGameManager.Instance.hpGauge.SetGauge(hp, 100);

        int dir = transform.position.x > from.transform.position.x ? 1 : -1;
        DamageTextManager.Instance.PrintText(transform.position, damage, dir);

        if (hp <= 0)
        {
            // gameover
            InGameManager.Instance.GameOver();
        }

        damagedAble = false;
        StartCoroutine(DamageCool());
    }

    public void GetHP(int value)
    {
        hp += value;
        if (hp >= 100/*maxHp*/)
        {
            hpBar.gameObject.SetActive(false);
        }
    }

    public void IncreaseExp(int exp)
    {
        curExp += exp;
        if (curExp >= GetMaxExp())
        {
            curExp -= GetMaxExp();
            // exp level up
        }

        InGameManager.Instance.expGauge.SetGauge(curExp, GetMaxExp());
    }

    IEnumerator DamageCool()
    {
        sprite.color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(damageCoolTime);
        damagedAble = true;
        sprite.color = new Color(1, 1, 1, 1f);
    }

    int GetMaxExp()
    {
        return 100;
    }
}
