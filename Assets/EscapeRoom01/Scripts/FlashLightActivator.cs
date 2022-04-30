using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Photon.Realtime;
using Photon.Pun;

//using EscapeRoom01;


	public class FlashLightActivator : MonoBehaviourPun
	{
		public bool Toggle = false;
		//public UnityEvent OnActivated;
		//public UnityEvent OnDeactivated;

		bool m_Activated = false;
		private Light SpotLight;
		private PhotonView pv;
		// Start is called before the first frame update
		void Start()
		{
			pv = GetComponent<PhotonView>();
			SpotLight = GetComponent<Light>();
			SpotLight.enabled = false;
		}

		// Update is called once per frame
		void Update()
		{
			//SpotLight.enabled = false;
		}
		
		public void SwitchLight(int n)
		{
			pv.RPC("RPC_SwitchLight", RpcTarget.AllBuffered, n);
		}
		
		[PunRPC]
		void RPC_SwitchLight(int n)
		{
			if (Toggle)
			{
				if (m_Activated)
				{
					//OnDeactivated.Invoke();
					SpotLight.enabled = false;
				}
				else
				{
					//OnActivated.Invoke();
					SpotLight.enabled = true;
				}
				m_Activated = !m_Activated;
			}
			else
			{
				SpotLight.enabled = true;
				//OnActivated.Invoke();
				m_Activated = true;
			}
		}
	}

