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
    public string playerName;

    [Header("Player Health")]
    public int maxHealth = 100;
    public int health = 100;

    public static event Action OnRespawn;


    private void Awake()
    {
        CF_GameManager.OnGameStateChanged += GameManagerOnOnGameStateChanged;
        playerName = "Player " + photonView.ViewID;
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


    public void TakeDamage(int damage, string attacker, out bool killedPlayer)
    {
        Debug.Log(damage + " damage taken from: " + attacker);
        // photonView.RPC("RPCTakeDamage", RpcTarget.All, damage.ToString());
        if (damage < health)
        {
            health -= damage;
            killedPlayer = false;
        }
        else
        {
            health = 0;
            killedPlayer = true;
        }
    }
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
    private void RPCTakeDamage(string damage)
    {
        health -= int.Parse(damage);
        Debug.Log("Damage taken: " + damage);
        GetComponent<CF_PlayerAppearance>().TakeDamage();
    }
}
