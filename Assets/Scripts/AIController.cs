using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    [Header("Waypoints")]
    [Tooltip("Drag & drop your waypoint transforms here, in the order you want the NPC to visit them.")]
    public List<Transform> waypoints = new List<Transform>();

    [Header("Movement Settings")]
    [Tooltip("Distance (in world units) at which we consider the agent to have 'reached' the waypoint.")]
    public float arriveThreshold = 0.2f;

    [Tooltip("Seconds to wait at each waypoint before moving to the next.")]
    public float waitTimeAtWaypoint = 2f;

    private NavMeshAgent agent;
    private int currentIndex = 0;
    private bool isWaiting = false;

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("AIController requires an Animator component on the same GameObject for animations.");
        }

        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("AIController requires a NavMeshAgent component on the same GameObject.");
        }

        if (waypoints.Count == 0)
        {
            Debug.LogWarning("No waypoints assigned to NPCWaypointMover.");
        }
    }

    void Start()
    {
        if (waypoints.Count > 0)
        {
            MoveToNextWaypoint();
        }
    }

    void Update()
    {
        if (waypoints.Count == 0) return;

        // Update animation state
        bool isMoving = !isWaiting && agent.remainingDistance > arriveThreshold;
        if (animator != null)
        {
            animator.SetBool("isMove", isMoving);
        }

        if (isWaiting) return;

        // Check if we've essentially "arrived" at the current waypoint
        if (!agent.pathPending && agent.remainingDistance <= arriveThreshold)
        {
            StartCoroutine(WaitAndMoveToNext());
        }
    }

    private void MoveToNextWaypoint()
    {
        if (waypoints.Count == 0) return;

        agent.SetDestination(waypoints[currentIndex].position);
    }

    private IEnumerator WaitAndMoveToNext()
    {
        isWaiting = true;
        agent.isStopped = true;

        if (animator != null)
        {
            animator.SetBool("isMove", false);
        }

        yield return new WaitForSeconds(waitTimeAtWaypoint);

        currentIndex = (currentIndex + 1) % waypoints.Count;

        agent.isStopped = false;
        MoveToNextWaypoint();

        isWaiting = false;
    }
}
