using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using TMPro;

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
            localPlayerInstance = gameObject;
            playerName = "Player " + photonView.ViewID;
            nameText.text = playerName;
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
                // Trigger Respawn Event
                health = maxHealth;
                OnRespawn?.Invoke();
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Damage taken: " + damage);
    }


    // Confirm Team Assignment when team is selected
    private void GameManagerOnOnGameStateChanged(GameState obj)
    {
        if (obj == GameState.TeamSelected && photonView.IsMine)
        {
            var teamProp = PhotonNetwork.LocalPlayer.CustomProperties["Team"];
            if (teamProp.ToString() == "BLUE")
            {
                team = Team.BLUE;
                ChangeColor(blueTeamColor);
            }
            else if (teamProp.ToString() == "RED")
            {
                team = Team.RED;
                ChangeColor(redTeamColor);
            }
            else 
            { 
                team = Team.NONE;
                ChangeColor(Color.gray);
            }

            Debug.Log("Network Player assigned to team: " + team.ToString());
        }
        
        if (obj == GameState.GameStart && photonView.IsMine)
        {
            // Trigger Respawn Event
            Debug.Log("Invoking Respawn");
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

    private void ChangeColor(Color color)
    {
        foreach (var item in GetComponentsInChildren<Renderer>())
        {
            item.material.color = color;
        }
    } 
}
