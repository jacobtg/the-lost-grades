using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    private bool _hasTriggered;

    [SerializeField] private bool mustHoldKeys;
    [SerializeField] private bool mustHoldGrades;
    
    [SerializeField] private Animator animator;
    [SerializeField] private string animationName;

    [SerializeField] private AudioClip soundToPlay;
    
    [SerializeField] List<GameObject> otherTriggers;

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
                    _hasTriggered = true;
                    animator.Play(animationName);

                    if (soundToPlay != null)
                    {
                        ScareManager.Instance.PlayScareSound(soundToPlay);
                    }

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
