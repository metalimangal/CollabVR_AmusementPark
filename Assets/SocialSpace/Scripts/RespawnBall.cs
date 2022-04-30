using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class RespawnBall : MonoBehaviour
{
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
        if (other.tag == "BasketBall")
        {
            audEffect.PlayOneShot(audEffect.clip);
            other.transform.position = transform.position + new Vector3(0.0f, -0.5f, 0.0f);
            PhotonView pv = other.gameObject.GetComponent<PhotonView>();
            Debug.Log(pv.OwnerActorNr + " : Get Score");

            spawnManager.GetComponent<SpawnManagerSocial>().usrInfo.addCoin();
        }
    }
}
