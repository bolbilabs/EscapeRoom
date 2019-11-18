using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingPillarCollision : MonoBehaviour
{
    [SerializeField] Animator anim;

    // void Update() {
    //     Debug.Log("hello!");
    // }

    public void onTriggerEnter(Collider collider) {
        Debug.Log("trigger");
        if (collider.CompareTag("Player")) {
            anim.SetBool("shouldPillarSlide", true);
        }
    }
}
