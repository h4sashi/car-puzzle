using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class CarHintManager : MonoBehaviour
{
    public float checkRadius = 0.8f;

    public bool isHintCheckRequested = false;
    public bool hintCheckDone = false;


    void Update()
    {
        if (isHintCheckRequested && !hintCheckDone)
        {
            HighlightUnblockedCars();
            hintCheckDone = true;
        }
    }

    public void TriggerHintCheck()
    {
        isHintCheckRequested = true;
        hintCheckDone = false;
    }

   private void HighlightUnblockedCars()
{
    CarController[] allCars = FindObjectsOfType<CarController>();

    foreach (CarController car in allCars)
    {
        if (car.IsMoving() || !car.HasPath())
            continue;

        bool isBlocked = false;
        List<Vector3> path = car.GetPathPoints();

        foreach (Vector3 pathPoint in path)
        {
            foreach (CarController otherCar in allCars)
            {
                if (otherCar == car || !otherCar.gameObject.activeInHierarchy)
                    continue;

                float dist = Vector3.Distance(pathPoint, otherCar.transform.position);

                if (dist < checkRadius)
                {
                    isBlocked = true;
                    break;
                }
            }

            if (isBlocked) break;
        }

        if (!isBlocked)
        {
            HighlightCar(car);
            Debug.Log($"✅ {car.name} has a free path!");
            return; // ✅ Stop here after highlighting one car
        }
    }

    Debug.Log("❌ No car has a free path.");
}


  private Coroutine blinkingCoroutine;

private void HighlightCar(CarController car)
{
    GameObject ideaIcon = car.ideaIcon;

    if (ideaIcon != null)
    {
        ideaIcon.SetActive(true);

        // Start blinking coroutine
        Image image = ideaIcon.GetComponent<Image>();
        if (image != null)
        {
            if (blinkingCoroutine != null) StopCoroutine(blinkingCoroutine);
            blinkingCoroutine = StartCoroutine(BlinkImage(image));
        }
    }
}

private IEnumerator BlinkImage(Image image)
{
    float duration = 1f; // One blink cycle duration (fade in and out)
    float timer = 0f;
    bool fadingOut = false;

    while (true)
    {

if(image == null) yield break; // Exit if image is destroyed\\


        timer += Time.deltaTime;

        float alpha = fadingOut
            ? Mathf.Lerp(1f, 0f, timer / duration)
            : Mathf.Lerp(0f, 1f, timer / duration);

        Color color = image.color;
        color.a = alpha;
        image.color = color;

        if (timer >= duration)
        {
            timer = 0f;
            fadingOut = !fadingOut; // Switch direction
        }

        yield return null;
    }
}

}
