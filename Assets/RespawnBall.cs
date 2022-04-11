using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class RespawnBall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BasketBall")
        {
            other.transform.position = transform.position + new Vector3(0.0f, -0.5f, 0.0f);
            PhotonView pv = other.gameObject.GetComponent<PhotonView>();
            Debug.Log(pv.OwnerActorNr + " : Get Score");
        }
    }
}
