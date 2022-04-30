using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Clock : MonoBehaviour
{
    private float timeRemaining = -1.0f;
    private float timeTotal = 600.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient && timeRemaining > 0.0f)
        {
            float time = timeRemaining - Time.deltaTime;
            GetComponent<PhotonView>().RPC("SyncTime", RpcTarget.All, time);
        }

        Quaternion qHour = Quaternion.identity;
        float angleHour = (1.0f - timeRemaining / timeTotal) * 90.0f;
        qHour.eulerAngles = new Vector3(angleHour, 0, 0);
        transform.GetChild(0).rotation = qHour;

        Quaternion qMinute = Quaternion.identity;
        float angleMinute = (timeTotal - timeRemaining) % (timeTotal / 3.0f) / (timeTotal / 3.0f) * 360.0f;
        qMinute.eulerAngles = new Vector3(angleMinute, 0, 0);
        transform.GetChild(1).rotation = qMinute;
    }

    public float GetTime()
    {
        return timeRemaining;
    }

    public void SetTime(float time)
    {
        timeRemaining = time;
        timeTotal = time;
    }

    [PunRPC]
    public void SyncTime(float time)
    {
        timeRemaining = time;
    }
}
