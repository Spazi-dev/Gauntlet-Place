using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // Cached References
    [SerializeField] Transform target;
    [SerializeField] GameObject hurtZone; 
    [SerializeField] GameObject hitZone; 
    GameObject lookTarget;
/*     GameObject player; */
    PlayerHealth playerHealth;
    NavMeshAgent navMeshAgent;
    Animator foeAnimator;
    // Variables
    [SerializeField] float chaseRange = 6f;
    [SerializeField] float trackRange = 2.5f;
    [SerializeField] float damage = 25f;
    [SerializeField] float turnSpeed = 500f;
    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;
    //bool playerInHitZone = false;
    int ticksPerSecond = 2;
    float gazeTimer;
    
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        foeAnimator = gameObject.GetComponent<Animator>();
        gazeTimer = 0f;
        if (lookTarget == null)
            lookTarget = GameObject.FindWithTag("EnemyLookTarget");
/*         if (player == null)
            player = GameObject.FindWithTag("Player"); */
        if (playerHealth == null)
            playerHealth = FindObjectOfType<PlayerHealth>();
            
    }

    void Update()
    {
        distanceToTarget = Vector3.Distance(target.position, transform.position);

        if (isProvoked) //If provoked, always aggro, todo: add timer that removes provoked if no eye contact
        {
            EngageTarget();
        }
        else if (distanceToTarget <= chaseRange) //Become provoked when target withing chase range
        {
            isProvoked = true;
            ChaseTarget();
        }
        //Check visibility to player every ticksPerSecond times per second
        CheckEyeContact();
    }

    void CheckEyeContact() //todo: Check eye contact if player within vision collider, or always if provoked
    {
        float duration = 1f / ticksPerSecond;
        gazeTimer += Time.deltaTime;
        while (gazeTimer >= duration)
        {
            gazeTimer -= duration;
            //Cast ray to player here;
            //Debug.DrawRay(transform.position, lookTarget.transform.position - transform.position, Color.white, 0.25f, true);
        }
    }

    void EngageTarget() //After detection choose whether approach or attack 
    {
            if (distanceToTarget >= navMeshAgent.stoppingDistance)
            {
                ChaseTarget();
            }

            if (distanceToTarget <= navMeshAgent.stoppingDistance)
            {
                //AttackTarget(); 
                FaceTarget();
            }
            if (distanceToTarget <= trackRange)
            {
                FaceTarget();
            }    
    }

    void AttackTarget()
    {
        //Debug.Log(name + " has seeked and is destroying " + target.name);

        foeAnimator.SetBool("Attacking", true);
        foeAnimator.SetBool("Moving", false);
        Debug.Log("Attacking to true");
        FaceTarget();
    }

    void ChaseTarget()
    {
        navMeshAgent.SetDestination(target.position);
        foeAnimator.SetBool("Moving", true);
        foeAnimator.SetBool("Attacking", false);
        Debug.Log("Attacking to false, chasing");
    }

    public void PlayerInHitZone()
    {
        AttackTarget();
    }

    void CauseHurtEvent()
    {
        playerHealth.TakeDamage(damage);
        Debug.Log("Chomp!");
    }

    void FaceTarget()
    {
        Vector3 direction = (lookTarget.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, trackRange);
    }
}