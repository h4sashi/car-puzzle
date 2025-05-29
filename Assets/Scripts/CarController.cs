using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
public class CarController : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 moveDir;
    private bool isMoving = false;
    public Image spriteImage; // the quad renderer on the roof
    public Sprite[] DirSign;     // the quad renderer on the roof
    public ArrowType arrowType;
    private List<Vector3> positionHistory = new List<Vector3>();
    private List<Quaternion> rotationHistory = new List<Quaternion>();
    private bool isRewinding = false;
    private int rewindIndex = 0;
    private float recordInterval = 0.02f;
    private float recordTimer = 0f;


    private List<Vector3> pathPoints;
    private int pathIndex = 0;
    private bool usePath = false;
    private bool rewindComplete = false;

    public bool carCrash = false;

    private MovesManager movesManager;
    public Renderer rend;
    public GameObject ideaIcon;


    void Start()
    {
        movesManager = GameObject.FindObjectOfType<MovesManager>();
        SetArrowDiection();
        AlignToDirection();
    }

    public void Initialize(Vector3 direction, ArrowType arrow, List<Vector3> path = null)
    {
        moveDir = direction.normalized;
        arrowType = arrow;

        if (path != null && path.Count > 1)
        {
            pathPoints = path;
            usePath = true;
            transform.position = pathPoints[0];
            pathIndex = 1;

            // Align to path start direction
            Vector3 initialDir = (pathPoints[1] - pathPoints[0]).normalized;
            if (initialDir != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(initialDir);
                transform.rotation = toRotation;
            }
        }
        else
        {
            AlignToDirection(); // Only use for linear cars
        }

        SetArrowDiection();
    }



    void Update()
    {
        if (isRewinding)
        {
            if (rewindIndex >= 0)
            {
                transform.position = positionHistory[rewindIndex];
                transform.rotation = rotationHistory[rewindIndex];
                rewindIndex--;
            }
            else
            {
                isRewinding = false;
                isMoving = false;
                rewindComplete = true; // Mark that we should reset path on next click
                positionHistory.Clear();
                rotationHistory.Clear();
            }

            return;
        }

        if (isMoving)
        {
            // Record state at intervals
            recordTimer += Time.deltaTime;
            if (recordTimer >= recordInterval)
            {
                positionHistory.Add(transform.position);
                rotationHistory.Add(transform.rotation);
                recordTimer = 0f;
            }

            if (usePath && pathPoints != null && pathIndex < pathPoints.Count)
            {
                Vector3 target = pathPoints[pathIndex];
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

                int lookaheadIndex = Mathf.Min(pathIndex + 1, pathPoints.Count - 1);
                Vector3 futurePoint = pathPoints[lookaheadIndex];
                Vector3 direction = (futurePoint - transform.position).normalized;

                if (direction != Vector3.zero)
                {
                    Quaternion toRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
                }

                while (pathIndex < pathPoints.Count && Vector3.Distance(transform.position, pathPoints[pathIndex]) < 0.05f)
                {
                    pathIndex++;
                }

                CheckExit();
            }
            else
            {
                transform.position += moveDir * speed * Time.deltaTime;

                if (moveDir != Vector3.zero)
                {
                    Quaternion toRotation = Quaternion.LookRotation(moveDir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
                }

                CheckExit();
            }
        }
    }


    void OnMouseDown()
    {
        if (!isMoving)
        {
            if (rewindComplete)
            {
                ResetPathState();
            }

            BeginMove();
        }
    }


    void ResetPathState()
    {
        rewindComplete = false;

        if (usePath && pathPoints != null && pathPoints.Count > 1)
        {
            transform.position = pathPoints[0];
            pathIndex = 1;

            Vector3 initialDir = (pathPoints[1] - pathPoints[0]).normalized;
            if (initialDir != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(initialDir);
                transform.rotation = toRotation;
            }
        }
        else
        {
            AlignToDirection();
        }
    }


    public void SetDirection(Vector3 direction)
    {
        moveDir = direction.normalized;
        AlignToDirection();
    }

    void BeginMove()
    {
        isMoving = true;
        movesManager.SubtractMoves(1);
    }

    void AlignToDirection()
    {
        if (moveDir == Vector3.forward)
            transform.rotation = Quaternion.Euler(0, 0, 0); // Up
        else if (moveDir == Vector3.back)
            transform.rotation = Quaternion.Euler(0, 180, 0); // Down
        else if (moveDir == Vector3.left)
            transform.rotation = Quaternion.Euler(0, -90, 0); // Left
        else if (moveDir == Vector3.right)
            transform.rotation = Quaternion.Euler(0, 90, 0); // Right
    }


    void SetArrowDiection()
    {
        switch (arrowType)
        {
            case ArrowType.Up:
                spriteImage.sprite = DirSign[0];
                break;
            case ArrowType.Down:
                spriteImage.sprite = DirSign[0];
                break;
            case ArrowType.Left:
                spriteImage.sprite = DirSign[0];
                break;
            case ArrowType.Right:
                spriteImage.sprite = DirSign[0];
                break;
            case ArrowType.LeftTurn:
                spriteImage.sprite = DirSign[0];
                break;
            case ArrowType.RightTurn:
                spriteImage.sprite = DirSign[0];
                break;
            case ArrowType.UTurn:
                spriteImage.sprite = DirSign[0];
                break;
        }
    }


    void CheckExit()
    {
        if (transform.position.magnitude > 17f)
        {
            // GameManager.Instance.AddScore();
            Destroy(gameObject);
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Car") && isMoving)
        {
            isMoving = false;
            StartCoroutine(StartRewindAfterDelay(0.3f));
        }
    }


    IEnumerator StartRewindAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        rewindIndex = positionHistory.Count - 1;
        isRewinding = true;
    }

void OnDrawGizmosSelected()
{
    if (usePath && pathPoints != null && pathIndex < pathPoints.Count)
    {
        Gizmos.color = Color.yellow;
        for (int i = pathIndex; i < pathPoints.Count - 1; i++)
        {
            Gizmos.DrawLine(pathPoints[i], pathPoints[i + 1]);
        }
    }
}

public bool HasPath()
{
    return usePath && pathPoints != null && pathPoints.Count > 1 && pathIndex < pathPoints.Count;
}

// Method to return the specific path that the car is currently traveling on.
public List<Vector3> GetPathPoints()
{
    return pathPoints;
}

public bool IsMoving()
{
    return isMoving;
}



    // void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Car") && isMoving)
    //     {
    //         GameManager.Instance.GameOver();
    //     }
    // }
}



