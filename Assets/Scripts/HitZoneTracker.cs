using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitZoneTracker : MonoBehaviour
{
	//Cached References
	[SerializeField] GameObject enemyParent;
	EnemyAI enemyAI;
	//Variables
	private void Start()
    {
        enemyAI = enemyParent.GetComponent<EnemyAI>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemyAI.PlayerInHitZone();
            //playerInHitZone = true;
        }
    }
        private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //playerInHitZone = false;
        }
    }

}
