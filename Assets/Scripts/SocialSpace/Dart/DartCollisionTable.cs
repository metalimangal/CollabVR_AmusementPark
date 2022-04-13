using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DartCollisionTable : MonoBehaviour
{
    public int scoreValue;

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
        if (other.tag == "Dart")
        {
            PhotonView pv = other.gameObject.GetComponent<PhotonView>();
            Debug.Log(pv.OwnerActorNr + " : Get Score " + scoreValue);
            Destroy(other.gameObject, 10);
        }
    }
}
