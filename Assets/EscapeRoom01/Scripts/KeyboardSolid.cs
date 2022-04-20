using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

	public class KeyboardSolid : MonoBehaviour
	{
		public TMP_InputField inputField;
		public GameObject normalButtons;
		public GameObject capsButtons;
		//public GameObject MyNetworkManager;
		public bool EnableInput = false;
		private bool caps;
		
		
		void Start()
		{
			caps=false;
			//EnableInput = MyNetworkManager.GetComponent<NetworkManager>().EnableInput;
		}
		
		void Update()
		{
			//EnableInput = MyNetworkManager.GetComponent<NetworkManager>().EnableInput;
		}
		
		public void InsertChar(string c)
		{
			inputField.text += c;
		}
		
		public void DeleteChar()
		{
			if (inputField.text.Length > 0)
			{
				inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
			}
		}
		
		public void InsertSpace()
		{
			inputField.text += " ";
		}
		
		public void CapsPressed()
		{
			if (!caps)
			{
				normalButtons.SetActive(false);
				capsButtons.SetActive(true);
				caps = true;
			}
			else{
				capsButtons.SetActive(false);
				normalButtons.SetActive(true);
				caps = false;
			}
		}

	}
