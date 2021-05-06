using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPhaseControl : MonoBehaviour
{
		//Cached References
	//[SerializeField]
	
		//Variables
    private Animator anim;
    void Start ()
    {
        anim = GetComponent<Animator>();
        anim.speed = 0f;
        RandomizePhase();
    }

    private void RandomizePhase()
    {
        float phase = Random.value;
        anim.Play("TestPhase", 0, phase);
        Debug.Log(phase);
    }

}
