using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class XRGrabNetworkInteractable : XRGrabInteractable
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
	
	/*protected override void OnSelectEnter(XRBaseInteractor interactor)
	{
		photonView.RequestOwnership();
		base.OnSelectEnter(interactor);
	}*/
	
	protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
		pv.RequestOwnership();
        base.OnSelectEntering(args);
        //Grab();
    }
}
