using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinHeadTrigger : MonoBehaviour
{
    [SerializeField] private bool mustHoldKeys;
    [SerializeField] private bool mustHoldGrades;
    [SerializeField] private bool officeSide;
    [SerializeField] private GameObject otherTrigger;

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

            if (shouldPlay)
            {
                ScareManager.Instance.PumpkinHeadRun(officeSide);
                
                Destroy(otherTrigger);
                Destroy(gameObject);
            }
        }
    }
}
