using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

[RequireComponent(typeof(Collider))]
public class HideAndSeekPlayer : MonoBehaviourPunCallbacks, IPunObservable
{
    public Transform playerParentTransform;
    public string defaultTeam = "Spectators";
    public string seekerWeaponTag = "Seeker Weapon";
    public string hiderWeaponTag = "Hider Weapon";
    [Tooltip("Set the initial value here.")] public float health = 1.0f;
    [Tooltip("The message to send upwards in the event the local player's health is at or below 0.")] public string deathMessage = "OnLocalPlayerDeath";

    public bool isLocalPlayer = false;
    public string playerName;
    [System.NonSerialized] public bool isHider = false;
    [System.NonSerialized] public bool isSeeker = false;

    public GameObject playerNameField;

    private TeamManager teamManager;
    private List<GameObject> seekerWeapons = new List<GameObject>();
    private List<GameObject> hiderWeapons = new List<GameObject>();
    private int framesToSkip = 10;
    private bool hasJoinedTeam = false;
    private HideAndSeekManager hideAndSeekManager;
    private float startingHealth;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerName);
            stream.SendNext(health);
            stream.SendNext(isHider);
            stream.SendNext(isSeeker);
        }
        else
        {
            playerName = (string)stream.ReceiveNext();
            health = (int)stream.ReceiveNext();
            isHider = (bool)stream.ReceiveNext();
            isSeeker = (bool)stream.ReceiveNext();
        }
    }

    void Awake()
    {
        startingHealth = health;
        //Find if the player is local
        if (photonView.IsMine)
        {
            isLocalPlayer = true;
        }
        //playerName = playerNameField.GetComponent<TMP_Text>().text;
        playerName = PhotonNetwork.PlayerList.Length.ToString();
        teamManager = FindObjectOfType(typeof(TeamManager)) as TeamManager;
        //if (isLocalPlayer)
        //{
               //Join the default team upon connecting
        //}
        foreach(GameObject child in this.transform)
        {
            if(child.tag == seekerWeaponTag)
            {
                seekerWeapons.Add(child);
            }
            if(child.tag == hiderWeaponTag)
            {
                hiderWeapons.Add(child);
            }
        }
        SetActiveList(false, seekerWeapons);
        SetActiveList(false, hiderWeapons);
    }

    void Update()
    {
        if (!hasJoinedTeam)
        {
            if(framesToSkip <= 0)
            {
                this.gameObject.layer = 11;
                teamManager.ChangeTeam(defaultTeam, playerName);
                hasJoinedTeam = true;

                hideAndSeekManager = GameObject.FindGameObjectWithTag("HaSManager").GetComponent<HideAndSeekManager>();
            }
            else
            {
                framesToSkip -= 1;
            }
        }
        if (isLocalPlayer)
        {
            if(health <= 0)
            {
                //SendMessageUpwards(deathMessage);
                List<Transform> temp = new List<Transform>();
                temp.Add(playerParentTransform);
                hideAndSeekManager.InitiateTeleport(0, temp);
                health = startingHealth;
            }
        }
        if (isHider)
        {
            
        }
        if (isSeeker)
        {

        }
    }

    void OnCollisionEnter(GameObject other)
    {
        
        //if (isHider)
        //{
        //TakeDamage(other);
        //}
        health -= 1;
    }

    public void TakeDamage(GameObject weapon)
    {
        health -= weapon.GetComponent<WeaponDescriptor>().damagePerHit;
    }

    public void SetSpectator()
    {
        isHider = false;
        isSeeker = false;
        SetActiveList(false, seekerWeapons);
        SetActiveList(false, hiderWeapons);
    }

    public void SetSeeker()
    {
        isSeeker = true;
        isHider = false;
        SetActiveList(true, seekerWeapons);
        SetActiveList(false, hiderWeapons);
    }

    public void SetHider()
    {
        isHider = true;
        isSeeker = false;
        SetActiveList(true, hiderWeapons);
        SetActiveList(false, seekerWeapons);
    }

    private void SetActiveList(bool val, List<GameObject> list)
    {
        foreach(GameObject item in list)
        {
            item.SetActive(val);
        }
    }
}
