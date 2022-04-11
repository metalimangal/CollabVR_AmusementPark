using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class CF_Player : MonoBehaviourPunCallbacks, IPunObservable
{
    public Team team;

    public int maxHealth = 100;
    public int health = 100;
    private GameObject XROrigin;

    public static GameObject localPlayerInstance;

    private void Awake()
    {
        CF_GameManager.OnGameStateChanged += GameManagerOnOnGameStateChanged;
    }


    // Start is called before the first frame update
    void Start()
    {
        XROrigin = GameObject.FindGameObjectWithTag("Player");

        if (photonView.IsMine)
        {
            localPlayerInstance = gameObject;
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

        // If the player is killed
        if (health <= 0)
        {
            if (photonView.IsMine)
            {
                StartCoroutine(Respawn(3));
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    private void GameManagerOnOnGameStateChanged(GameState obj)
    {
        if (obj == GameState.TeamSelected && photonView.IsMine)
        {
            var teamProp = PhotonNetwork.LocalPlayer.CustomProperties["Team"];
            if (teamProp.ToString() == "BLUE")
            {
                team = Team.BLUE;
            }
            else if (teamProp.ToString() == "RED")
            {
                team = Team.RED;
            }
            else team = Team.NONE;
            Debug.Log("Network Player assigned to team: " + team.ToString());
        }
        
        if (obj == GameState.GameStart && photonView.IsMine)
        {
            StartCoroutine(Respawn(3));
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

    IEnumerator Respawn(int seconds)
    {
        health = maxHealth;
        XROrigin.GetComponentInChildren<CapsuleCollider>().enabled = false;
        Transform spawn = CF_SpawnManager.Instance.GetSpawn(team);
        XROrigin.transform.position = spawn.position;
        XROrigin.transform.rotation = spawn.rotation;
        yield return new WaitForSeconds(seconds);
        XROrigin.GetComponentInChildren<CapsuleCollider>().enabled = true;


        Debug.Log("Player Respawned");
    }

}
