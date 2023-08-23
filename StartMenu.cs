using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject controlsText;
    [SerializeField] private GameObject creditsText;
    [SerializeField] private GameObject backButton;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnStartClick()
    {
        SceneManager.LoadScene("Game Scene");
    }

    public void OnControlsClick()
    {
        TurnOnControlsText();
    }

    public void OnCreditsClick()
    {
        TurnOnCreditsText();
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }

    public void OnBackClick()
    {
        TurnOnButtons();
    }
    
    private void TurnOnControlsText()
    {
        buttons.SetActive(false);
        controlsText.SetActive(true);
        backButton.SetActive(true);
    }

    private void TurnOnCreditsText()
    {
        buttons.SetActive(false);
        creditsText.SetActive(true);
        backButton.SetActive(true);
    }

    private void TurnOnButtons()
    {
        controlsText.SetActive(false);
        creditsText.SetActive(false);
        backButton.SetActive(false);
        buttons.SetActive(true);
    }
}
