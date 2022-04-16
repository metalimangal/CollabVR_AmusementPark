using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(ParticleSystem))]
public class WeaponDescriptor : MonoBehaviourPunCallbacks, IPunObservable
{
    public float damagePerHit = 1.0f;
    [System.NonSerialized] public bool fire = false;

    private ParticleSystem bullets;
    private GameObject owningPlayer;
    private bool canFire = true;
    private float lastFired = 0.0f;

    void Start()
    {
        bullets = this.GetComponent<ParticleSystem>();
        owningPlayer = GetOwningPlayer(this.gameObject);
        if (owningPlayer.GetComponent<HideAndSeekPlayer>().isLocalPlayer)
        {
            //Enable inputs if the owning player is the local player
        }
    }

    void Update()
    {
        var emission = bullets.emission;
        if (canFire && fire)
        {
            emission.enabled = fire;
        }
        else
        {
            emission.enabled = false;
        }
    }

    private GameObject GetOwningPlayer(GameObject obj)
    {
        GameObject player;
        if(obj.TryGetComponent<HideAndSeekPlayer>(out HideAndSeekPlayer HaSP))
        {
            player = obj;
        }
        else
        {
            player = GetOwningPlayer(obj.transform.parent.gameObject);
        }
        return player;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(fire);
        }
        else
        {
            fire = (bool)stream.ReceiveNext();
        }
    }
}