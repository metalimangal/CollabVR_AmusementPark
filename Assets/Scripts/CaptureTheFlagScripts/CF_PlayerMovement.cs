using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Realtime;

public class CF_PlayerMovement : MonoBehaviourPunCallbacks
{
    XROrigin _xrOrigin;
    CapsuleCollider _collider;

    ActionBasedContinuousMoveProvider movement;

    [Header("Jumping Stuff")]
    [SerializeField] private InputActionReference jumpActionRef;
    [SerializeField] private float jumpForce = 500.0f;
    private Rigidbody _body;
    public LayerMask GroundLayer;
    private bool isGrounded => Physics.Raycast(new Vector2(transform.position.x, transform.position.y + 2.0f), Vector3.down, 2.1f, GroundLayer);

    //Team stuff
    [Header("Team")]
    public Team team;
    public Color blueTeamColor = Color.blue;
    public Color redTeamColor = Color.red;

    //Player Name
    public string playerName = "";

    private void Awake()
    {
        CF_GameManager.OnGameStateChanged += GameStateChanged;
        CF_Player.OnRespawn += OnOnRespawn;
    }

    // Start is called before the first frame update
    void Start()
    {
        _xrOrigin = GetComponent<XROrigin>();
        _collider = GetComponentInChildren<CapsuleCollider>();
        _body = GetComponent<Rigidbody>();
        
        movement = GameObject.Find("Locomotion System").GetComponent<ActionBasedContinuousMoveProvider>();

        jumpActionRef.action.performed += OnJump;

        ChangeColor(Color.gray);
    }

    private void OnJump(InputAction.CallbackContext obj)
    {
        if (!isGrounded) { return; }
        _body.AddForce(Vector3.up * jumpForce);
    }

    // Update is called once per frame
    void Update()
    {
        _collider.height = Mathf.Clamp(
            _xrOrigin.CameraInOriginSpaceHeight,
            1f,
            2.5f);
        Vector3 center = _xrOrigin.CameraInOriginSpacePos;
        _collider.center = new Vector3(
            center.x,
            _collider.height / 2,
            center.z);
    }

    private void OnOnRespawn()
    {
        StartCoroutine(Respawn(3));
    }

    IEnumerator Respawn(int seconds)
    {
        Debug.Log("Respawning...");

        // Disabling Collision and Movement
        _xrOrigin.GetComponentInChildren<CapsuleCollider>().enabled = false;
        movement.enabled = false;

        // Disabling Controllers
        foreach (var controller in GetComponentsInChildren<ActionBasedController>())
        {
            controller.enableInputActions = false;
        }

        // Teleporting Logic
        Transform spawnPoint = CF_SpawnManager.Instance.GetSpawn(team);
        
        var heightAdjustment = _xrOrigin.Origin.transform.up * _xrOrigin.CameraInOriginSpaceHeight;
        var cameraDestination = spawnPoint.position + heightAdjustment;
        _xrOrigin.MoveCameraToWorldLocation(cameraDestination);
        if (team == Team.BLUE) _xrOrigin.MatchOriginUpCameraForward(Vector3.up, Vector3.right);
        if (team == Team.RED)  _xrOrigin.MatchOriginUpCameraForward(Vector3.up, Vector3.left);

        // Enabling Collision and Movement
        _xrOrigin.GetComponentInChildren<CapsuleCollider>().enabled = true;
        yield return new WaitForSeconds(seconds);
        movement.enabled = true;

        // Enabling Controllers
        foreach (var controller in GetComponentsInChildren<ActionBasedController>())
        {
            controller.enableInputActions = true;
        }

        Debug.Log("Player Respawned");
    }

    private void GameStateChanged(GameState obj)
    {
        // Disable movement on gamestart
        if (obj == GameState.GameStart)
        {
            movement.enabled = false;
        }

        else movement.enabled = true;
    }

    //public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    //{
    //    base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
    //    if (changedProps.ContainsKey("Team") && photonView.Owner == targetPlayer)
    //    {
    //        OnOnSetTeam();
    //    }
    //}

    private void OnOnSetTeam()
    {
        var teamProp = PhotonNetwork.LocalPlayer.CustomProperties["Team"];
        Debug.Log("Set local player team");
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
    }

    private void ChangeColor(Color color)
    {
        foreach (var item in GetComponentsInChildren<Renderer>())
        {
            item.material.color = color;
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        foreach (var player in FindObjectsOfType<CF_Player>())
        {
            if (player.transform.GetComponent<PhotonView>().IsMine)
            {
                playerName = player.playerName;
            }
        }
    }
}
