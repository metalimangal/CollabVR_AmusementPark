using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class HideAndSeekManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [Tooltip("How long a match should last.")] public float matchTime = 180f;
    [Tooltip("How long to wait before the game starts.")] public float timeToStart = 30f;
    [Tooltip("How long seekers must wait for hiders to hide.")] public float hidingTime = 30f;
    [Tooltip("The time remaining before team changes are disabled.")] public float disableTeamChangeAt = 5f;
    public string startGameButtonName = "Start Game";
    public Text clockDisplay;
    public string hiderTeamName = "Hiders";
    public string seekerTeamName = "Seekers";
    [Tooltip("The index of the location for all specator players to spawn.")] public int spectatorSpawnArea = 0;
    [Tooltip("The index of the location for all hider players to spawn when the game starts.")] public int hiderSpawnArea = 1;
    [Tooltip("The index of the location for all seeker players to spawn when the game starts.")] public int seekerSpawnArea = 2;

    private float clockTime = 0.0f; //The time to display on the in-game clock
    private bool gameRunning = false;
    private Button startGameButton;
    private List<JoinTeamButton> teamButtons;
    private QuitManager quitManager;
    private TeleportManager teleportManager;
    private TeamManager teamManager;
    private List<string> activeHiders = new List<string>();
    private List<string> activeSeekers = new List<string>();
    private List<Transform> hiderTransforms = new List<Transform>();
    private List<Transform> seekerTransforms = new List<Transform>();
    private bool isInitiatingManager = false;   //Determine whether this manager is the one initiating a game start (used for teleporting players)

    // Start is called before the first frame update
    void Start()
    {
        //Find start button
        quitManager = FindObjectOfType<QuitManager>();
        teamButtons = new List<JoinTeamButton>(FindObjectsOfType<JoinTeamButton>());
        teamManager = FindObjectOfType<TeamManager>();
        teleportManager = FindObjectOfType<TeleportManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameRunning)
        {
            //If all active players are gone from a team, the game ends and that team loses
            //If the time runs out, hiders win and seekers lose
            //Disable exiting the room (at least for the host)
            //Disable start button
        }
        else
        {
            //Enable exiting the room
            //Enable start button
        }
    }

    public void ForfeitPlayer(string playerName)
    {
        //Remove player from appropriate active player list
        //Tell player to behave as a spectator
        //Teleport player to specator spawn
    }

    public void StartGameAfterCountdown()
    {
        clockTime = timeToStart;
    }

    public void StartHiderGameNow()
    {
        activeHiders = teamManager.RetrieveTeamOfName(hiderTeamName).teammates;
        //Tell local player what team they are on
        if (isInitiatingManager)    //Only the initiating manager teleports players, to prevent conflict and ensure correct destination
        {
            //Find hider player transforms
            teleportManager.objectsToTeleport = hiderTransforms;
            teleportManager.teleportLocation = hiderSpawnArea;
            teleportManager.TeleportObjectsToArea();
        }
        clockTime = hidingTime;  //Set the local clock time to be displayed
        isInitiatingManager = false;    //Reset the initiating manager status
        //Start seeker spawn countdown
    }

    public void StartSeekerGameNow()
    {
        activeSeekers = teamManager.RetrieveTeamOfName(seekerTeamName).teammates;
        //Tell local player what team they are on
        if (isInitiatingManager)    //Only the initiating manager teleports players, to prevent conflict and ensure correct destination
        {
            //Find seeker player transforms
            teleportManager.objectsToTeleport = seekerTransforms;
            teleportManager.teleportLocation = seekerSpawnArea;
            teleportManager.TeleportObjectsToArea();
        }
        clockTime = matchTime;  //Set the local clock time to be displayed
        isInitiatingManager = false;    //Reset the initiating manager status
        //Start game end countdown
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(clockTime);
        }
        else
        {
            clockTime = (float) stream.ReceiveNext();
        }
    }
}
