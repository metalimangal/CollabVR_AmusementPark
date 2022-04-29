using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class HideAndSeekManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [Tooltip("How long a match should last.")] public float matchTime = 180f;
    [Tooltip("How long to wait before the game starts.")] public float timeToStart = 10f;
    [Tooltip("How long seekers must wait for hiders to hide.")] public float hidingTime = 30f;
    [Tooltip("The time remaining before team changes are disabled.")] public float disableTeamChangeAt = 5f;
    //public string startGameButtonName = "Start Game";
    //public string quitGameButtonName = "Quit Game";
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
    public Button startGameButton;
    public Button quitGameButton;
    private List<JoinTeamButton> teamButtons;
    //private QuitManager quitManager;
    private TeleportManager teleportManager;
    private TeamManager teamManager;
    private List<string> activeHiders = new List<string>();
    private List<string> activeSeekers = new List<string>();
    private List<Transform> hiderTransforms = new List<Transform>();
    private List<Transform> seekerTransforms = new List<Transform>();
    private List<HideAndSeekPlayer> hiderScripts = new List<HideAndSeekPlayer>();
    private List<HideAndSeekPlayer> seekerScripts = new List<HideAndSeekPlayer>();
    private bool isInitiatingManager = false;   //Determine whether this manager is the one initiating a game start (used for teleporting players)
    private bool startNetworkCountdown = false;

    // Start is called before the first frame update
    void Start()
    {
        //Find and sort buttons
        //foreach(Button button in FindObjectsOfType<Button>())
        //{
        //    if(button.name == startGameButtonName)
        //    {
        //        if (startGameButton)
        //        {
        //            Debug.LogWarning("More than one start button found.  Updating to use the last found start button.");
        //        }
        //        startGameButton = button;
        //    }
            //if (button.name == quitGameButtonName)
            //{
            //    if (quitGameButton)
            //    {
            //        Debug.LogWarning("More than one quit button found.  Updating to use the last found quit button.");
            //    }
            //    quitGameButton = button;
            //}
        //}

        //quitManager = FindObjectOfType<QuitManager>();
        teamButtons = new List<JoinTeamButton>(FindObjectsOfType<JoinTeamButton>());
        teamManager = this.gameObject.GetComponent<TeamManager>();
        teleportManager = this.gameObject.GetComponent<TeleportManager>();

        //startGameButton.onClick.AddListener(StartGameAfterCountdown);
    }

    // Update is called once per frame
    void Update()
    {
        if((float) clockTime > 0)
        {
            quitGameButton.interactable = false;
            startGameButton.interactable = false;
        }
        else
        {
            quitGameButton.interactable = true;
            startGameButton.interactable = true;
        }
        if (gameRunning)
        {
            if (false)//isInitiatingManager)
            {
                HiderWinCheck();
                SeekerWinCheck();
            }
        }
        else
        {
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
        if (startNetworkCountdown)
        {
            StartCountdown();
            startNetworkCountdown = false;
        }
    }

    public List<Transform> FindPlayerTransformsFromNames(List<string> names)
    {
        List<Transform> transforms = new List<Transform>();
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("HaSPlayer"))
        {
            if (names.Contains(player.GetComponentInChildren<HideAndSeekPlayer>().playerName))
            {
                transforms.Add(player.transform);
            }
        }
        //foreach(GameObject scriptOwner in GameObject.FindGameObjectsWithTag("HaSHitbox"))
        //{
        //    transforms.Add(scriptOwner.GetComponent<HideAndSeekPlayer>().playerParentTransform);
        //    Debug.Log(scriptOwner.GetComponent<HideAndSeekPlayer>().playerParentTransform);
        //}
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
        StopAllCoroutines();
        clockTime = 0;
        List<Transform> allActivePlayers = new List<Transform>();
        foreach(Transform hider in hiderTransforms)
        {
            allActivePlayers.Add(hider);
            hider.BroadcastMessage("SetSpectator");
        }
        foreach(Transform seeker in seekerTransforms)
        {
            allActivePlayers.Add(seeker);
            seeker.BroadcastMessage("SetSpectator");
        }

        InitiateTeleport(spectatorSpawnArea, allActivePlayers);
    }

    public void ForfeitPlayer(string playerName)
    {
        if (activeSeekers.Contains(playerName))
        {
            int idx = activeSeekers.IndexOf(playerName);
            List<Transform> temp = new List<Transform>();
            temp.Add(seekerTransforms[idx]);
            seekerTransforms[idx].BroadcastMessage("SetSpectator");
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
                hiderTransforms[idx].BroadcastMessage("SetSpectator");
                activeHiders.Remove(playerName);
                hiderTransforms.Remove(hiderTransforms[idx]);
            }
        }
    }

    public void StartGameAfterCountdown()
    {
        startNetworkCountdown = true;
    }

    public void StartCountdown()
    {
        isInitiatingManager = true;
        clockTime = timeToStart;
        StartCoroutine(CountdownTimerToZero(1));
    }

    public void StartHiderGameNow()
    {
        activeHiders = teamManager.RetrieveTeamOfName(hiderTeamName).teammates;
        foreach(string hider in activeHiders)
        {
            Debug.Log(hider);
        }
        hiderTransforms = FindPlayerTransformsFromNames(activeHiders);
        Debug.Log(hiderTransforms.Count);
        foreach(Transform hider in hiderTransforms)
        {
            hider.BroadcastMessage("SetHider");
        }
        //if (isInitiatingManager)    //Only the initiating manager teleports players, to prevent conflict and ensure correct destination
        //{
            InitiateTeleport(hiderSpawnArea, hiderTransforms);
        //}
        clockTime = hidingTime;  //Set the local clock time to be displayed
        StartCoroutine(CountdownTimerToZero(0));
    }

    public void StartSeekerGameNow()
    {
        activeSeekers = teamManager.RetrieveTeamOfName(seekerTeamName).teammates;
        seekerTransforms = FindPlayerTransformsFromNames(activeSeekers);
        foreach(Transform seeker in seekerTransforms)
        {
            seeker.gameObject.BroadcastMessage("SetSeeker");
        }
        //if (isInitiatingManager)    //Only the initiating manager teleports players, to prevent conflict and ensure correct destination
        //{
            InitiateTeleport(seekerSpawnArea, seekerTransforms);
        //}
        clockTime = matchTime;  //Set the local clock time to be displayed
        //isInitiatingManager = false;    //Reset the initiating manager status
        //bool standIn = false;
        StartCoroutine(CountdownTimerToZero(2));
        isInitiatingManager = false;    //Reset the initiating manager status
        gameRunning = true;
    }

    public void InitiateTeleport(int loc, List<Transform> transforms)
    {
        while(teleportManager.objectsToTeleport.Count > 0)
        {
            teleportManager.objectsToTeleport.RemoveAt(0);
        }
        foreach(Transform transform in transforms)
        {
            teleportManager.objectsToTeleport.Add(transform);
        }
        teleportManager.teleportLocation = loc;
        teleportManager.SendMessage("TeleportObjectsToArea");
    }

    IEnumerator CountdownTimerToZero(int boolToSet)
    {
        //StopAllCoroutines();
        while(clockTime > 0)
        {
            yield return new WaitForSecondsRealtime(1f);
            clockTime-=1;
        }
        if(boolToSet == 0)
        {
            startSeekers = true;
        }
        if(boolToSet == 1)
        {
            startHiders = true;
        }
        if(boolToSet == 2)
        {
            gameRunning = true;
        }
    }

    public void AnnounceWin()
    {
        announcementDisplay.text = "Winners: " + winningTeamAnnouncement;
        quitGameButton.interactable = true;
        startGameButton.interactable = true;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(startNetworkCountdown);
            //stream.SendNext(clockTime);
            //if (announceTeamWin)
            //{
            //    stream.SendNext(announcementCode);
            //    stream.SendNext(winningTeamAnnouncement);
            //    announceTeamWin = false;
            //}
        }
        else
        {
            stream.SendNext(startNetworkCountdown);
            //clockTime = (float) stream.ReceiveNext();
            //if((string)stream.PeekNext() == announcementCode)
            //{
            //    winningTeamAnnouncement = (string)stream.ReceiveNext();
            //    AnnounceWin();
            //}
        }
    }
}
