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

public class ER01_WaterDropper : MonoBehaviour
{
	public UnityEvent OnTurnedOn;
	
	public XRNode inputSource;
	
	private bool buttonInput;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		/*InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
		device.TryGetFeatureValue(CommonUsages.primaryButton, out buttonInput);

		if (buttonInput)
		{
			OnTurnedOn.Invoke();
		}*/
    }
}
