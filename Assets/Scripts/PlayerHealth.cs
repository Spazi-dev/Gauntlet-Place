using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	//Cached References
	[SerializeField] float hitPoints = 100f;
	[SerializeField] ParticleSystem hurtVFX;
	//Variables
	
    private void Start()
    {
        
    }

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
        //hurtVFX.Play();
        if (hitPoints <= 0)
        {
            Debug.Log("Died!");
        }
    }

}
