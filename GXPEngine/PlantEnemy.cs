﻿using System.Collections.Generic;
using System.Drawing;
using GXPEngine;
using GXPEngine.Core;
internal class PlantEnemy : Enemy
{
    private List<Bullet> enemyBullets = new List<Bullet>();

    private int shootFrame = 5;
    private int animationSpeed = 8;
    private int offSetY = 11;
    private int offSetX = -8;

    private int width = 16;
    private int height = 21;

    public PlantEnemy(Vec2 position, float health) : base(position, health)
    {
        attackAnimationSprite = new AnimationSprite("assets/PlantAttack.png", 8, 1);
        attackAnimationSprite.visible = false;
        attackAnimationSprite.collider.isTrigger = true;
        AddChild(attackAnimationSprite);


        takeDamageAnimationSprite = new AnimationSprite("assets/PlantHit.png", 5, 1);
        takeDamageAnimationSprite.visible = false;
        takeDamageAnimationSprite.collider.isTrigger = true;
        AddChild(takeDamageAnimationSprite);

        IdleAnimationSprite = new AnimationSprite("assets/PlantIdle.png", 11, 1);
        IdleAnimationSprite.visible = false;
        IdleAnimationSprite.collider.isTrigger = true;
        AddChild(IdleAnimationSprite);
    }

    public void Update()
    {
        Move();
        Animation();
        SpawnBullet(x + offSetX, y + offSetY);
    }

    public override void Kill()
    {
        LateDestroy();
    }

    public override int GetShootFrame()
    {
        return shootFrame;
    }

    public override float GetAnimationSpeed()
    {
        return animationSpeed;
    }

    public override Sprite GetBulletSprite()
    {
        return new Sprite("assets/PlantBullet.png", true, false);
    }

    protected override Collider createCollider()
    {
        Bitmap bitmap = new Bitmap(width, height);
        Sprite colliderSprite = new Sprite(bitmap, false);
        AddChild(colliderSprite);
        colliderSprite.SetXY(width / 2f, height / 2f);
        BoxCollider boxCollider = new BoxCollider(colliderSprite);
        return boxCollider;
    }
}
