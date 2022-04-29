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
    private int framesToSkip = 15;
    private bool localPlayerFound = false;

    void Update()
    {
        if (!localPlayerFound)
        {
            if(framesToSkip > 0)
            {
                framesToSkip -= 1;
            }
            else
            {
                hideAndSeekManager = GameObject.FindGameObjectWithTag("HaSManager").GetComponent<HideAndSeekManager>();
                button = this.GetComponent<Button>();
                button.onClick.AddListener(Forfeit);
                GameObject[] temp = GameObject.FindGameObjectsWithTag("HaSHitbox");
                List<HideAndSeekPlayer> players = new List<HideAndSeekPlayer>();
                foreach (GameObject obj in temp)
                {
                    players.Add(obj.GetComponent<HideAndSeekPlayer>());
                    Debug.Log("Player added.");
                }
                foreach (HideAndSeekPlayer player in players)
                {
                    if (player.isLocalPlayer)
                    {
                        localPlayer = player.playerName;
                        localPlayerTransform = player.playerParentTransform;
                        Debug.Log("Local player found.");
                    }
                }
                localPlayerFound = true;
            }
        }    }

    void Forfeit()
    {
        List<Transform> temp = new List<Transform>();
        temp.Add(localPlayerTransform);
        hideAndSeekManager.InitiateTeleport(0, temp);
        //hideAndSeekManager.ForfeitPlayer(localPlayer);
    }
}
