using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CF_TeamSelectUIManager : MonoBehaviour
{
    public GameObject StartButtonUI;
    private void Awake()
    {
        CF_GameManager.OnGameStateChanged += OnOnGameStateChanged;
        StartButtonUI.SetActive(false);
    }

    private void OnOnGameStateChanged(GameState obj)
    {
        StartButtonUI.SetActive(obj == GameState.TeamSelected);
    }
}
