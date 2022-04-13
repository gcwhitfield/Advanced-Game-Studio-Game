using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    [Header("Behavior")]
    public NavMeshAgent agent;

    public float LookRadius = 10f;
    public Transform[] waypoints;

    [Header("References")]
    public Interactable tentHidingSpot;

    public GameObject hideRing;

    public int level;

    private int waypointsIndex;
    private Vector3 waypointDestination;

    private Transform player;
    private float distance;
    private bool isPatrol;
    private bool isFrozen = false;

    // Start is called before the first frame update
    private void Start()
    {
        player = DaughterController.Instance.transform;
        //Initialize Patrol
        waypointsIndex = 0;
        agent.stoppingDistance = 0.5f;
        isPatrol = true;
        IterateWaypoints();
        MoveToDestination();
        //tentHidingSpot.ExecuteOnInteract(Destroy);
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isFrozen)
        {
            distance = Vector3.Distance(player.position, transform.position);
            if (distance <= LookRadius && !DaughterController.Instance.hidden)
            {
                agent.stoppingDistance = 2.5f;
                Chase();
                isPatrol = false;
                if (hideRing != null)
                {
                    hideRing.SetActive(true);
                }
            }
            else
            {
                //agent.ResetPath();
                if (!isPatrol)
                {
                    agent.stoppingDistance = 0.5f;
                    MoveAway();
                }
                Patrol();
                if (hideRing != null)
                {
                    hideRing.SetActive(false);
                }
            }
        }
        else
        {
            Debug.Log("Frozen");
        }
    }

    //Draw the search range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, LookRadius);
    }

    //Chase function
    private void Chase()
    {
        agent.SetDestination(player.position);
        if (distance <= agent.stoppingDistance)
        {
            // TO-DO: attack player, let character die
            // rotation toward player
        }
    }

    //private void FaceTarget()
    //{
    //    Vector3 direction = (player.position - transform.position).normalized;
    //    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
    //    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    //}

    //Patrol function
    private void Patrol()
    {
        if (Vector3.Distance(transform.position, waypointDestination) < 3)
        {
            IterateWaypoints();
            MoveToDestination();
        }
    }

    private void MoveToDestination()
    {
        waypointDestination = waypoints[waypointsIndex].position;
        agent.SetDestination(waypointDestination);
    }

    private void MoveAway()
    {
        waypointDestination = waypoints[waypoints.Length - 1].position;
        agent.SetDestination(waypointDestination);
    }

    private void IterateWaypoints()
    {
        waypointsIndex++;
        if (waypointsIndex >= waypoints.Length - 1)
        {
            waypointsIndex = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("LightCone"))
        {
            if (level == 3)
            {
                isFrozen = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("LightCone"))
        {
            isFrozen = false;
        }
    }
}