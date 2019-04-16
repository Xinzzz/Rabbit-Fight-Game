using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{
    public int hurtType;
    Animator anim;

    bool hurting = false;
    float hurtTime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!hurting)
        {
            anim.Play("IdleW");
        }
        else
        {
            hurtTime -= Time.deltaTime;
            if(hurtTime <= 0)
            {
                hurting = false;
                hurtTime = 0.3f;
            }
        }

    }

    public void TakeDamage(int hurtType)
    {

        Debug.Log("been hit");
        StartCoroutine(HurtAnimation(0.2f,hurtType));
    }

    IEnumerator HurtAnimation(float waitTime,int hurtType)
    {
        yield return new WaitForSeconds(waitTime);
        if (hurtType == 1)
        {
            hurting = true;
            anim.Play("HurtW02");
        }
        else if (hurtType == 2)
        {
            hurting = true;
            anim.Play("HurtW01");
        }
        else if (hurtType == 3)
        {
            hurting = true;
            anim.Play("HurtW03");
        }

    }
}
