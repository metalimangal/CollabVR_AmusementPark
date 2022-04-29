using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
//using EscapeRoom01;


	public class XROffsetNetworkGrabbable : XROffsetGrabbable
	{
		private PhotonView pv;
		// Start is called before the first frame update
		void Start()
		{
			pv = GetComponent<PhotonView>();
		}

		// Update is called once per frame
		void Update()
		{
			
		}
		
		protected override void OnSelectEntered(SelectEnterEventArgs evt)
		{
			pv.RequestOwnership();
			base.OnSelectEntered(evt);
			//Grab();
		}
	}

