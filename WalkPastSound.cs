using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkPastSound : MonoBehaviour
{
    private bool _hasTriggered;

    [SerializeField] private bool mustHoldKeys;
    [SerializeField] private bool mustHoldGrades;
    
    private AudioSource _audioSource;
    [SerializeField] private AudioClip sound;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bool shouldPlay = true;

            if (mustHoldKeys && !ObjectivesManager.Instance.holdingKey)
            {
                shouldPlay = false;
            }

            if (mustHoldGrades && !ObjectivesManager.Instance.holdingGrades)
            {
                shouldPlay = false;
            }
        
            if (!_hasTriggered && shouldPlay)
            {
                _audioSource.PlayOneShot(sound);        
                _hasTriggered = true;   
            }            
        }
    }
}
