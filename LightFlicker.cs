using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LightFlicker : MonoBehaviour
{
     private Light _light;
     
     [SerializeField] private Renderer renderer;
     
     [SerializeField] private Material emissiveMat;
     [SerializeField] private Material nonEmissiveMat;
     
     private bool turnOn;
     private float elapsedTime;
     private float time;
     
     private void Start()
     {
          _light = GetComponent<Light>();

          // Start with light off
          turnOn = false;
          ToggleMaterial(false);
          time = GenerateRandomOnTime();
     }

     private void Update()
     {
          if (elapsedTime < time)
          {
               elapsedTime += Time.deltaTime;
          }
          else
          {
               _light.enabled = turnOn;
               ToggleMaterial(turnOn);
               turnOn = !turnOn;
               elapsedTime = 0.0f;
               time = turnOn ? GenerateRandomOffTime() : GenerateRandomOnTime();
          }
     }

     private float GenerateRandomOnTime()
     {
          return Random.Range(1.0f, 2.5f);
     }
     
     private float GenerateRandomOffTime()
     {
          return Random.Range(0.25f, 0.6f);
     }

     private void ToggleMaterial(bool on)
     {
          if (on)
          {
               renderer.material = emissiveMat;
          }
          else
          {
               renderer.material = nonEmissiveMat;
          }
     }
}
