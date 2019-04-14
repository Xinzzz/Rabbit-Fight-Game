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
        if(Input.GetKeyDown(attackButton))
        {
            StartCombo();
        }
    }

    void StartCombo()
    {
        if(canPress)
        {
            buttomPress++;
        }

        if(buttomPress == 1)
        {
            anim.Play("Attack01");
            attacking = true;
        }
    }

    public void CheckCombo()
    {
        Debug.Log(buttomPress);
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
            buttomPress = 2;
            canPress = true;
        }
    }




}
