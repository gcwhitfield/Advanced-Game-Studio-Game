using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    public NavMeshAgent enemy;
    public float LookRadius = 10f;
    public DaughterController daughter;
    Transform player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerManager.instance.Daughter.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance <= LookRadius && !daughter.Hidden)
        {
            enemy.SetDestination(player.position);
            if (distance <= enemy.stoppingDistance)
            {
                // TO-DO: attack player, let character die

                // rotation toward player
                FaceTarget();
            }
        }
        else {
            enemy.ResetPath();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, LookRadius);
    }

    void FaceTarget() {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }
}
