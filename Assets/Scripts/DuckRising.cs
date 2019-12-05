using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckRising : MonoBehaviour
{
    public Animator anim;

    private void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Player"))
        {
            anim.SetBool("shouldRise", true);
        }
    }
}
