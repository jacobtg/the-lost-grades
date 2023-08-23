using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeLeaveScare : MonoBehaviour
{
    private bool hasBeenTriggered;

    [SerializeField] private Transform playerScarePos;
    [SerializeField] private Transform playerLookAt;
    [SerializeField] private GameObject scareObj;
    [SerializeField] private AudioClip scareSound;
    [SerializeField] private Light clownLight;
    [SerializeField] private AudioClip doorShutSound;
    
    private void OnTriggerEnter(Collider other)
    {
        if (ObjectivesManager.Instance.holdingKey && !hasBeenTriggered)
        {
            if (doorShutSound)
                ScareManager.Instance.PlayScareSound(doorShutSound);
            hasBeenTriggered = true;
            StartCoroutine(ScareManager.Instance.JumpScare(playerScarePos.position, playerLookAt.position, scareObj, scareSound, clownLight, false));
        }
    }
}
