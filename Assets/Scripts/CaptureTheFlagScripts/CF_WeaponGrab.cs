/*
 * 
 * 
 * This script inherits from xr grab interactable and contains
 * all of the gun functionalities (bad coding, sorry)
 * 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.InputSystem;
using System;

public class CF_WeaponGrab : XRGrabInteractable, IPunOwnershipCallbacks
{
    [Header("Gun Related Stuff")]
    public Team belongsTo = Team.NONE;
    public Transform shootTransform;
    public ParticleSystem ps;

    [Header("Gun Scriptable")]
    // public scriptable object reference
    public CF_GunScriptableObject gunData;

    // Gun Given Parameters
    private int ammoCount;
    private float fireRate;
    private float reloadTime;

    // Audio
    private AudioSource audioSource;
    private AudioClip shootAudio;
    private AudioClip emptyAudio;
    private AudioClip reloadAudio;


    // Gun Parameters
    [Header("Gun Parameters")]
    public int currentAmmo;
    public TextMeshProUGUI ammoText;
    public InputActionReference reloadReference;
    [SerializeField] private float reloadHapticAmplitude, reloadHapticDuration;

    private bool allowShoot = true;
    private bool isReloading = false;

    private PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();

        // Setting Gun parameters equals to value from scriptable object
        ammoCount = gunData.ammoCount;
        fireRate = gunData.fireRate;
        reloadTime = gunData.reloadTime;
        shootAudio = gunData.shootAudio;
        emptyAudio = gunData.emptyAudio;
        reloadAudio = gunData.reloadAudio;

        currentAmmo = ammoCount;
        ammoText.text = currentAmmo.ToString();
        reloadReference.action.performed += OnReload;
        
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    [PunRPC]
    void Shoot()
    {
        // Ammo Stuff
        if (currentAmmo > 0) { currentAmmo -= 1; }
        ammoText.text = currentAmmo.ToString();

        ps.Play();
        audioSource.PlayOneShot(shootAudio);
        // Shooting Logic
        Ray ray = new Ray(shootTransform.position, shootTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (TryGetComponent(out CF_Player enemyPlayer))
            {
                Debug.Log(enemyPlayer.playerName);

                // if (enemyPlayer.team != belongsTo) { enemyPlayer.TakeDamage(30); }
                enemyPlayer.TakeDamage(30);
            }
        }
        
        // Delay
        StartCoroutine(FireDelay());
    }

    [PunRPC]
    void Reload()
    {
        allowShoot = false;
        isReloading = true;

        audioSource.PlayOneShot(reloadAudio);
        StartCoroutine(ReloadDelay());
    }

    private void OnReload(InputAction.CallbackContext obj)
    {
        if (view.IsMine && isSelected)
        {
            view.RPC("Reload", RpcTarget.All);
        }
    }

    IEnumerator FireDelay()
    {
        allowShoot = false;
        yield return new WaitForSeconds(1 / fireRate);
        allowShoot = true;
    }

    IEnumerator ReloadDelay()
    {
        allowShoot = false;
        isReloading = true;

        ammoText.text = "Reloading";
        ammoText.color = Color.yellow;
        ammoText.fontSize -= 1;

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = ammoCount;
        ammoText.text = currentAmmo.ToString();
        ammoText.color = Color.white;
        ammoText.fontSize += 1;
        allowShoot = true;
        isReloading = false;
    }

    protected override void OnActivated(ActivateEventArgs args)
    {
        if (view.IsMine)
        {
            if (allowShoot && currentAmmo > 0 && !isReloading)
            {
                view.RPC("Shoot", RpcTarget.All);
            }
            else if (currentAmmo == 0)
            {
                args.interactorObject.transform.GetComponent<ActionBasedController>().SendHapticImpulse(reloadHapticAmplitude, reloadHapticDuration);
                audioSource.PlayOneShot(emptyAudio);
                if (!isReloading)
                {
                    ammoText.text = "Reload";
                    ammoText.color = Color.red;
                }
            }
        }
        base.OnActivated(args);
    }



    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        view.RequestOwnership();
        base.OnHoverEntered(args);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        belongsTo = args.interactorObject.transform.parent.gameObject.GetComponentInParent<CF_PlayerMovement>().team;
        
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        belongsTo = Team.NONE;
        base.OnSelectExited(args);
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if (!isSelected)
        {
            targetView.TransferOwnership(requestingPlayer);
        }
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {

    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {

    }
}
