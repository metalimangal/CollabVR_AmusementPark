using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using TMPro;
using Photon.Realtime;

public class CF_Player : MonoBehaviourPunCallbacks
{
    public Team team;
    public string playerName;

    [Header("Player Health")]
    public int maxHealth = 100;
    public int health = 100;

    public static event Action<string> OnRespawn;


    private void Awake()
    {
        CF_GameManager.OnGameStateChanged += GameManagerOnOnGameStateChanged;
        playerName = "Player " + photonView.Owner.ActorNumber;
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
                OnRespawn?.Invoke(photonView.Owner.ActorNumber.ToString());
            }
        }
    }


    public void TakeDamage(int damage, string attacker, out bool killedPlayer)
    {
        photonView.RPC("RPCTakeDamage", RpcTarget.All, damage);
        if (damage < health)
        {
            killedPlayer = false;
        }
        else
        {
            killedPlayer = true;
        }

    }
    private void GameManagerOnOnGameStateChanged(GameState obj)
    {
        if (obj == GameState.GameStart && photonView.IsMine)
        {
            // Trigger Respawn Event
            health = maxHealth;
            OnRespawn?.Invoke(photonView.Owner.ActorNumber.ToString());
        }
    }

    [PunRPC]
    private void RPCTakeDamage(int damage)
    {
        if (!photonView.IsMine)
            return;

        if (damage < health)
        {
            health -= damage;
        }
        else
        {
            health = 0;
        }
    }
}
