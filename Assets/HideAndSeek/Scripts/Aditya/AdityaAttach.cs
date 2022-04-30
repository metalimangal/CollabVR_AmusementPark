using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AdityaAttach : MonoBehaviour
{
    [SerializeField] private GameObject tablet;
    
    private GameObject _hitbox;
    private GameObject _tagger;

    private GameObject _localPlayer;
    private Transform _rightHand;
    private Transform _leftHand;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _localPlayer = GameObject.Find("LocalPlayer"); //placeholder, need to see avatar first
    }

    public void Attach()
    {
        _rightHand = _localPlayer.transform.Find("Right Hand"); //placeholder
        _leftHand = _localPlayer.transform.Find("Left Hand"); //placeholder

        Instantiate(tablet, _leftHand);
        
        _tagger = PhotonNetwork.Instantiate("Seeker_Weapon", _rightHand.localPosition, _rightHand.localRotation);
        _tagger.transform.parent = _rightHand;
        
        _hitbox = PhotonNetwork.Instantiate("Seeker_Weapon", _localPlayer.transform.localPosition, _localPlayer.transform.localRotation);
        _hitbox.transform.parent = _localPlayer.transform;
    }
}
