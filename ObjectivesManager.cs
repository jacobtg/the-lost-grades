using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public enum Objectives
{
    FindKey,
    FindClassroomWithGrades,
    FindExit
}

public class ObjectivesManager : MonoBehaviour
{
    public static ObjectivesManager Instance { get; private set; }
    
    [SerializeField] public Objectives currentObjective;
    [HideInInspector] public bool holdingKey;
    [HideInInspector] public bool holdingGrades;

    [SerializeField] public GameObject objectiveTextPanel;
    [SerializeField] public TextMeshProUGUI objectiveText;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(DisplayInitialObjective());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Start Scene");
        }
    }

    private IEnumerator DisplayInitialObjective()
    {
        yield return new WaitForSeconds(1.5f);
        objectiveTextPanel.SetActive(true);
        objectiveText.text = "Find key";
        yield return new WaitForSeconds(6f);
        objectiveTextPanel.SetActive(false);
    }

    public void PickedUpKey()
    {
        holdingKey = true;
        StartCoroutine(ScareManager.Instance.OfficeScareAfterKeyPickup());
        ScareManager.Instance.AfterKeyPickup();
        NextObjective();
    }

    public void PickedUpGrades()
    {
        ScareManager.Instance.AfterGradesPickup();
        NextObjective();
    }

    private void NextObjective()
    {
        currentObjective += 1;
        StartCoroutine(DisplayNextObjective());
    }

    private IEnumerator DisplayNextObjective()
    {
        yield return new WaitForSeconds(1.5f);
        switch (currentObjective)
        {
            case Objectives.FindClassroomWithGrades:
                yield break;
            case Objectives.FindExit:
                objectiveTextPanel.SetActive(true);
                objectiveText.text = "The grades belong where you started";
                break;
            default:
                objectiveText.text = "Invalid objective [DEBUG]";
                break;
        }
        yield return new WaitForSeconds(6f);
        objectiveTextPanel.SetActive(false);
    }
}
