using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
//using EscapeRoom01;


	public class DialNetworkInteractable : DialInteractable
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
		
		protected override void OnSelectEntering(SelectEnterEventArgs args)
		{
			pv.RequestOwnership();
			base.OnSelectEntering(args);
			//Grab();
		}
		
		/*protected override void OnSelectEntered(SelectEnterEventArgs args)
		{
			pv.RequestOwnership();
			base.OnSelectEntered(args);
			//Grab();
		}*/
		
		/*protected override void OnSelectExited(SelectExitEventArgs args)
		{
			pv.RequestOwnership();
			base.OnSelectExited(args);
		}
		
		public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
		{
			pv.RequestOwnership();
			base.ProcessInteractable(updatePhase);
		}*/
		
		/*public override bool IsSelectableBy(XRBaseInteractor interactor)
		{
			pv.RequestOwnership();
			int interactorLayerMask = 1 << interactor.gameObject.layer;
			return base.IsSelectableBy(interactor) && (interactionLayerMask.value & interactorLayerMask) != 0 ;
		}*/
		
	}

