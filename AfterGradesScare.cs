using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AfterGradesScare : MonoBehaviour
{
    private bool _hasBeenTriggered;

    [SerializeField] private Transform playerScarePos;
    [SerializeField] private Transform playerLookAt;
    [SerializeField] private GameObject scareObj;
    [SerializeField] private AudioClip scareSound;
    [SerializeField] private Light scareLight;
    
    private void OnTriggerEnter(Collider other)
    {
        if (ObjectivesManager.Instance.holdingGrades && !_hasBeenTriggered)
        {
            _hasBeenTriggered = true;
            ObjectivesManager.Instance.objectiveText.text = "Told you to not look back";
            ObjectivesManager.Instance.objectiveText.color = Color.red;
            ObjectivesManager.Instance.objectiveTextPanel.SetActive(true);
            StartCoroutine(ScareManager.Instance.JumpScare(playerScarePos.position, playerLookAt.position, scareObj, scareSound, scareLight, true));
        }
    }
}
