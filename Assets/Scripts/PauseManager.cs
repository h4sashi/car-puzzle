using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public bool isPaused = false;

    public GameObject pausePanel, settingsBtn, btmPanel;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused == true)
        {
            PauseGame();
        }
        else
        {
            Resume();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        settingsBtn.SetActive(false);
        btmPanel.SetActive(false);
        pausePanel.SetActive(true);

    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        settingsBtn.SetActive(true);
        btmPanel.SetActive(true);
        pausePanel.SetActive(false);
    }

     public void Restart()
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
     public void ToggleSound()
    {
      
    }

     public void ToggleMusic()
    {
      
    }

     public void Home()
    {
       SceneManager.LoadScene("Home");
    }


    
}
