using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanTurnOnFlashlight : MonoBehaviour
{
    private bool _hasTriggered;

    [SerializeField] private bool mustHoldKeys;
    [SerializeField] private bool mustHoldGrades;
    
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
                    Flashlight.Instance.canTurnOn = true;
                    Flashlight.Instance.TurnOnLight(false);
                    Destroy(gameObject);
                }
            }
        }
    }
}
