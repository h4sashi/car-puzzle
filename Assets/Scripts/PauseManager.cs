using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public bool isPaused = false;

    public GameObject pausePanel, settingsBtn, btmPanel;

    public GameObject[] carControllers;

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
        carControllers = GameObject.FindGameObjectsWithTag("Car");
        foreach (GameObject carController in carControllers)
        {
            carController.GetComponent<BoxCollider>().enabled = false; // Disable the BoxCollider to prevent interaction
            CarController cc = carController.GetComponent<CarController>();
            if (cc != null)
            {
                cc.enabled = false; // Disable the CarController script
            }
        }
        isPaused = true;
        Time.timeScale = 0f;
        settingsBtn.SetActive(false);
        btmPanel.SetActive(false);
        pausePanel.SetActive(true);

    }

    public void Resume()
    {
        carControllers = GameObject.FindGameObjectsWithTag("Car");
        foreach (GameObject carController in carControllers)
        {
        
            CarController cc = carController.GetComponent<CarController>();
            if (cc != null)
            {
                cc.enabled = true; // Enable the CarController script
                cc.gameObject.GetComponent<BoxCollider>().enabled = true; // Disable the BoxCollider to prevent interaction
            }
        }
        isPaused = false;
        Time.timeScale = 1f;

        if (SceneManager.GetActiveScene().name == "1" || SceneManager.GetActiveScene().name == "2" ||
           SceneManager.GetActiveScene().name == "3")
        {
            btmPanel.SetActive(false); // Do not resume if we are in the Home scene
        }
        else
        {
            btmPanel.SetActive(true);
        }

        settingsBtn.SetActive(true);
        pausePanel.SetActive(false); // Resume the bottom panel if we are not in the Home scene

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
