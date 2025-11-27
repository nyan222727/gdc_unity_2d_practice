using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Exist time
    float timer = 0;
    void Start()
    {
        
    }

    void Update()
    {
        // The interval of every update. Time.deltaTime
        timer += Time.deltaTime;
        if (timer > 2)
        {
            //Delete who
            Destroy(this.gameObject); // Only write this >> 元件 // Correct one >> this.gameObject
        }
    }
}
