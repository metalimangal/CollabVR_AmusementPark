using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR;

[RequireComponent(typeof(ParticleSystem))]
public class WeaponDescriptor : MonoBehaviourPunCallbacks, IPunObservable
{
    public float damagePerHit = 1.0f;
    [Tooltip("Determines which trigger to listen to, if attached to the local player.")] public bool leftHanded = false;
    public bool hapticsEnabled = true;
    [System.NonSerialized] public bool fire = false;

    private InputDevice controller;
    private ParticleSystem bullets;
    public GameObject owningPlayer;
    private bool localControl;
    public bool canFire = true;
    private float lastFired = 0.0f;

    void Start()
    {
        bullets = this.GetComponent<ParticleSystem>();
        //owningPlayer = GetOwningPlayer(this.gameObject);
        localControl = owningPlayer.GetComponent<HideAndSeekPlayer>().isLocalPlayer;
        if (localControl)
        {
            if (leftHanded)
            {
                var leftHandDevices = new List<UnityEngine.XR.InputDevice>();
                UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, leftHandDevices);
                if(leftHandDevices.Count == 1)
                {
                    controller = leftHandDevices[0];
                }
                else
                {
                    if(leftHandDevices.Count == 0)
                    {
                        Debug.LogError("No left hand devices found.",this);
                    }
                    else
                    {
                        Debug.LogError("More than one left hand device found.",this);
                    }
                }
            }
            else
            {
                var rightHandDevices = new List<UnityEngine.XR.InputDevice>();
                UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, rightHandDevices);
                if (rightHandDevices.Count == 1)
                {
                    controller = rightHandDevices[0];
                }
                else
                {
                    if (rightHandDevices.Count == 0)
                    {
                        Debug.LogError("No right hand devices found.", this);
                    }
                    else
                    {
                        Debug.LogError("right than one left hand device found.", this);
                    }
                }
            }
        }
    }

    void Update()
    {
        if (!localControl)
        {
            canFire = true;
        }
        if (canFire && localControl)    //Don't bother checking if firing is disabled
        {
            bool triggerValue;
            if (controller.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValue) && triggerValue)
            {
                if (hapticsEnabled)
                {
                    controller.SendHapticImpulse(0, 0.01f); //Vibrate the controller, if enabled
                }
                fire = true;
            }
            else
            {
                fire = false;
            }
        }
        
        if(!canFire)
        {
            fire = false;
        }

        var emission = bullets.emission;

        emission.enabled = fire;
    }

    //private GameObject GetOwningPlayer(GameObject obj)
    //{
    //    GameObject player;
    //    if(obj.TryGetComponent<HideAndSeekPlayer>(out HideAndSeekPlayer HaSP))
    //    {
    //        player = obj;
    //    }
    //    else
    //    {
    //        player = GetOwningPlayer(obj.transform.parent.gameObject);
    //    }
    //    return player;
    //}

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            int networkFire = 0;
            if (fire)
            {
                networkFire = 1;
            }
            stream.SendNext(networkFire);
        }
        else
        {
            if((int)stream.ReceiveNext() == 1)
            {
                fire = true;
            }
            else
            {
                fire = false;
            }
        }
    }
}