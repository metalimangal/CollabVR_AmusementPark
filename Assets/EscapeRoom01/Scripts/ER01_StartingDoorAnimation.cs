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

public class ER01_StartingDoorAnimation : MonoBehaviourPun
{
    //public Transform RotateFrom;
    //public Transform RotateTo;
    //public float speed = 1f;
    //public float timeCount = 0.0f;
	
	public XRNode inputSource;
	
	public bool NoKeyRequired = false;
	
	public bool IsMasterDoor = false;
	
	private bool buttonInput;
	
	//public GameObject lid;
	public Vector3 targetPosition = new Vector3(0f, 0f, 0f);
	private Vector3 currentPosition;
	private Vector3 newPosition;
	
	private PhotonView pv;
	
	// 0 = book, 1 = oven, 2 = chest, 3 = downstairs
	public bool[] KeySlots;
	public bool OpenDoorIndicator;
	public AudioClip DoorOpenSound;
	private AudioSource MyAudioSource;
	
	private bool ToggleChange;
	private bool PlayMusic;
	
	static int s_IDMax = 0;

    public bool CloseCaptioned = false;
	
	int m_ID;
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
		OpenDoorIndicator = false;
		//currentPosition = lid.gameObject.transform.position;
		//KeySlots[0] = true;
	}
	
    void Update()
    {
		/*currentPosition = new Vector3(
             Mathf.LerpAngle(currentPosition.x, targetPosition.x, Time.deltaTime),
             Mathf.LerpAngle(currentPosition.y, targetPosition.y, Time.deltaTime),
             Mathf.LerpAngle(currentPosition.z, targetPosition.z, Time.deltaTime));
 
        lid.gameObject.transform.position = currentPosition;
		
		Debug.Log(lid.gameObject.transform.position);*/
		 
		/*InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
		device.TryGetFeatureValue(CommonUsages.primaryButton, out buttonInput);
		
		if (buttonInput)
		{
			OpenDoor();
		}*/
		
		//Debug.Log(gameObject.transform.position);
		if (!NoKeyRequired)
		{
			if (IsMasterDoor)
			{
				if (KeySlots[0] && KeySlots[1] && KeySlots[2] && KeySlots[3])
				{
					OpenDoor();
				}
			}
			else if (!IsMasterDoor)
			{
				if (KeySlots[0])
				{
					/*if (PlayMusic == true && ToggleChange == true)
					{
						//Play the audio you attach to the AudioSource component
						MyAudioSource.Play();
						//Ensure audio doesn’t play more than once
						ToggleChange = false;
					}*/
					OpenDoor();
					
				}
			}
		}
		else if (NoKeyRequired)
		{
			/*InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
			device.TryGetFeatureValue(CommonUsages.primaryButton, out buttonInput);
			
			if (buttonInput)
			{
				OpenDoor();
			}*/
			
		}
    }
	
	public void ActivateKeySlot(int KeyIndex)
	{
		pv.RPC("RPC_ActivateKeySlot", RpcTarget.AllBufferedViaServer, KeyIndex);
	}
	
	public void OpenDoor()
	{
		if (ToggleChange)
		{
			SFXPlayer.Instance.PlaySFX(DoorOpenSound, gameObject.transform.position, new SFXPlayer.PlayParameters()
				{
					Volume = 1.0f,
					Pitch = Random.Range(0.8f, 1.2f),
					SourceID = m_ID
				}, 2f, CloseCaptioned);
				ToggleChange = false;
		}
		pv.RPC("RPC_OpenDoor", RpcTarget.AllBufferedViaServer, 0);
	}
	
	public void OpenDoorInstantly()
	{
		if (ToggleChange)
		{
			SFXPlayer.Instance.PlaySFX(DoorOpenSound, gameObject.transform.position, new SFXPlayer.PlayParameters()
				{
					Volume = 1.0f,
					Pitch = Random.Range(0.8f, 1.2f),
					SourceID = m_ID
				}, 2f, CloseCaptioned);
				ToggleChange = false;
		}
		pv.RPC("RPC_OpenDoorInstantly", RpcTarget.AllBufferedViaServer, 0);
	}
	
	[PunRPC]
	void RPC_ActivateKeySlot(int i)
	{
		KeySlots[i] = true;
	}
	
	[PunRPC]
	void RPC_OpenDoor(int i)
	{
		currentPosition = new Vector3(
		 Mathf.LerpAngle(gameObject.transform.position.x, targetPosition.x, Time.deltaTime),
		 Mathf.LerpAngle(gameObject.transform.position.y, targetPosition.y, Time.deltaTime),
		 Mathf.LerpAngle(gameObject.transform.position.z, targetPosition.z, Time.deltaTime));
		 
		/*newPosition = new Vector3(
		 Mathf.LerpAngle(targetPosition.x, targetPosition.x, Time.deltaTime),
		 Mathf.LerpAngle(targetPosition.y, targetPosition.y, Time.deltaTime),
		 Mathf.LerpAngle(targetPosition.z, targetPosition.z, Time.deltaTime));*/

		gameObject.transform.position = currentPosition;
		
		
		
        //Check if you just set the toggle to false
        /*if (PlayMusic == false && ToggleChange == true)
        {
            //Stop the audio
            MyAudioSource.Stop();
            //Ensure audio doesn’t play more than once
            ToggleChange = false;
        }*/
		//lid.gameObject.transform.position = newPosition;
		
		//Debug.Log(lid.gameObject.transform.position);
	}
	
	[PunRPC]
	void RPC_OpenDoorInstantly(int it)
	{
		gameObject.transform.position = targetPosition;
	}
	
	
}
