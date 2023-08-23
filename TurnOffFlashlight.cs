using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffFlashlight : MonoBehaviour
{
    private bool _hasTriggered;

    [SerializeField] private bool mustHoldKeys;
    [SerializeField] private bool mustHoldGrades;
    
    [SerializeField] List<GameObject> otherTriggers;
    [SerializeField] private AudioClip whisperSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!_hasTriggered)
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

                if (shouldPlay)
                {
                    Flashlight.Instance.TurnOffLight();
                    Flashlight.Instance.canTurnOn = false;
            
                    ScareManager.Instance.PlayScareSound(whisperSound);

                    foreach (var trigger in otherTriggers)
                    {
                        Destroy(trigger);
                    }
            
                    Destroy(gameObject);
                }
            }
        }
    }
}
