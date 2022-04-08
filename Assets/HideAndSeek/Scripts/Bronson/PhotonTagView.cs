using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class PhotonTagView : MonoBehaviourPunCallbacks, IPunObservable
{
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this.gameObject.tag);   //Send the tag over the network
        }
        else
        {
            this.gameObject.tag = (string)stream.ReceiveNext(); //Receive the tag over the network
        }
    }

}
