using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ForfeitButton : MonoBehaviour
{
    [System.NonSerialized] public string localPlayer;

    private Button button;
    private HideAndSeekManager hideAndSeekManager;

    void Awake()
    {
        hideAndSeekManager = FindObjectOfType<HideAndSeekManager>();
        button = this.GetComponent<Button>();
        button.onClick.AddListener(Forfeit);

        HideAndSeekPlayer[] players = FindObjectsOfType(typeof(HideAndSeekPlayer)) as HideAndSeekPlayer[];
        if (players.Length == 0)
        {
            Debug.LogError("No players found.", this);
        }
        //Get local player name
        foreach (HideAndSeekPlayer player in players)
        {
            if (player.isLocalPlayer)
            {
                localPlayer = player.playerName;
            }
        }
    }

    void Forfeit()
    {
        hideAndSeekManager.ForfeitPlayer(localPlayer);
    }
}
