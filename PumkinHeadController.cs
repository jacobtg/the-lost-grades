using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumkinHeadController : MonoBehaviour
{
    private CharacterController _characterController;

    [SerializeField] private float speed = 100f;
    [SerializeField] private Vector3 endPos;
    [SerializeField] private AudioClip pumpkinSound;

    private void Start()
    {
        ScareManager.Instance.PlayScareSound(pumpkinSound);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);
        
        if (Vector3.Distance(transform.position, endPos) < 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
