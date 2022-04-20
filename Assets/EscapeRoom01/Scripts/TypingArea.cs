using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public class TypingArea : MonoBehaviour
	{
		public GameObject leftHand;
		public GameObject rightHand;
		public GameObject leftTypingSphere;
		public GameObject rightTypingSphere;
		
		private void OnTriggerEnter(Collider other)
		{
			GameObject hand = other.GetComponentInParent<HandPresence>().gameObject;
			if (hand ==null) return;
			if (hand == leftHand)
			{
				leftTypingSphere.SetActive(true);
			}
			else if (hand == rightHand)
			{
				rightTypingSphere.SetActive(true);
			}
		}
		
		
		private void OnTriggerExit(Collider other)
		{
			GameObject hand = other.GetComponentInParent<HandPresence>().gameObject;
			if (hand ==null) return;
			if (hand == leftHand)
			{
				leftTypingSphere.SetActive(false);
			}
			else if (hand == rightHand)
			{
				rightTypingSphere.SetActive(false);
			}
		}
	}
