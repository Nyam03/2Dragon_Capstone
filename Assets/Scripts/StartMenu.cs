using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("IntroScene");
    }

    public void QuitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }

    public void ReStart()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void Stage1()
    {
        SceneManager.LoadScene("Stage1");
    }

    public void Stage2()
    {
        SceneManager.LoadScene("Stage2");
    }

    public void Stage3()
    {
        SceneManager.LoadScene("Stage3");
    }
}
