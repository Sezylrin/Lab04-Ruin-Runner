using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private AudioSource _audioSource;
    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public IEnumerator BreakShield()
    {
        _spriteRenderer.enabled = true;
        _audioSource.Play();

        yield return new WaitForSeconds(_audioSource.clip.length);

        _spriteRenderer.enabled = true;
        gameObject.SetActive(false);
    }
}