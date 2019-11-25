using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LillyColoring : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private void OnTriggerEnter(Collider c) {
        if (c.CompareTag("Player")) {
            anim.SetBool("shouldLillyTurnRed", true);
        }
    }
}
