using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator _doorAnim;
    private AudioSource _audioSource;
    
    private void Awake()
    {
        _doorAnim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }
    
    public void OpenDoor()
    {
        if (gameObject.CompareTag("Reversed Door"))
        {
            _doorAnim.Play("DoorOpenReversed");
        }
        else
        {
            _doorAnim.Play("DoorOpen");
        }
    }
}
