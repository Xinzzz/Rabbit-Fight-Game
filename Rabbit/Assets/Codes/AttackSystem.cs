using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    Animator anim;

    public KeyCode attackButton;

    int buttomPress;
    bool canPress;
    public bool attacking;

    //actual attack
    public Transform attackPos;
    public LayerMask whatIsEnemy;
    public float attackRange;

    public GameObject hitEffect;
    public Transform hitPos;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        buttomPress = 0;
        canPress = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(attackButton))
        {
            StartCombo();
        }
    }

    void StartCombo()
    {
        if(canPress)
        {
            buttomPress++;
            if (buttomPress >= 1 && (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") ||
                anim.GetCurrentAnimatorStateInfo(0).IsName("Run")))
            {
                anim.Play("Attack01");
                Damage(1);
                attacking = true;
            }
        }
        
    }

    public void CheckCombo()
    {
               
        canPress = false;
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack01") && buttomPress == 1)
        {
            attacking = false;         
            buttomPress = 0;
            canPress = true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack01") && buttomPress >= 2)
        {
            anim.Play("Attack02");
            Damage(2);
            canPress = true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack02") && buttomPress == 2)
        {
            attacking = false;
            buttomPress = 0;
            canPress = true;
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack02") && buttomPress >= 3)
        {
            anim.Play("Attack03");
            Damage(3);
            canPress = true;
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack03") && buttomPress == 3)
        {
            attacking = false;
            buttomPress = 0;
            canPress = true;
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack03") && buttomPress >= 4)
        {
            anim.Play("Attack02");
            Damage(2);
            buttomPress = 2;
            canPress = true;
        }
    }

    void Damage(int attackType)
    {
        Collider2D[] enmiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemy);
        for (int i = 0; i < enmiesToDamage.Length; i++)
        {
            Instantiate(hitEffect, hitPos.position, Quaternion.identity);
            enmiesToDamage[i].GetComponent<Enemy>().StartCoroutine(enmiesToDamage[i].GetComponent<Enemy>().TakeDamage(attackType));
            if (enmiesToDamage[i].GetComponent<Enemy>().transform.position.x < transform.position.x)
            {
                enmiesToDamage[i].GetComponent<Enemy>().hitFromRight = true;
                if (!enmiesToDamage[i].GetComponent<Enemy>().facingRight)
                {
                    enmiesToDamage[i].GetComponent<Enemy>().Flip();

                }
            }
            else
            {
                enmiesToDamage[i].GetComponent<Enemy>().hitFromRight = false;
                if (enmiesToDamage[i].GetComponent<Enemy>().facingRight)
                {
                    enmiesToDamage[i].GetComponent<Enemy>().Flip();

                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

}
