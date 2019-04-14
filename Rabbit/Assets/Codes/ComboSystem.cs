using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboSystem : MonoBehaviour
{
    [Header("Input")]
    public KeyCode attack;

    [Header("Attack")]
    Attack att01;
    Attack att02;
    Attack att03;

    Animator anim;

    Player rabbit;

    public bool attacking;

    int attackCount = 0;
    float attackTimer;
    float durationTime = 0.1f;

    public float comboLeeway = 0.5f; //gives player the time to press the next button
    bool continueCombo = false;

    bool canPress = true;


    private void Start()
    {
        anim = GetComponent<Animator>();
        rabbit = GetComponent<Player>();
        att01 = new Attack(0.3f, "Attack01");
        att02 = new Attack(0.4f, "Attack02");
        att03 = new Attack(0.4f, "Attack03");
    }

    private void Update()
    {
        //Debug.Log(attackTimer);
        //first attack
        if(Input.GetKeyDown(attack))
        {
            if(canPress)
            {
                attackCount++;
            }

            Debug.Log(attackCount);


            if (attackCount == 1)
            {
                Debug.Log("first");
                anim.Play(att01.animName);
                attacking = true;
                attackTimer = att01.length + durationTime;
                comboLeeway = 0.5f;
            }

            //second attack
            if(attackCount == 2 && continueCombo)
            {
                Debug.Log("second");
                attacking = true;
                attackTimer = att02.length + durationTime;
                anim.Play(att02.animName);
                comboLeeway = 0.5f;
                //StartCoroutine(nextCombo(0.10f, att02));
            }
            //third attack
            if(attackCount == 3 && continueCombo)
            {
                Debug.Log("third");
                attacking = true;
                attackTimer = att03.length + durationTime;
                anim.Play(att03.animName);
                comboLeeway = 0.5f;
                //StartCoroutine(nextCombo(0.35f, att03));               
            }
        }       
        if(attacking)
        {
            attackTimer -= Time.deltaTime;

            //for player have the time to press attack and go to next combo.
            comboLeeway -= Time.deltaTime;
            if(comboLeeway <= 0)
            {
                continueCombo = false;
            }
            else
            {
                continueCombo = true;
            }

            //if nothing happened, reset combo states to init.
            if (attackTimer <= 0)
            {

                attacking = false;
                comboLeeway = 0.5f;
                attackCount = 0;               
            }
        }             
        
    }

    //wait for animation playing is finished
  /* IEnumerator nextCombo(float waitTime,Attack att)
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log(att.animName);
        anim.Play(att.animName);
        attackTimer = att.length + durationTime;
        comboLeeway = 0.5f;
        attacking = true;
       
    }*/
}

public class Attack
{
    public string animName;
    public float length;

    public Attack(float attackDuration, string animationName)
    {
        animName = animationName;
        length = attackDuration;
    }
}