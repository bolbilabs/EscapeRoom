using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkingLillypads : MonoBehaviour
{
    bool shouldLillypadsSink;

    // Start is called before the first frame update
    void Start()
    {
        shouldLillypadsSink = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldLillypadsSink) {
            transform.Translate(Vector3.down * Time.deltaTime, Space.Self);
            if (transform.position.y <= -20) {
                // trigger reset
                shouldLillypadsSink = false;
            }
        }
    }
}
