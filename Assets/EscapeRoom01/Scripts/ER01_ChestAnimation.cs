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

// Interpolates rotation between the rotations
// of from and to.
// (Choose from and to not to be the same as
// the object you attach this script to)

public class ER01_ChestAnimation : MonoBehaviourPun
{
    //public Transform RotateFrom;
    //public Transform RotateTo;
    //public float speed = 1f;
    //public float timeCount = 0.0f;
	
	public XRNode inputSource;
	
	private bool buttonInput;
	
	public GameObject lid;
	public Vector3 targetAngle = new Vector3(0f, 0f, 0f);
	private Vector3 currentAngle;
	private Vector3 newAngle;
	public AudioClip ChestOpenSound;
	private AudioSource MyAudioSource;
	
	private bool ToggleChange;
	private bool PlayMusic;
	
	static int s_IDMax = 0;

    public bool CloseCaptioned = false;
	
	int m_ID;
	
	private PhotonView pv;
	
	public bool[] KeySlots; // 0 = ChestKeySlot01, 1 = ChestKeySlot02
	public bool OpenChestIndicator;
	
	public void Start()
	{
		m_ID = s_IDMax;
        s_IDMax++;
		
		pv = GetComponent<PhotonView>();
		
		MyAudioSource = GetComponentInChildren<AudioSource>();
		PlayMusic = true;
		ToggleChange = true;
		
		for (int i = 0; i < KeySlots.Length - 1; i++)
		{
			KeySlots[i] = false;
		}
		OpenChestIndicator = false;
		//currentAngle = lid.gameObject.transform.localEulerAngles;
	}
	
    void Update()
    {
		/*currentAngle = new Vector3(
             Mathf.LerpAngle(currentAngle.x, targetAngle.x, Time.deltaTime),
             Mathf.LerpAngle(currentAngle.y, targetAngle.y, Time.deltaTime),
             Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));
 
        lid.gameObject.transform.localEulerAngles = currentAngle;
		
		Debug.Log(lid.gameObject.transform.localEulerAngles);*/
		 
		/*InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
		device.TryGetFeatureValue(CommonUsages.primaryButton, out buttonInput);*/
		
		
		
		if (KeySlots[0] && KeySlots[1])
		{
			if (ToggleChange)
			{
				SFXPlayer.Instance.PlaySFX(ChestOpenSound, gameObject.transform.position, new SFXPlayer.PlayParameters()
					{
						Volume = 1.0f,
						Pitch = Random.Range(0.8f, 1.2f),
						SourceID = m_ID
					}, 2f, CloseCaptioned);
					ToggleChange = false;
			}
			OpenChest();
		}
    }
	
	public void ActivateKeySlot(int KeyIndex)
	{
		pv.RPC("RPC_ActivateKeySlot", RpcTarget.AllBufferedViaServer, KeyIndex);
	}
	
	public void OpenChest()
	{
		pv.RPC("RPC_OpenChest", RpcTarget.AllBufferedViaServer, 0);
	}
	
	[PunRPC]
	void RPC_ActivateKeySlot(int i)
	{
		KeySlots[i] = true;
	}
	
	[PunRPC]
	void RPC_OpenChest(int i)
	{
		currentAngle = new Vector3(
		 Mathf.LerpAngle(lid.gameObject.transform.localEulerAngles.x, targetAngle.x, Time.deltaTime),
		 Mathf.LerpAngle(lid.gameObject.transform.localEulerAngles.y, targetAngle.y, Time.deltaTime),
		 Mathf.LerpAngle(lid.gameObject.transform.localEulerAngles.z, targetAngle.z, Time.deltaTime));
		 
		/*newAngle = new Vector3(
		 Mathf.LerpAngle(targetAngle.x, targetAngle.x, Time.deltaTime),
		 Mathf.LerpAngle(targetAngle.y, targetAngle.y, Time.deltaTime),
		 Mathf.LerpAngle(targetAngle.z, targetAngle.z, Time.deltaTime));*/

		lid.gameObject.transform.localEulerAngles = currentAngle;
		//lid.gameObject.transform.localEulerAngles = newAngle;
		
		//Debug.Log(lid.gameObject.transform.localEulerAngles);
	}
	
	
}
