/*
    This script is placed on the fire particle.
    It controls the fire extinguishing rate by reducing the emission each time a 'Water' particle collides with it.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using Photon.Realtime;
using Photon.Pun;

using Random = UnityEngine.Random;

public class FireOut : MonoBehaviour {

    private ParticleSystem fireParticles;
    private ParticleSystem.EmissionModule fireEmission;
    private bool notFound = false;
    private float currentEmission = 0;
    [SerializeField] private float fadeRate = 1;
	
	private PhotonView pv;
	
	
    // Use this for initialization
    void Start()
    {
		
		pv = GetComponent<PhotonView>();
		
        // Get Particle System and emission
        if (GetComponent<ParticleSystem>())
        {
            fireParticles = GetComponent<ParticleSystem>();
            fireEmission = fireParticles.emission;
            currentEmission = fireEmission.rateOverTime.constant;
        }
        else
        {
            Debug.Log("ERROR :: FireOut - Particle System not found on object: " + gameObject.name);
            notFound = true;
        }
    }
	
	void Update () {
        // if no particle system, do nothing
        if (notFound)
            return;
        // check if fire is extinguished
        if (currentEmission <= 0)
        {
            fireParticles.Stop();
        }
	}

    private void OnParticleCollision(GameObject other)
    {
        // if fire got wet decrease emission rate
        if (other.tag == "Water")
        {
			
			pv.RPC("RPC_DestroyObject", RpcTarget.AllBuffered, 1);
            //currentEmission -= fadeRate;
            //fireEmission.rateOverTime = currentEmission;
        }
    }
	
	[PunRPC]
	void RPC_DestroyObject(int n)
	{
		Destroy(gameObject);
	}
}
