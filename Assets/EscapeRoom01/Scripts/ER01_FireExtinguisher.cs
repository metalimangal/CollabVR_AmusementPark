using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ER01_FireExtinguisher : MonoBehaviour
{
	public ParticleSystem part;
	public GameObject lightObject;
	public List<ParticleCollisionEvent> collisionEvents;

	void Start()
	{
	 part = GetComponent<ParticleSystem>();
	 collisionEvents = new List<ParticleCollisionEvent>();
	}

	void OnParticleCollision(GameObject other)
	{
	 int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
	 int i = 0;

	 while (i < numCollisionEvents)
	 {
		 if (other.tag == "Fire")
		 {
			 other.SetActive(false);
			 lightObject.SetActive(false);
		 }
		 i++;
	 }
	}
}
