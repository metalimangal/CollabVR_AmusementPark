/* 
 This script can be placed on an empty game object.

 */

using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ParticleController : MonoBehaviourPun
{

    public GameObject explosion;
    public GameObject fire;

    public GameObject balloonPop;
    public GameObject waterDrop;

    public List<GameObject> particles = new List<GameObject>();
    public List<GameObject> explosionParticles = new List<GameObject>();
	
	private PhotonView pv;
	
	public AudioClip FireExtinguishSound;
	private bool ToggleChange;
	static int s_IDMax = 0;
    public bool CloseCaptioned = false;
	int m_ID;
	public bool FireExtinguishIndicator;
	//private Transform waterDropLocation;
	
	void Start()
	{
		m_ID = s_IDMax;
        s_IDMax++;
		FireExtinguishIndicator = false;
		ToggleChange = true;
		pv = GetComponent<PhotonView>();
	}
	
    // Update is called once per frame
    void Update()
    {
        /*if (particles.Count > 0)
        {
            // clean up any particles that have stopped playing
            // **Creating and Destroying gameObjects is not effcient if happening a lot, better to create an object pool to reuse objects.
            if (!particles[0].GetComponent<ParticleSystem>().isPlaying)
            {
                Destroy(particles[0]);
                particles.Remove(particles[0]);
            }
        }
        // gradually reduce explosion particles
        if (explosionParticles.Count > 0)
        {
            if (!explosionParticles[0].GetComponent<ParticleSystem>().isPlaying)
            {
                PlayFire(explosionParticles[0].transform.position);
                Destroy(explosionParticles[0]);
                explosionParticles.Remove(explosionParticles[0]);
            }
        }*/
    }

    public void PlayExplosion(Vector3 position)
    {
        GameObject temp = Instantiate(explosion);
        temp.transform.position = position;
        explosionParticles.Add(temp);
    }
    public void PlayFire(Vector3 position)
    {
        GameObject temp = Instantiate(fire);
        temp.transform.position = position;
        particles.Add(temp);
    }
    public void PlayBalloonPop(Vector3 position)
    {
        GameObject temp = Instantiate(balloonPop);
        temp.transform.position = position;
        particles.Add(temp);
    }
    public void PlayWaterDrop(Vector3 position)
    {
        GameObject temp = Instantiate(waterDrop);
        temp.transform.position = position;
        particles.Add(temp);
    }
	
	public void DropWater(Transform waterDropLocation)
	{
		GameObject temp = PhotonNetwork.Instantiate("EscapeRoom01/" + waterDrop.name, waterDropLocation.position, waterDropLocation.rotation);
		if (ToggleChange)
		{
			SFXPlayer.Instance.PlaySFX(FireExtinguishSound, gameObject.transform.position, new SFXPlayer.PlayParameters()
			{
				Volume = 1.0f,
				Pitch = Random.Range(0.8f, 1.2f),
				SourceID = m_ID
			}, 0.5f, CloseCaptioned);
			ToggleChange = false;
		}
        //temp.transform.position = transform.position;
        particles.Add(temp);
	}
}
