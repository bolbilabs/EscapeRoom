﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollisionReporter : MonoBehaviour
{
    void OnCollisionEnter(Collision c)
    {
    
            //we'll just use the first contact point for simplicity
            EventManager.TriggerEvent<BombBounceEvent, Vector3>(c.contacts[0].point);


        //              foreach (ContactPoint contact in c.contacts) {
        //
        //                      if (c.impulse.magnitude > 0.5f)
        //                              EventManager.TriggerEvent<AudioEventManager.BoxAudioEvent, Vector3> (contact.point);
        //                      
        //              }
    }
}
