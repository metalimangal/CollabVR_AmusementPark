using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

using Photon.Realtime;
using Photon.Pun;
//using EscapeRoom01;


	public class WatchActivator : MonoBehaviourPun
	{
		public UnityEvent OnEnterAligned;
		public UnityEvent OnExitAligned;
		
		public XRNode inputSource;
		
		public PhotonView pv;
		
		private XRRig rig;
		private bool buttonInput;
		private CharacterController character;
		private bool m_WasAligned = false;
		// Start is called before the first frame update
		void Start()
		{
			pv = GetComponent<PhotonView>();
		}

		// Update is called once per frame
		void Update()
		{
			InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
			device.TryGetFeatureValue(CommonUsages.primaryButton, out buttonInput);
			
			if (pv.IsMine)
			{
				if (buttonInput)
				{
					if (!m_WasAligned)
					{
						OnEnterAligned.Invoke();
						m_WasAligned = true;
					}
				}
				else
				{
					if (m_WasAligned)
					{
						OnExitAligned.Invoke();
						m_WasAligned = false;
					}
				}
			}
		}
	}

