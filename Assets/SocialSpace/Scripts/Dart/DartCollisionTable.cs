using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DartCollisionTable : MonoBehaviour
{
    public int scoreValue;
    private AudioSource audEffect; 
    public GameObject spawnManager;


    // Start is called before the first frame update
    void Start()
    {
        audEffect = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Dart")
        {
            audEffect.PlayOneShot(audEffect.clip);
            PhotonView pv = other.gameObject.GetComponent<PhotonView>();
            Debug.Log(pv.OwnerActorNr + " : Get Score " + scoreValue);
            Destroy(other.gameObject, 10);

            spawnManager.GetComponent<SpawnManagerSocial>().usrInfo.addCoin();
        }
    }
}
