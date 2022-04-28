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

    // when the players touch these triggers, the monster is garunteed to come attack
    public List<Interactable> monsterAttackTriggers;

    public List<Transform> retreatSpots;

    // when this is set to 'true', the monster can attack. If this is set to false, then
    // the monster's attack is currently on cooldown and it can not attack yet
    private bool canAttack = true;

    private Vector3 target = Vector3.zero; // 'target' is the location that the navmesh will move towards

    private float retreatSpeed = 10.0f;
    private float chaseSpeed = 4.0f;
    private float retreatDistance = 50.0f;

    // Start is called before the first frame update
    private void Start()
    {
        currState = MonsterState.ATTACKING;

        foreach (Interactable i in monsterAttackTriggers)
        {
            i.ExecuteOnTriggerEnter(Attack);
        }
    }

    // called when the father shoots the monster
    public void OnMonsterAttacked()
    {
        // play the monster being attacked sound by father
        AudioManager.Instance.MonsterGetBeatAudio(gameObject);
        currState = MonsterState.RETREATING;
        Retreat();
    }

    // call this function to make the monster attack the father
    public void Attack()
    {
        currState = MonsterState.ATTACKING;
    }

    private void Update()
    {
        Debug.Log("Monster state: " + currState.ToString());
        switch (currState)
        {
            case MonsterState.ATTACKING:
                target = FatherController.Instance.transform.position;
                if (Vector3.Distance(target, transform.position) < 20.0f)
                {
                    navMeshAgent.speed = chaseSpeed;
                }
                // if the father and the monster are close to each other, have the monster attack the father
                float attackDist = 1;
                if (canAttack)
                {
                    if (Vector3.Distance(transform.position, FatherController.Instance.transform.position) < attackDist)
                    {
                        // play the attack sound
                        AudioManager.Instance.MonsterAttackBeastAudio(FatherController.Instance.gameObject);
                        // TODO: play the damage animation
                        Debug.Log("The father has been attacked!");
                        canAttack = false;
                        StartCoroutine("ResetAttackAfterCoolDown", 1.0f);
                    }
                }

                break;

            case MonsterState.RETREATING:
                float dist = Vector3.Distance(target, transform.position);
                if (dist < 3.0f)
                {
                    transform.position = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized * retreatDistance +
                                FatherController.Instance.transform.position;
                    currState = MonsterState.ATTACKING;
                }
                break;

            case MonsterState.WAITING:
                target = transform.position;
                break;
        }

        navMeshAgent.SetDestination(target);
    }

    private void Retreat()
    {
        Vector3 fatherPosition = FatherController.Instance.transform.position;
        Vector3 direction = transform.position - fatherPosition;
        direction = new Vector3(direction.x, 0, direction.z).normalized;
        target = direction * retreatDistance + fatherPosition;
        navMeshAgent.speed = retreatSpeed;
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
            currState = MonsterState.WAITING;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("LightCone"))
        {
            if (currState != MonsterState.RETREATING)
                currState = MonsterState.ATTACKING;
        }
    }
}