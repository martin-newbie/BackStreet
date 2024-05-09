using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public Material defaultMat;
    public Material damagedMat;

    protected float moveSpeed;
    protected int damage;
    protected bool isInit = false;
    protected bool isAlive;
    protected Transform target;
    protected SpriteRenderer sprite;
    protected Action<Enemy> retireAction;
    protected Animator animator;
    protected CircleCollider2D hitCollider;
    protected EnemyData enemyData;

    public virtual void InitEnemy(EnemyData _enemyData, Transform _target, Action<Enemy> retire)
    {
        target = _target;

        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        hitCollider = GetComponent<CircleCollider2D>();

        isInit = true;
        isAlive = true;
        retireAction = retire;

        enemyData = _enemyData;
        hitCollider.radius = enemyData.colliderRadius;
        moveSpeed = enemyData.moveSpeed;
        damage = enemyData.atkDamage;
        hp = enemyData.maxHp;

        if (enemyData.animeIndex >= 0)
            animator.runtimeAnimatorController = ResourceManager.Instance.GetEnemyAnim(enemyData.animeIndex);
    }

    private void Update()
    {
        if (!isInit) return;
        if (!isAlive) return;

        MoveLogic();
    }

    protected virtual void MoveLogic()
    {
        var dist = Vector3.Distance(target.position, transform.position);
        if (dist < 0.5f)
        {
            return;
        }

        var dir = (target.position - transform.position).normalized;
        transform.position += dir * Time.deltaTime * moveSpeed;

        sprite.sortingOrder = InGameManager.Instance.GetDrawOrder((int)transform.position.y);
        SetDir(dir.x);
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