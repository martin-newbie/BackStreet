using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public Material defaultMat;
    public Material damagedMat;

    [HideInInspector] public float moveSpeed;
    [HideInInspector] public int damage;
    [HideInInspector] public bool isInit = false;
    [HideInInspector] public bool isAlive;

    [HideInInspector] public Transform target;
    [HideInInspector] public SpriteRenderer sprite;
    [HideInInspector] public Action<Enemy> retireAction;
    [HideInInspector] public Animator animator;
    [HideInInspector] public CircleCollider2D collider;
    [HideInInspector] public EnemyData enemyData;
    IMovementPattern movementPattern;

    public virtual void InitEnemy(EnemyData _enemyData, Transform _target, Action<Enemy> retire)
    {
        target = _target;
        
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        collider = GetComponent<CircleCollider2D>();

        isInit = true;
        isAlive = true;
        retireAction = retire;

        enemyData = _enemyData;
        collider.radius = enemyData.colliderRadius;
        moveSpeed = enemyData.moveSpeed;
        damage = enemyData.atkDamage;
        hp = enemyData.maxHp;

        movementPattern = MonsterSpawner.Instance.GetMovementPattern(enemyData.movementPattern);
        movementPattern.Init(this);
    }

    private void Update()
    {
        if (!isInit) return;
        if (!isAlive) return;

        MoveLogic();
    }

    protected virtual void MoveLogic()
    {
        movementPattern?.Movement(target, transform, sprite, moveSpeed);
        return;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isAlive) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            ContactStayWithPlayer(collision.gameObject);
        }
    }

    protected virtual void ContactStayWithPlayer(GameObject obj)
    {
        // TODO : Replace yan to player class when it's ready
        var player = obj.GetComponent<Player>();
        player.OnDamage(this, damage);
    }

    public override void OnDamage(Entity from, int damage)
    {
        if (!isAlive) return;

        hp -= damage;
        int dir = transform.position.x > from.transform.position.x ? 1 : -1;
        DamageTextManager.Instance.PrintText(transform.position, damage, dir);

        if (hp <= 0)
        {
            isAlive = false;
            retireAction?.Invoke(this);
        }
        else
        {
            StartCoroutine(DamagedMatChange(0.1f));
        }
    }

    IEnumerator DamagedMatChange(float dur)
    {
        sprite.material = damagedMat;
        yield return new WaitForSeconds(dur);
        sprite.material = defaultMat;
    }
}

public interface IMovementPattern
{
    void Init(Enemy subject);
    void Movement(Transform target, Transform transform, SpriteRenderer sprite, float moveSpeed);
}