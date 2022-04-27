using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using EscapeRoom01;


	public class KeyboardButton : MonoBehaviour
	{
		KeyboardSolid keyboard;
		TextMeshProUGUI buttonText;
		private bool InputEnabled = false;
		
		
		
		
		void Start()
		{
			keyboard = GetComponentInParent<KeyboardSolid>();
			buttonText = GetComponentInChildren<TextMeshProUGUI>();
			
			/*
			if (buttonText.text.Length == 1)
			{
				NameToButtonText();
				GetComponentInChildren<ButtonVR>().onRelease.AddListener(delegate { keyboard.InsertChar(buttonText.text); });
			}
			*/
			
			if (gameObject.tag != "NonCharKeyboard")
			{
				NameToButtonText();
			}
			
		}
		
		void Update()
		{
			if (gameObject.tag != "NonCharKeyboard")
			{
				//NameToButtonText();
				if (keyboard.EnableInput && !InputEnabled)
				{
					InputEnabled = true;
					GetComponentInChildren<ButtonVR>().onRelease.AddListener(delegate { keyboard.InsertChar(buttonText.text); });
				}
			}
		}
		
		public void NameToButtonText()
		{
			buttonText.text = gameObject.name;
		}

	}

