using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ScareManager : MonoBehaviour
{
    public static ScareManager Instance { get; private set; }
    
    private GameObject _playerObj;
    
    private AudioSource _backgroundMusicSource;
    private AudioSource _scareSoundSource;

    // Office
    [SerializeField] private GameObject blackboard;
    [SerializeField] private Light officeBlackboardLight;
    [FormerlySerializedAs("dontRunAwaySound")] [SerializeField] private AudioClip blackboardReadSound;
    
    // Personal rum
    [SerializeField] private GameObject personalRumLightObj;

    // Grades pickup/B0
    [SerializeField] private Door B0classroomDoor;
    [SerializeField] private GameObject B0classroomLightObj;
    [SerializeField] private GameObject B0rack;
    [SerializeField] private AudioClip afterGradesSound;

    // Pumkin Head
    [SerializeField] private GameObject pumpkinHeadOfficePrefab;
    [SerializeField] private GameObject pumpkinHeadOtherSidePrefab;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _playerObj = GameObject.FindWithTag("Player");
        _backgroundMusicSource = GameObject.Find("Background Music").GetComponent<AudioSource>();
        _scareSoundSource = GetComponent<AudioSource>();
    }

    public IEnumerator OfficeScareAfterKeyPickup()
    {
        Flashlight.Instance.TurnOffLight();
        officeBlackboardLight.enabled = true;
        PlayerController.Instance.freeze = true;
        _playerObj.transform.LookAt(blackboard.transform.position);
        PlayerController.Instance.fpsCamera.transform.LookAt(blackboard.transform.position);
        _backgroundMusicSource.Pause();
        _scareSoundSource.PlayOneShot(blackboardReadSound);
        yield return new WaitForSeconds(20f);
        Flashlight.Instance.TurnOnLight(false);
        officeBlackboardLight.enabled = false;
        PlayerController.Instance.freeze = false;
        _backgroundMusicSource.Play();
    }

    public void PumpkinHeadRun(bool playerAtOfficeSide)
    {
        if (playerAtOfficeSide)
        {
            Instantiate(pumpkinHeadOtherSidePrefab);
        }
        else
        {
            Instantiate(pumpkinHeadOfficePrefab);
        }
    }
    
    public void AfterKeyPickup()
    {
        personalRumLightObj.SetActive(true);
    }
    
    public void AfterGradesPickup()
    {
        B0classroomDoor.OpenDoor();
        B0rack.transform.position = new Vector3(-1.128f, -0.737f, -0.34f);
        B0rack.transform.Rotate(0f, 0f, 90.0f);
        B0classroomLightObj.SetActive(true);
        _backgroundMusicSource.Stop();
        _backgroundMusicSource.clip = afterGradesSound;
        _backgroundMusicSource.loop = true;
        _backgroundMusicSource.Play(); 
    }

    public IEnumerator JumpScare(Vector3 playerPos, Vector3 playerLookAt, GameObject scareObject, AudioClip sound, Light light, bool isDemonScare)
    {
        _backgroundMusicSource.Pause();
        Flashlight.Instance.TurnOffLight();
        PlayerController.Instance.freeze = true;
        scareObject.SetActive(true);
        _playerObj.transform.position = playerPos;
        _playerObj.transform.LookAt(playerLookAt);
        PlayerController.Instance.fpsCamera.transform.LookAt(playerLookAt);
        light.enabled = true;
        _scareSoundSource.PlayOneShot(sound);
        yield return new WaitForSeconds(2.5f);
        Destroy(scareObject);
        Destroy(light);
        Flashlight.Instance.TurnOnLight(false);
        _scareSoundSource.Stop();
        _backgroundMusicSource.Play();
        PlayerController.Instance.freeze = false;
        if (isDemonScare)
        {
            ObjectivesManager.Instance.objectiveTextPanel.SetActive(false);
            ObjectivesManager.Instance.objectiveText.color = Color.white;
            yield return new WaitForSeconds(1f);
            ObjectivesManager.Instance.PickedUpGrades();
        }
    }

    public void PlayScareSound(AudioClip sound)
    {
        _scareSoundSource.PlayOneShot(sound);
    }
}
