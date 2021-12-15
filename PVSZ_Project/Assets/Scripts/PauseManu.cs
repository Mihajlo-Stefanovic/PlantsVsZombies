using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject PointerRaycaster;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            
            if(GameIsPaused)
            {
                
                Resume();
            
            }else
            {
                Pause();
                
            }
        }
    }
     public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale=1f;
        GameIsPaused=false;
        PointerRaycaster.SetActive(true);
        
    }
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale=0f;
        GameIsPaused=true;
        PointerRaycaster.SetActive(false);
    }
    public void LoadManu()
    {
        Time.timeScale=1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
