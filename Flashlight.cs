using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Flashlight : MonoBehaviour
{
    public static Flashlight Instance { get; private set; }
    
    [SerializeField] private Light spotlight;
    [SerializeField] private float normalIntensity = 4.0f;
    [SerializeField] private float highIntensity = 10.0f;
    [SerializeField] private float normalIntensityRange = 14.0f;
    [SerializeField] private float highIntensityRange = 35.0f;
    [SerializeField] private float highIntensityDamagePerSec = 30.0f;
    
    [SerializeField] private bool onWhenSpawn;
    public bool canTurnOn = true;
    private bool isOn = false;
    private bool isHighIntensityOn = false;

    [SerializeField] private float batteryLevel = 100.0f;
    [SerializeField] private float normalIntensityBatteryDelta = 0.01f;
    [SerializeField] private float highIntensityBatteryDelta = 5.0f;

    [SerializeField] private TextMeshProUGUI batteryLevelText;

    [FormerlySerializedAs("fpsCamera")] [SerializeField] private Camera fpCamera;
    
    // Audio
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip turnOnLightSound;
    [SerializeField] private AudioClip turnOffLightSound;
    [SerializeField] private AudioClip cantTurnOnSound;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        fpCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();

        if (onWhenSpawn)
        {
            TurnOnLight(false);
        }
    }

    private void Update()
    {
        if (canTurnOn && batteryLevel <= 0.0f)
        {
            canTurnOn = false;
            TurnOffLight();
        }

        if (canTurnOn)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!isOn)
                {
                    TurnOnLight(false);
                }
                else
                {
                    TurnOffLight();
                }
            }
            else if (Input.GetMouseButtonDown(0)) // Strong light on!
            {
                if (isOn && !isHighIntensityOn)
                {
                    TurnOnLight(true);
                }
                else if (isOn && isHighIntensityOn)
                {
                    TurnOnLight(false);
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.F))
                audioSource.PlayOneShot(cantTurnOnSound);
        }

        if (isOn)
        {
            if (isHighIntensityOn)
            {
                DecreaseBatteryLevel(highIntensityBatteryDelta * Time.deltaTime);
            }
            else
            {
                DecreaseBatteryLevel(normalIntensityBatteryDelta * Time.deltaTime);
            }
        }
        
        if (isHighIntensityOn && Physics.Raycast(fpCamera.transform.position, fpCamera.transform.forward, out RaycastHit hit, highIntensityRange))
        {
            Debug.DrawRay(fpCamera.transform.position, fpCamera.transform.forward, Color.red);
            if (hit.collider.gameObject.CompareTag("Spider"))
            {
                var spiderController = hit.collider.gameObject.GetComponent<SpiderController>();
                spiderController.Damage(highIntensityDamagePerSec * Time.deltaTime);
            }
        }
        
        batteryLevel = Mathf.Clamp(batteryLevel, 0, 100);

        if (batteryLevel < 1.0f)
        {
            SceneManager.LoadScene("Game Over Scene");
        }
        
        batteryLevelText.text = $"{Mathf.CeilToInt(batteryLevel)}%";
    }

    public void TurnOnLight(bool turnOnHighIntensity)
    {
        audioSource.PlayOneShot(turnOnLightSound);
        if (turnOnHighIntensity)
        {
            isOn = true;
            isHighIntensityOn = true;
            spotlight.intensity = highIntensity;
            spotlight.range = highIntensityRange;
        }
        else
        {
            isOn = true;
            isHighIntensityOn = false;
            spotlight.intensity = normalIntensity;
            spotlight.range = normalIntensityRange;
        }
    }
    
    public void TurnOffLight()
    {
        audioSource.PlayOneShot(turnOffLightSound);
        spotlight.intensity = 0.0f;
        isOn = false;
        isHighIntensityOn = false;
    }

    public void DecreaseBatteryLevel(float amount)
    {
        batteryLevel -= amount;
    }
}
