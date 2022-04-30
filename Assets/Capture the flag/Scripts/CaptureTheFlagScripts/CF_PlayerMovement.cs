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
    public GameObject networkPlayerInstance;

    //Player Name
    public string playerName = "";

    //Reference to ray interactor
    private XRRayInteractor[] interactors;

    private void Awake()
    {
        CF_GameManager.OnGameStateChanged += GameStateChanged;
        CF_Player.OnRespawn += OnOnRespawn;
        CF_TeamManager.OnSetTeam += OnOnSetTeam;
    }

    private void OnDestroy()
    {
        CF_GameManager.OnGameStateChanged -= GameStateChanged;
        CF_Player.OnRespawn -= OnOnRespawn;
        CF_TeamManager.OnSetTeam -= OnOnSetTeam;
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
        interactors = FindObjectsOfType<XRRayInteractor>();
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

    private void OnOnRespawn(string ownerNumber)
    {
        if (ownerNumber == networkPlayerInstance.GetPhotonView().Owner.ActorNumber.ToString())
        {
            Debug.Log("Respawning Player: " + ownerNumber);
            StartCoroutine(Respawn(3));
        }
    }

    IEnumerator Respawn(int seconds)
    {
        Debug.Log("Respawning...");

        // Disabling Collision and Movement
        
        movement.enabled = false;
        

        // Force Drop
        foreach (var interactor in interactors)
        {
            interactor.allowSelect = false;
            interactor.allowActivate = false;
        }

        yield return new WaitForSeconds(seconds / 2);
        _xrOrigin.GetComponentInChildren<CapsuleCollider>().enabled = false;

        foreach (var interactor in interactors)
        {
            interactor.allowSelect = true;
            interactor.allowActivate = true;
        }
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
        yield return new WaitForSeconds(seconds/2);
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

    private void OnOnSetTeam(Team t)
    {
        Debug.Log("Set local player team Team: " + t);
        team = t;
        if (t == Team.BLUE)
        {
            ChangeColor(blueTeamColor);
        }
        else if (t == Team.RED)
        {
            ChangeColor(redTeamColor);
        }
        else
        {
            ChangeColor(Color.gray);
        }
    }

    private void ChangeColor(Color color)
    {
        foreach (var item in GetComponentsInChildren<Renderer>())
        {
            if (item.tag != "HealthBand")
            {
                item.material.color = color;
            }
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
                networkPlayerInstance = player.gameObject;
            }
        }
    }
}
