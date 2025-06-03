using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    public GameObject missionCompleteGO, topUI, btmUI, pauseUI; // Array to hold all car GameObjects
    public int carsLeft; // Total number of cars in the scene

    // Update is called once per frame
    void Update()
    {
        carsLeft = GameObject.FindGameObjectsWithTag("Car").Length;

        if (carsLeft <= 0)
        {
            if (missionCompleteGO.activeSelf)
            {
                return; // If the mission complete GameObject is already active, do nothing
            }
            else
            {
                missionCompleteGO.SetActive(true); // Activate the mission complete GameObject
                topUI.SetActive(false); // Deactivate the top UI
                btmUI.SetActive(false); // Deactivate the bottom UI
                pauseUI.SetActive(false); // Deactivate the pause UI
                Time.timeScale = 0; // Pause the game
            }

        }
    }
}
