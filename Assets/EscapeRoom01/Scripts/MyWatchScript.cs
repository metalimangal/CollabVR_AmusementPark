using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
//using EscapeRoom01;


public class MyWatchScript : MonoBehaviour
{
	
	//by making a script subclass that, the wrist will pick them up at start time and will notify them it's been loaded
	//this will allow them to add button to the UI that are scene specific.
	public abstract class IUIHook : MonoBehaviour
	{
		public abstract void GetHook(MyWatchScript watch);
	}
	
	public float LoadingTime = 2.0f;
	public Slider LoadingSlider;
	public Text GameInfoText;
	public Text TimerText;
	public GameObject InstructionsPanel;

	[Header("UI")]
	public Canvas RootCanvas;
	public Transform ButtonParentTransform;
	public Button ButtonPrefab;
	public Toggle TogglePrefab;
	
	[Header("Event")]
	public UnityEvent OnLoaded;
	public UnityEvent OnUnloaded;

	public GameObject LeftUILineRenderer;
	public GameObject RightUILineRenderer;
	
	bool m_Loading = false;
	float m_LoadingTimer;
	
	public bool InstructionsOn = false;

	void Start()
	{
		LoadingSlider.gameObject.SetActive(false);
		GameInfoText.gameObject.SetActive(false);
		TimerText.gameObject.SetActive(false);
		InstructionsPanel.gameObject.SetActive(false);
		InstructionsOn = false;

		var hooks = FindObjectsOfType<IUIHook>();
		foreach (var h in hooks)
		{
			h.GetHook(this);
		}
		
		RootCanvas.worldCamera = Camera.main;
	}

	// Update is called once per frame
	void Update()
	{
		if (m_Loading)
		{
			m_LoadingTimer += Time.deltaTime;
			LoadingSlider.value = Mathf.Clamp01(m_LoadingTimer / LoadingTime);
			if (m_LoadingTimer >= LoadingTime)
			{
				OnLoaded.Invoke();
				LeftUILineRenderer.SetActive(true);
				LoadingSlider.gameObject.SetActive(false);
				GameInfoText.gameObject.SetActive(true);
				TimerText.gameObject.SetActive(true);
				//InstructionsPanel.gameObject.SetActive(true);
				m_Loading = false;
			}
		}
		if (InstructionsOn)
		{
			InstructionsPanel.gameObject.SetActive(true);
		}
	}

	public void LookedAt()
	{
		m_Loading = true;
		m_LoadingTimer = 0.0f;
		LoadingSlider.value = 0.0f;
		LoadingSlider.gameObject.SetActive(true);
		//GameInfoText.gameObject.SetActive(true);
	}

	public void LookedAway()
	{
		m_Loading = false;
		OnUnloaded.Invoke();
		LoadingSlider.gameObject.SetActive(false);
		GameInfoText.gameObject.SetActive(false);
		TimerText.gameObject.SetActive(false);
		InstructionsPanel.gameObject.SetActive(false);
		LeftUILineRenderer.SetActive(false);
		RightUILineRenderer.SetActive(false);
	}

	public void AddButton(string name, UnityAction clickedEvent)
	{
		var newButton = Instantiate(ButtonPrefab, ButtonParentTransform);
		var text = newButton.GetComponentInChildren<Text>();
		
		RecursiveLayerChange(newButton.transform, ButtonParentTransform.gameObject.layer);

		text.text = name;

		newButton.onClick.AddListener(clickedEvent);
	}

	public void AddToggle(string name, UnityAction<bool> checkedEvent)
	{
		var newToggle = Instantiate(TogglePrefab, ButtonParentTransform);
		var text = newToggle.GetComponentInChildren<Text>();
		
		RecursiveLayerChange(newToggle.transform, ButtonParentTransform.gameObject.layer);

		text.text = name;
		
		newToggle.onValueChanged.AddListener(checkedEvent);
	}
	
	public void ToggleInstructions()
	{
		if (InstructionsOn)
		{
			InstructionsOn = false;
		}
		else if (!InstructionsOn)
		{
			InstructionsOn = true;
		}
	}

	void RecursiveLayerChange(Transform root, int layer)
	{
		foreach (Transform t in root)
		{
			RecursiveLayerChange(t, layer);
		}

		root.gameObject.layer = layer;
	}
}

