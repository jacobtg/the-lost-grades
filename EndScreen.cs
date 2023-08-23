using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnPlayAgainClick()
    {
        SceneManager.LoadScene("Game Scene");
    }

    public void OnMainMenuClick()
    {
        SceneManager.LoadScene("Start Scene");
    }
}
