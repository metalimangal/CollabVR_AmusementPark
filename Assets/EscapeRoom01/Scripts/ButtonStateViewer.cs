using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonStateViewer : MonoBehaviour
{
	public Button[] OtherRooms;
	public GameObject[] RoomDescriptions;
	public int ThisRoomNumber;
	Color defaultColor;
	Color pressedColor;
	Color hoverColor;
	public TextMeshProUGUI DisplayText;
    // Start is called before the first frame update
    void Start()
    {
        defaultColor = GetComponent<Image>().color;
		pressedColor = Color.black;
		hoverColor = new Color32(1,195,195,195);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void SetHoverColor()
	{
		if (GetComponent<Image>().color != pressedColor)
		{
			GetComponent<Image>().color = hoverColor;
			DisplayText.color = Color.black;
		}
		foreach (var item in RoomDescriptions)
		{
			item.SetActive(false);
		}
		RoomDescriptions[ThisRoomNumber - 1].SetActive(true);
	}
	
	public void SetPressedColor()
	{
		GetComponent<Image>().color = pressedColor;
		DisplayText.color = Color.white;
		
		foreach (var item in OtherRooms)
		{
			item.GetComponent<Image>().color = defaultColor;
			item.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
		}
	}
	
	public void SetDefaultColor()
	{
		if (GetComponent<Image>().color != pressedColor)
		{
			GetComponent<Image>().color = defaultColor;
			DisplayText.color = Color.black;
		}
		RoomDescriptions[ThisRoomNumber - 1].SetActive(false);
	}
}
