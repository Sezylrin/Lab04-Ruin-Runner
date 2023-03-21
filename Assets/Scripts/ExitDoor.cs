using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        if (!GameManager.Instance.CanExitLevel()) return;
        StartCoroutine(CompleteLevel());
    }

    IEnumerator CompleteLevel()
    {
        FindAnyObjectByType<PlayerManager>().enabled = false;
        _audioSource.Play();

        yield return new WaitForSeconds(_audioSource.clip.length);

        FindAnyObjectByType<PlayerManager>().ClearAction();
        Loader.Load(Scene.Victory);
    }
}