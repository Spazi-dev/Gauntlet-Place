using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
        // Cached References
    [SerializeField] Transform target; //Player's transform, used as target for pathfinding
    //[SerializeField] GameObject hurtZone; 
    //[SerializeField] GameObject hitZone; 
    GameObject lookTarget; //Empty in the player's waist, as player's ECM transform is clipping with ground in this case. Transform target with offset would do the same job
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
    //bool isBusy = false;
    int ticksPerSecond = 2; //Player detection raycast rate
    float gazeTimer = 0f;
    
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        foeAnimator = gameObject.GetComponent<Animator>();
        int layerMask = LayerMask.GetMask("PlayerCollision"); //Get layer mask for detecting just the player with raycasts
        //int bitShift = 1 << 3;
        //Debug.Log("bitshift should be " + bitShift);
        //Debug.Log("layermask is " + layerMask); //This should return 8
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
        else if (distanceToTarget <= chaseRange) //Become provoked when target withing chase range, todo: change from radial detection to conical vector3.dot detection
        {
            isProvoked = true;
            ChaseTarget();
        }
        //Check visibility to player every ticksPerSecond times per second
        CheckEyeContact();
    }

    void CheckEyeContact() //todo: Check eye contact if player within vision collider, or always if provoked
    {
        float duration = 1f / ticksPerSecond; //This code runs only times ticksPerSecond
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
            if (distanceToTarget <= trackRange) //Track range needs to be larger than navMeshAgent.stoppingDistance because navmesh stops just outside of the range
            {
                FaceTarget();
            }    
    }

    void AttackTarget()
    {
        foeAnimator.SetBool("Attacking", true);
        foeAnimator.SetBool("Moving", false);
        //Debug.Log("Attacking");
    }

    void ChaseTarget()
    {
        navMeshAgent.SetDestination(target.position);
        foeAnimator.SetBool("Moving", true);
        foeAnimator.SetBool("Attacking", false);
        //Debug.Log("Chasing");
    }

    void CheckHitZone() //Use spherecast or vector3.dot cone to detect if player is in front and attack
    {
        AttackTarget();
    }

    void CauseHurtEvent() //Event called from attack animation
    {
        playerHealth.TakeDamage(damage);
        Debug.Log("Chomp!");
    }

    void FaceTarget()
    {
        Vector3 direction = (lookTarget.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
        CheckHitZone(); //When player is close enough to be faced, we should check if enemy can reach with an attack
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, trackRange);
    }
}