using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using TMPro;
using Photon.Realtime;

public class CF_Player : MonoBehaviourPunCallbacks, IPunObservable
{
    public Team team;

    [Header("Color Changing Stuff")]
    public Color blueTeamColor = Color.blue;
    public Color redTeamColor = Color.red;

    [Header("Player Name")]
    public string playerName = "";
    public TextMeshProUGUI nameText;

    [Header("Player Health")]
    public int maxHealth = 100;
    public int health = 100;

    public static GameObject localPlayerInstance;
    public static event Action OnRespawn;


    private void Awake()
    {
        CF_GameManager.OnGameStateChanged += GameManagerOnOnGameStateChanged;
    }


    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            playerName = "Player " + photonView.ViewID;
            photonView.RPC("ChangeName", RpcTarget.All, playerName);
        }
    }

    // Update is called once per frame
    void Update()
    {

        // If the player is killed
        if (health <= 0)
        {
            if (photonView.IsMine)
            {
                // Trigger Respawn Event
                health = maxHealth;
                OnRespawn?.Invoke();
            }
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if (photonView.IsMine)
        {
            playerName = "Player " + photonView.ViewID;
            photonView.RPC("ChangeName", RpcTarget.All, playerName);
        }
    }

    public void TakeDamage(int damage, string attacker)
    {
        health -= damage;
        Debug.Log(damage + " damage taken from: " + attacker);
        // photonView.RPC("RPCTakeDamage", RpcTarget.All, damage.ToString());
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        if (changedProps.ContainsKey("Team")) 
        {
            var teamProp = changedProps["Team"];
            if (teamProp.ToString() == "BLUE")
            {
                team = Team.BLUE;
                photonView.RPC("ChangeColor", RpcTarget.All, "BLUE");
            }
            else if (teamProp.ToString() == "RED")
            {
                team = Team.RED;
                photonView.RPC("ChangeColor", RpcTarget.All, "RED");
            }
            else 
            { 
                team = Team.NONE;
                photonView.RPC("ChangeColor", RpcTarget.All, "GREY");
            }
        }
    }
    // Confirm Team Assignment when team is selected
    private void GameManagerOnOnGameStateChanged(GameState obj)
    {
        if (obj == GameState.GameStart && photonView.IsMine)
        {
            // Trigger Respawn Event
            health = maxHealth;
            OnRespawn?.Invoke();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
        }
        else
        {
            health = (int)stream.ReceiveNext();
        }
    }

    [PunRPC]
    private void ChangeColor(string team)
    {
        var color = Color.gray;
        if (team == "BLUE")
        {
            color = blueTeamColor;
        }
        else if (team == "RED")
        {
            color = redTeamColor;
        }

        foreach (var item in GetComponentsInChildren<Renderer>())
        {
            item.material.color = color;
        }
    } 

    [PunRPC]
    private void ChangeName(string name)
    {
        nameText.text = name;
    }

    [PunRPC]
    private void RPCTakeDamage(string damage)
    {
        health -= int.Parse(damage);
        Debug.Log("Damage taken: " + damage);
    }
}
