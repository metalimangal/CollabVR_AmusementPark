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
    public string quitGameButtonName = "Quit Game";
    public Text clockDisplay;
    public Text announcementDisplay;
    public string hiderTeamName = "Hiders";
    public string seekerTeamName = "Seekers";
    [Tooltip("The index of the location for all specator players to spawn.")] public int spectatorSpawnArea = 0;
    [Tooltip("The index of the location for all hider players to spawn when the game starts.")] public int hiderSpawnArea = 1;
    [Tooltip("The index of the location for all seeker players to spawn when the game starts.")] public int seekerSpawnArea = 2;

    private string announcementCode = "ANNOUNCEMENT";
    private bool announceTeamWin = false;
    private string winningTeamAnnouncement;

    private float clockTime = 0.0f; //The time to display on the in-game clock
    private bool gameRunning = false;
    private bool startHiders = false;
    private bool startSeekers = false;
    private Button startGameButton;
    public Button quitGameButton;
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
        //Find and sort buttons
        foreach(Button button in FindObjectsOfType<Button>())
        {
            if(button.name == startGameButtonName)
            {
                if (startGameButton)
                {
                    Debug.LogWarning("More than one start button found.  Updating to use the last found start button.");
                }
                startGameButton = button;
            }
            //if (button.name == quitGameButtonName)
            //{
            //    if (quitGameButton)
            //    {
            //        Debug.LogWarning("More than one quit button found.  Updating to use the last found quit button.");
            //    }
            //    quitGameButton = button;
            //}
        }

        //quitManager = FindObjectOfType<QuitManager>();
        teamButtons = new List<JoinTeamButton>(FindObjectsOfType<JoinTeamButton>());
        teamManager = FindObjectOfType<TeamManager>();
        teleportManager = FindObjectOfType<TeleportManager>();

        startGameButton.onClick.AddListener(StartGameAfterCountdown);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameRunning)
        {
            if (isInitiatingManager)
            {
                HiderWinCheck();
                SeekerWinCheck();
            }
            quitGameButton.interactable = false;
            startGameButton.interactable = false;
        }
        else
        {
            quitGameButton.interactable = true;
            startGameButton.interactable = true;
            if (startHiders)
            {
                StartHiderGameNow();
                startHiders = false;
            }
            if (startSeekers)
            {
                StartSeekerGameNow();
                startSeekers = false;
            }
        }
        clockDisplay.text = clockTime.ToString();
    }

    public List<Transform> FindPlayerTransformsFromNames(List<string> names)
    {
        List<Transform> transforms = new List<Transform>();

        return transforms;
    }

    public void HiderWinCheck()
    {
        if(clockTime == 0 || activeSeekers.Count == 0)
        {
            gameRunning = false;
            EndGameTeleport();
            winningTeamAnnouncement = hiderTeamName;
            announceTeamWin = true;
            AnnounceWin();
        }
    }

    public void SeekerWinCheck()
    {
        if(activeHiders.Count == 0)
        {
            gameRunning = false;
            EndGameTeleport();
            winningTeamAnnouncement = seekerTeamName;
            announceTeamWin = true;
            AnnounceWin();
        }
    }

    public void EndGameTeleport()
    {
        List<Transform> allActivePlayers = new List<Transform>();
        foreach(Transform hider in hiderTransforms)
        {
            allActivePlayers.Add(hider);
            hider.SendMessage("SetSpectator");
        }
        foreach(Transform seeker in seekerTransforms)
        {
            allActivePlayers.Add(seeker);
            seeker.SendMessage("SetSpectator");
        }

        teleportManager.objectsToTeleport = allActivePlayers;
        teleportManager.teleportLocation = seekerSpawnArea;
        teleportManager.TeleportObjectsToArea();
    }

    public void ForfeitPlayer(string playerName)
    {
        if (activeSeekers.Contains(playerName))
        {
            int idx = activeSeekers.IndexOf(playerName);
            List<Transform> temp = new List<Transform>();
            temp.Add(seekerTransforms[idx]);
            teleportManager.objectsToTeleport = temp;
            teleportManager.teleportLocation = spectatorSpawnArea;
            teleportManager.TeleportObjectsToArea();
            seekerTransforms[idx].gameObject.SendMessage("SetSpectator");
            activeSeekers.Remove(playerName);
            seekerTransforms.Remove(seekerTransforms[idx]);
        }
        else
        {
            if (activeHiders.Contains(playerName))
            {
                int idx = activeHiders.IndexOf(playerName);
                List<Transform> temp = new List<Transform>();
                temp.Add(hiderTransforms[idx]);
                teleportManager.objectsToTeleport = temp;
                teleportManager.teleportLocation = spectatorSpawnArea;
                teleportManager.TeleportObjectsToArea();
                hiderTransforms[idx].gameObject.SendMessage("SetSpectator");
                activeHiders.Remove(playerName);
                hiderTransforms.Remove(hiderTransforms[idx]);
            }
        }
    }

    public void StartGameAfterCountdown()
    {
        clockTime = timeToStart;
        StartCoroutine(CountdownTimerToZero(startHiders));
    }

    public void StartHiderGameNow()
    {
        activeHiders = teamManager.RetrieveTeamOfName(hiderTeamName).teammates;
        hiderTransforms = FindPlayerTransformsFromNames(activeHiders);
        foreach(Transform hider in hiderTransforms)
        {
            hider.gameObject.SendMessage("SetHider");
        }
        if (isInitiatingManager)    //Only the initiating manager teleports players, to prevent conflict and ensure correct destination
        {
            teleportManager.objectsToTeleport = hiderTransforms;
            teleportManager.teleportLocation = hiderSpawnArea;
            teleportManager.TeleportObjectsToArea();
        }
        clockTime = hidingTime;  //Set the local clock time to be displayed
        isInitiatingManager = false;    //Reset the initiating manager status
        StartCoroutine(CountdownTimerToZero(startSeekers));
    }

    public void StartSeekerGameNow()
    {
        activeSeekers = teamManager.RetrieveTeamOfName(seekerTeamName).teammates;
        seekerTransforms = FindPlayerTransformsFromNames(activeSeekers);
        foreach(Transform seeker in seekerTransforms)
        {
            seeker.gameObject.SendMessage("SetSeeker");
        }
        if (isInitiatingManager)    //Only the initiating manager teleports players, to prevent conflict and ensure correct destination
        {
            teleportManager.objectsToTeleport = seekerTransforms;
            teleportManager.teleportLocation = seekerSpawnArea;
            teleportManager.TeleportObjectsToArea();
        }
        clockTime = matchTime;  //Set the local clock time to be displayed
        isInitiatingManager = false;    //Reset the initiating manager status
        bool standIn = false;
        StartCoroutine(CountdownTimerToZero(standIn));
        gameRunning = true;
    }

    IEnumerator CountdownTimerToZero(bool boolToSet)
    {
        while(clockTime > 0)
        {
            yield return new WaitForSecondsRealtime(1f);
            clockTime--;
        }
        boolToSet = true;
    }

    public void AnnounceWin()
    {
        announcementDisplay.text = "Winners: " + winningTeamAnnouncement;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(clockTime);
            if (announceTeamWin)
            {
                stream.SendNext(announcementCode);
                stream.SendNext(winningTeamAnnouncement);
                announceTeamWin = false;
            }
        }
        else
        {
            clockTime = (float) stream.ReceiveNext();
            if((string)stream.PeekNext() == announcementCode)
            {
                winningTeamAnnouncement = (string)stream.ReceiveNext();
                AnnounceWin();
            }
        }
    }
}
