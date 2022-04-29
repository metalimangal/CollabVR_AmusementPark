using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ForfeitButton : MonoBehaviour
{
    [System.NonSerialized] public string localPlayer;
    public Transform localPlayerTransform;

    private Button button;
    private HideAndSeekManager hideAndSeekManager;

    void Awake()
    {
        hideAndSeekManager = GameObject.FindGameObjectWithTag("HaSManager").GetComponent<HideAndSeekManager>();
        button = this.GetComponent<Button>();
        button.onClick.AddListener(Forfeit);
        GameObject[] temp = GameObject.FindGameObjectsWithTag("HaSHitbox");
        List<HideAndSeekPlayer> temp1 = new List<HideAndSeekPlayer>();
        foreach(GameObject obj in temp)
        {
            temp1.Add(obj.GetComponent<HideAndSeekPlayer>());
        }
        HideAndSeekPlayer[] players = temp1.ToArray();
        if (players.Length == 0)
        {
            Debug.LogError("No players found.", this);
        }
        foreach (HideAndSeekPlayer player in players)
        {
            if (player.isLocalPlayer)
            {
                localPlayer = player.playerName;
                localPlayerTransform = player.playerParentTransform;
            }
        }
    }

    void Forfeit()
    {
        List<Transform> temp = new List<Transform>();
        temp.Add(localPlayerTransform);
        hideAndSeekManager.InitiateTeleport(0, temp);
        hideAndSeekManager.ForfeitPlayer(localPlayer);
    }
}
