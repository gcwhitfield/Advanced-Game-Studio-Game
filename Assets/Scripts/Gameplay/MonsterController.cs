using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    public NavMeshAgent agent;
    public float LookRadius = 10f;
    public DaughterController daughter;
    public Transform[] waypoints;
    int waypointsIndex;
    Vector3 waypointDestination;
    Transform player;
    float distance;
    bool isPatrol;
    
    // Start is called before the first frame update
    void Start()
    {
        player = DaughterController.Instance.transform;
        //Initialize Patrol
        waypointsIndex = 0;
        agent.stoppingDistance = 0.5f;
        isPatrol = true;
        IterateWaypoints();
        MoveToDestination();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(player.position, transform.position);
        if (distance <= LookRadius && !daughter.hidden)
        {
            agent.stoppingDistance = 2.5f;
            Chase();
            isPatrol = false;
        }
        else {
            //agent.ResetPath();
            if (!isPatrol) {
                agent.stoppingDistance = 0.5f;
                MoveToDestination();
                isPatrol = true;
            }    
            Patrol();
        }
    }
    //Draw the search range
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, LookRadius);
    }

    //Chase function
    void Chase() {
        agent.SetDestination(player.position);
        if (distance <= agent.stoppingDistance)
        {
            // TO-DO: attack player, let character die
            // rotation toward player
            FaceTarget();
        }
    }
    void FaceTarget() {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    //Patrol function
    void Patrol() {
        if (Vector3.Distance(transform.position, waypointDestination) < 3) {
            IterateWaypoints();
            MoveToDestination();
        }
    }
    void MoveToDestination() {
        waypointDestination = waypoints[waypointsIndex].position;
        agent.SetDestination(waypointDestination);
    }
    void IterateWaypoints() {
        waypointsIndex++;
        if (waypointsIndex >= waypoints.Length) {
            waypointsIndex = 0;
        }
    }
}
