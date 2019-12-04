using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStartPop : MonoBehaviour
{
    public Animator playerAnim;
    public Animator controllerAnim;
    // Start is called before the first frame update
    void Start()
    {
        playerAnim.SetBool("isCamera", true);
        controllerAnim.SetBool("isCamera", true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
