using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{

    private BoxCollider2D exitTrigger;
    // Start is called before the first frame update
    void Start()
    {
        exitTrigger = this.gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //if (keys == maxKeys)
            //{
            //Loader.Load(Scene.Victory);
            //}
        }
    }
}
