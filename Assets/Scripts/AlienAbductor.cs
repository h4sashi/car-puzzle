using UnityEngine;
using System.Collections;

public class AlienAbductor : MonoBehaviour
{
    [Header("Abduction Settings")]
    public float moveSpeed       = 10f;    // units per second
    public float hoverHeight     = 5f;     // how high above the car to hover
    public float abductDuration  = 0.3f;   // seconds to “hover” and disable
    public Vector3 flyAwayOffset = new Vector3(0, 20f, 0); // relative off-screen
    public GameObject abductionLight;

    /// <summary>
    /// Call this to abduct a car.
    /// </summary>
    public void Abduct(GameObject car)
    {
        StopAllCoroutines();
        StartCoroutine(AbductCoroutine(car.transform));
    }

    private IEnumerator AbductCoroutine(Transform car)
    {
        // 1) Fly to a point just above the car
        Vector3 targetPos = car.position + Vector3.up * hoverHeight;
        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, 
                targetPos, 
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        // Optional: rotate to face down
        // transform.LookAt(car.position);

        // 2) Hover & "abduct" (pause)
        abductionLight.SetActive(true);
        yield return new WaitForSeconds(abductDuration);

        // 3) Disable the car to simulate abduction
        abductionLight.SetActive(false);
        car.gameObject.SetActive(false);

        // 4) Fly off the top of the screen
        Vector3 offscreenPos = transform.position + flyAwayOffset;
        while (Vector3.Distance(transform.position, offscreenPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, 
                offscreenPos, 
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        // (Optional) Destroy or recycle this UFO
        // Destroy(gameObject);
    }

    public GameObject[] cars;
    public void DoRandomAbduct(){
        cars = GameObject.FindGameObjectsWithTag("Car");
        Abduct(cars[Random.Range(0, cars.Length)]);
        // cars = null;
    }

    
}