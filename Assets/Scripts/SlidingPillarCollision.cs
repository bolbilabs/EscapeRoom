using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingPillarCollision : MonoBehaviour
{
    [SerializeField] Animator anim;

    public void onTriggerEnter(Collider collider) {
        Debug.Log("trigger");
        if (collider.CompareTag("Player")) {
            anim.SetBool("shouldPillarSlide", true);
        }
    }
}
