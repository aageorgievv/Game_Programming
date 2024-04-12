﻿using System;
using System.Collections.Generic;
using GXPEngine;

public enum EnemyState
{
    Attack,
    TakeDamage,
    Idle
}
internal abstract class Enemy : GameObject
{
    private List<Bullet> enemyBullets = new List<Bullet>();

    protected float health;

    public EnemyState enemyState;

    protected AnimationSprite currentAnimation;
    protected AnimationSprite attackAnimationSprite;
    protected AnimationSprite takeDamageAnimationSprite;
    protected AnimationSprite IdleAnimationSprite;

    private int counter;
    private int frame;

    private bool hasShot = false;
    private int bulletSpeed = -2;

    public Enemy(Vec2 position, float health) : base(true)
    {
        x = position.x;
        y = position.y;
        this.health = health;

        enemyState = EnemyState.Attack;
    }

    public virtual void Kill()
    {
        // trigger death animation
        // wait for it to end
        // then call LateDestroy();
        LateDestroy();
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0f)
        {
            Kill();
        }
    }

    public virtual void Move()
    {

    }
    public abstract int GetShootFrame();
    public abstract float GetAnimationSpeed();
    public abstract Sprite GetBulletSprite();

    public virtual void SpawnBullet(float x, float y)
    {
        if(frame == GetShootFrame())
        {
            if(!hasShot)
            {
                Bullet bullet = new Bullet(GetBulletSprite(), bulletSpeed, BulletFaction.Enemy);
                bullet.SetXY(x, y);

                bullet.OnDestroyed += OnBulletDestroyed;

                game.AddChild(bullet);
                enemyBullets.Add(bullet);
                hasShot = true;
            }
        } else
        {
            hasShot = false;
        }
    }

    public virtual void Animation()
    {
        AnimationSprite prevAnimation = currentAnimation;
        switch(enemyState)
        {
            case EnemyState.Attack:
                currentAnimation = attackAnimationSprite;
                break;
            case EnemyState.TakeDamage:
                currentAnimation = takeDamageAnimationSprite;
                break;
            case EnemyState.Idle:
                currentAnimation = IdleAnimationSprite;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if(currentAnimation != prevAnimation)
        {
            if(prevAnimation != null)
            {
                prevAnimation.visible = false;
            }

            currentAnimation.visible = true;
        }

        if(counter >= GetAnimationSpeed())
        {
            counter = 0;
            if(frame >= currentAnimation.frameCount)
            {
                frame = 0;
            }
            currentAnimation.SetFrame(frame);
            frame++;
        }
        counter++;
    }

    private void OnBulletDestroyed(Bullet bullet)
    {
        bullet.OnDestroyed -= OnBulletDestroyed;
        enemyBullets.Remove(bullet);
    }
}
