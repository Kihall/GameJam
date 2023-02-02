using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void PlayFirstMap()
    {
        SceneManager.LoadScene("FirstMapScene");
    }

    public void PlaySecondMap()
    {
        SceneManager.LoadScene("SecondMap");
    }

    public void QuitGame()
    {
        Debug.Log("Oyundan Çıktık!");
        Application.Quit();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    } 
}
