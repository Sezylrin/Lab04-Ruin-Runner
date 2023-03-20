using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequiredKeys : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer[] Keys;
    void Start()
    {
        if (!GameManager.Instance)
        {
            Debug.Log("colour changed");
            return;
        }
        foreach (SpriteRenderer Key in Keys)
        {
            Key.color = Color.gray;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance || GameManager.Instance.keysCollected == 0)
            return;
        Keys[GameManager.Instance.keysCollected - 1].color = Color.white;
    }
}
