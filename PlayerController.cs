using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    
    private CharacterController _characterController;

    [HideInInspector] public bool freeze;

    private bool _isDead;
    [SerializeField] private float health = 100.0f;
    [SerializeField] private float gravity = 9.82f;
    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] public Camera fpsCamera;

    [FormerlySerializedAs("pickUpText")] [SerializeField] private TextMeshProUGUI interactionText;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip doorOpenSound;
    [SerializeField] private AudioClip doorLockedSound;
    private void Awake()
    {
        Instance = this;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _characterController = GetComponent<CharacterController>();
        _audioSource = GetComponent<AudioSource>();
    }
    
    private void Update()
    {
        health = Mathf.Clamp(health, 0f, 100f);
        if (health < 1.0f)
        {
            _isDead = true;
        }
        
        if (!_isDead)
        {
            // TEEMPPPPPPP
            if (Input.GetKeyDown(KeyCode.Q))
                Application.Quit();

            if (!freeze)
            {
                // Get WASD input
                Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                // Normalize and create direction from that
                Vector2 moveInputDirection = moveInput.normalized;

                // Set player rotation to camera rotation
                Vector3 playerRotation = Vector3.zero;
                // Only interested in y rotation
                playerRotation.y = fpsCamera.transform.eulerAngles.y;
                transform.eulerAngles = playerRotation;

                // Get move angle from input direction
                float moveAngle = Mathf.Atan2(moveInputDirection.x, moveInputDirection.y) * Mathf.Rad2Deg;

                // Get current angle of camera and add the new move angle.
                // Calculate new move direction from that with some quaternion magic.
                Vector3 playerForwardDir =
                    Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, moveAngle, 0)) *
                    Vector3.forward;
                playerForwardDir.Normalize();

                // Apply gravity to player
                _characterController.Move(gravity * Time.deltaTime * Vector3.down);

                if (moveInputDirection.magnitude != 0.0f && _characterController.isGrounded)
                {
                    _characterController.Move(moveSpeed * Time.deltaTime * playerForwardDir);

                    if (!_audioSource.isPlaying)
                    {
                        _audioSource.PlayOneShot(walkSound);
                    }
                }
                else
                {
                    _audioSource.Stop();
                }
            }

            if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out RaycastHit hit))
            {
                var hitObject = hit.collider.gameObject;
                bool displayingInteractionText = false;

                if (hitObject.CompareTag("Openable Door") && hit.distance <= 3.5f)
                {
                    ShowOpenText("door");
                    displayingInteractionText = true;
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        var rootDoor = hitObject.transform.root.gameObject;
                        interactionText.enabled = false;
                        if (ObjectivesManager.Instance.holdingKey)
                        {
                            // Play open door sound
                            rootDoor.GetComponent<AudioSource>().PlayOneShot(doorOpenSound);
                            rootDoor.GetComponent<Door>().OpenDoor();
                            hitObject.tag = "Untagged";
                        }
                        else
                        {
                            rootDoor.GetComponent<AudioSource>().PlayOneShot((doorLockedSound));
                        }
                    }
                } 
                else if (hitObject.CompareTag("Unlocked Door") && hit.distance <= 3.5f)
                {
                    ShowOpenText("door");
                    displayingInteractionText = true;
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        var rootDoor = hitObject.transform.root.gameObject;
                        interactionText.enabled = false;
                        // Play open door sound
                        rootDoor.GetComponent<AudioSource>().PlayOneShot(doorOpenSound);
                        rootDoor.GetComponent<Door>().OpenDoor();
                        hitObject.tag = "Untagged";
                    }
                }

                if (hitObject.CompareTag("Clipboard") &&
                    ObjectivesManager.Instance.currentObjective == Objectives.FindClassroomWithGrades &&
                    hit.distance <= 1.8f)
                {
                    ShowPickUpText("grades");
                    displayingInteractionText = true;
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactionText.enabled = false;
                        ObjectivesManager.Instance.holdingGrades = true;
                        Destroy(hitObject);
                    }
                }

                if (hitObject.CompareTag("Key") && ObjectivesManager.Instance.currentObjective == Objectives.FindKey &&
                    hit.distance <= 1.8f)
                {
                    ShowPickUpText("key");
                    displayingInteractionText = true;
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactionText.enabled = false;
                        ObjectivesManager.Instance.PickedUpKey();
                        Destroy(hitObject);
                    }
                }

                if (hitObject.CompareTag("Grades Table") &&
                    ObjectivesManager.Instance.currentObjective == Objectives.FindExit && ObjectivesManager.Instance.holdingGrades && hit.distance <= 3.5)
                {
                    ShowPlaceText("grades");
                    displayingInteractionText = true;
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactionText.enabled = false;
                        SceneManager.LoadScene("End Scene");
                    }
                }

                if (!displayingInteractionText)
                {
                    interactionText.enabled = false;
                }
            }
        }
        else
        {
            Debug.Log("You are dead.");
        }
    }

    private void ShowPickUpText(string itemText)
    {
        interactionText.text = $"Pick up {itemText} [E]";
        interactionText.enabled = true;
    }
    
    private void ShowOpenText(string itemText)
    {
        interactionText.text = $"Open {itemText} [E]";
        interactionText.enabled = true;
    }

    private void ShowPlaceText(string itemText)
    {
        interactionText.text = $"Place {itemText} and escape [E]";
        interactionText.enabled = true;
    }

    public void Damage(float damageAmount)
    {
        health -= damageAmount;
    }
}