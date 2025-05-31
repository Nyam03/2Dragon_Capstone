using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;
    public GameObject SettingPanel;
    public GameObject SettBGI;

    private bool isPaused = false;

    void Start()
    {
        if (PausePanel != null)
        {
            PausePanel.SetActive(false);
            SettingPanel.SetActive(false);
            SettBGI.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Back();
                Resume();

            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        SettBGI.SetActive(false);
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        SettBGI.SetActive(false);
        PausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Setting()
    {
        PausePanel.SetActive(false);
        SettingPanel.SetActive(true);
        SettBGI.SetActive(true);
    }

    public void Back()
    {
        SettingPanel.SetActive(false);
        SettBGI.SetActive(false);
        PausePanel.SetActive(true);
    }

    public void QuitMain()
    {
        SceneManager.LoadScene("StartScene");
    }
}
