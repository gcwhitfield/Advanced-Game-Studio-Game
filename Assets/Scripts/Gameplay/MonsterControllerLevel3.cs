using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterControllerLevel3 : MonoBehaviour
{
    private enum MonsterState
    {
        ATTACKING,
        RETREATING,
        WAITING
    };

    private MonsterState currState;

    public NavMeshAgent navMeshAgent;
    public List<Transform> retreatSpots;

    // when this is set to 'true', the monster can attack. If this is set to false, then
    // the monster's attack is currently on cooldown and it can not attack yet
    private bool canAttack = true;

    private bool isFrozen = false;

    // Start is called before the first frame update
    private void Start()
    {
        currState = MonsterState.WAITING;
    }

    // called when the father shoots the monster
    private void OnMonsterAttacked()
    {
        // TODO: play the monster attacked sound
        currState = MonsterState.RETREATING;
    }

    private void Update()
    {
        Vector3 target = Vector3.zero; // 'target' is the location that the navmesh will move towards

        switch (currState)
        {
            case MonsterState.ATTACKING:
                target = FatherController.Instance.transform.position;

                // if the father and the monster are close to each other, have the monster attack the father
                float attackDist = 1;
                if (canAttack)
                {
                    if (Vector3.Distance(transform.position, FatherController.Instance.transform.position) < attackDist)
                    {
                        // TODO: play the attack sound
                        // TODO: play the damage animation
                        Debug.Log("The father has been attacked!");
                        canAttack = false;
                        StartCoroutine("ResetAttackAfterCoolDown", 1.0f);
                    }
                }

                break;

            case MonsterState.RETREATING:
                // choose a random retreat spot to move towards
                int r = Random.Range(0, retreatSpots.Count);
                target = retreatSpots[r].position;
                float dist = Vector3.Distance(target, transform.position);
                if (dist < 1)
                {
                    currState = MonsterState.ATTACKING;
                }
                break;

            case MonsterState.WAITING:
                target = transform.position;
                break;
        }

        if (!isFrozen)
        {
            navMeshAgent.SetDestination(target);
        }
    }

    private IEnumerator ResetAttackAfterCooldown(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        canAttack = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("LightCone"))
        {
            isFrozen = true;
            navMeshAgent.isStopped = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("LightCone"))
        {
            isFrozen = false;
            navMeshAgent.isStopped = false;
        }
    }
}