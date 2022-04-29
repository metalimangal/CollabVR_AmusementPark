using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using EscapeRoom01;


	public class ButtonStateViewer : MonoBehaviour
	{
		public Button[] OtherRooms;
		public GameObject[] RoomDescriptions;
		public int ThisRoomNumber;
		public string ButtonType;
		Color defaultColor;
		Color pressedColor;
		Color hoverColor;
		Color defaultTextColor;
		public TextMeshProUGUI DisplayText;
		// Start is called before the first frame update
		void Start()
		{
			defaultColor = GetComponent<Image>().color;
			defaultTextColor = GetComponentInChildren<TextMeshProUGUI>().color;
			pressedColor = Color.black;
			hoverColor = new Color32(1,240,195,195);
		}

		// Update is called once per frame
		void Update()
		{
			
		}
		
		public void SetHoverColor()
		{
			if (ButtonType == "Room Button")
			{
				if (GetComponent<Image>().color != pressedColor)
				{
					GetComponent<Image>().color = hoverColor;
					DisplayText.color = Color.white;
				}
				foreach (var item in RoomDescriptions)
				{
					item.SetActive(false);
				}
				RoomDescriptions[ThisRoomNumber - 1].SetActive(true);
			}
			else if (ButtonType == "Play Button")
			{
				if (GetComponent<Image>().color != pressedColor)
				{
					GetComponent<Image>().color = hoverColor;
					DisplayText.color = Color.white;
				}
			}
		}
		
		public void SetPressedColor()
		{
			if (ButtonType == "Room Button")
			{
				GetComponent<Image>().color = pressedColor;
				DisplayText.color = Color.white;
				
				foreach (var item in OtherRooms)
				{
					item.GetComponent<Image>().color = defaultColor;
					item.GetComponentInChildren<TextMeshProUGUI>().color = defaultTextColor;
				}
			}
			else if (ButtonType == "Play Button")
			{
				GetComponent<Image>().color = pressedColor;
				DisplayText.color = Color.white;
			}
		}
		
		public void SetDefaultColor()
		{
			if (ButtonType == "Room Button")
			{
				if (GetComponent<Image>().color != pressedColor)
				{
					GetComponent<Image>().color = defaultColor;
					DisplayText.color = defaultTextColor;
				}
				RoomDescriptions[ThisRoomNumber - 1].SetActive(false);
			}
			else if (ButtonType == "Play Button")
			{
				if (GetComponent<Image>().color != pressedColor)
				{
					GetComponent<Image>().color = defaultColor;
					DisplayText.color = defaultTextColor;
				}
			}
		}
	}

