using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CF_PlayerTeam : MonoBehaviourPunCallbacks
{
    public CF_PlayerAppearance playerAppearance;
    public CF_Player player;
    public Team team;
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        if (changedProps.ContainsKey("Team") && photonView.Owner == targetPlayer)
        {
            Debug.Log("team property updated");
            photonView.RPC("ChangeTeam", RpcTarget.All);
            playerAppearance.ChangeColorForTeam(changedProps["Team"].ToString());
        }
    }

    [PunRPC]
    private void ChangeTeam()
    {
        string t = photonView.Owner.CustomProperties["Team"].ToString();
        Debug.Log(photonView.Owner.ActorNumber.ToString());
        if (t == "BLUE")
        {
            team = Team.BLUE;
        }
        else if (t == "RED")
        {
            team = Team.RED;
        }
        else
        {
            team = Team.NONE;
        }
        player.team = team;
    }
}
