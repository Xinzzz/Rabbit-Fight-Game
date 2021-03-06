﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public Animator anim;

    [SerializeField]
    protected float moveSpeed;
    protected float gravity = 10f;

    [SerializeField]
    protected int health;

    public bool isDead;
    

    public bool facingRight;
    public bool attacking;
    public bool hurting;

    public virtual void Start()
    {
        facingRight = false;
        anim = GetComponent<Animator>();
    }

    public void Flip()
    {
        facingRight = !facingRight;
        Vector3 charScale = transform.localScale;
        charScale.x *= -1;
        transform.localScale = charScale;
    }

    public abstract IEnumerator TakeDamage(int hurtType);

    public void DeadOrNot()
    {
        if (health <= 0)
        {
            isDead = true;
        }
        else
        {
            isDead = false;
        }
    }
}
