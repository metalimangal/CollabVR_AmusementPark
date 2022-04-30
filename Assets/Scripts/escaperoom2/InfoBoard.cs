using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

using TMPro;

public class InfoBoard : MonoBehaviour
{
    private string boardInfo = "";
    private int foundCount = 0;

    public int voteCountMother = 0;
    public int voteCountGirlFriend = 0;
    public int voteCountClassmate = 0;
    public int voteCountDetective = 0;
    public int voteTotal = 0;

    public int decision = -1;

    // Start is called before the first frame update
    void Start()
    {
        boardInfo = GetComponent<TMP_Text>().text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateInfo(string info)
    {
        GetComponent<PhotonView>().RPC("SyncInfo", RpcTarget.All, info);
    }

    [PunRPC]
    public void SyncInfo(string info)
    {
        boardInfo += info;
        GetComponent<TMP_Text>().text = boardInfo;
        foundCount++;
    }

    public int GetFoundCount()
    {
        return foundCount;
    }

    public void VoteMother()
    {
        GetComponent<PhotonView>().RPC("SyncVoteMother", RpcTarget.All);
    }

    public void VoteGirlFriend()
    {
        GetComponent<PhotonView>().RPC("SyncVoteGirlFriend", RpcTarget.All);
    }

    public void VoteClassmate()
    {
        GetComponent<PhotonView>().RPC("SyncVoteClassmate", RpcTarget.All);
    }

    public void VoteDetective()
    {
        GetComponent<PhotonView>().RPC("SyncVoteDetective", RpcTarget.All);
    }

    public void Decide(int id)
    {
        GetComponent<PhotonView>().RPC("SyncDecide", RpcTarget.All, id);
    }

    [PunRPC]
    public void SyncVoteMother()
    {
        voteCountMother++;
        voteTotal++;
    }

    [PunRPC]
    public void SyncVoteGirlFriend()
    {
        voteCountGirlFriend++;
        voteTotal++;
    }

    [PunRPC]
    public void SyncVoteClassmate()
    {
        voteCountClassmate++;
        voteTotal++;
    }

    [PunRPC]
    public void SyncVoteDetective()
    {
        voteCountDetective++;
        voteTotal++;
    }

    [PunRPC]
    public void SyncDecide(int id)
    {
        decision = id;
    }
}
