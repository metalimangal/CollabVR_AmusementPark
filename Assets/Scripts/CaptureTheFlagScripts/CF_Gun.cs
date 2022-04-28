using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(AudioSource), typeof(CF_WeaponGrab), typeof(PhotonView))]
public class CF_Gun : MonoBehaviourPun
{
    [Header("Gun Parameters")]
    [SerializeField] private bool _friendlyFire = false;
    [SerializeField] private bool _canHurt = true;
    private string ownerName;
    private Team ownerTeam;


    [Header("References")]
    public Transform shootTransform;
    public ParticleSystem ps;
    public TextMeshProUGUI ammoText;
    public InputActionReference reloadReference;
    private CF_WeaponGrab _interactable;
    private CF_TwoHandGrab _interactable2;
    private bool isTwoHand;

    [Header("Gun Scriptable")]
    [SerializeField] private CF_GunScriptableObject _gunData;

    // Gun Given Parameters
    private int _ammoCount;
    private float _fireRate;
    private bool _isAutomatic;
    private float _reloadTime;
    private int _gunDamage;
    private int _currentAmmo;
    private float _reloadHapticAmplitude, _reloadHapticDuration;

    private float _lastFired;

    // Audio
    private AudioSource _audioSource;
    private AudioClip _shootAudio;
    private AudioClip _emptyAudio;
    private AudioClip _reloadAudio;

    // Firing Mechanisms
    private bool _allowShoot = true;
    private bool _isReloading = false;
    private bool _firing = false;

    void Start()
    {
        // Setting Gun Parameters
        _ammoCount = _gunData.ammoCount;
        _fireRate = _gunData.fireRate;
        _isAutomatic = _gunData.isAutomatic;
        _reloadTime = _gunData.reloadTime;
        _gunDamage = _gunData.damage;
        _shootAudio = _gunData.shootAudio;
        _emptyAudio = _gunData.emptyAudio;
        _reloadAudio = _gunData.reloadAudio;

        _currentAmmo = _ammoCount;
        ammoText.text = _currentAmmo.ToString();
        

        _audioSource = gameObject.GetComponent<AudioSource>();

        if (gameObject.TryGetComponent(out CF_WeaponGrab weaponGrab))
        {
            _interactable = weaponGrab;
            _interactable.activated.AddListener(OnActivate);
            _interactable.deactivated.AddListener(StoppedFiring);
            _interactable.selectEntered.AddListener(BindReload);
            _interactable.selectExited.AddListener(UnbindReload);
            isTwoHand = false;
        }
        else
        {
            _interactable2 = gameObject.GetComponent<CF_TwoHandGrab>();
            _interactable2.deactivated.AddListener(StoppedFiring);
            _interactable2.activated.AddListener(OnActivate);
            _interactable2.selectEntered.AddListener(BindReload);
            _interactable2.selectExited.AddListener(UnbindReload);
            isTwoHand = true;
        }
    }

    private void Update()
    {
        if (_isAutomatic)
        {
            if (_firing && !_isReloading && _currentAmmo > 0)
            {
                if (Time.time - _lastFired > 1 / _fireRate)
                {
                    _lastFired = Time.time;
                    ammoText.text = _currentAmmo.ToString();
                    photonView.RPC("Shoot", RpcTarget.All);
                    ShootRaycast();
                }
            }
        }
    }

    private void StoppedFiring(DeactivateEventArgs args)
    {
        if (_isAutomatic)
        {
            _firing = false;
        }
    }

    private void OnActivate(ActivateEventArgs args)
    {
        if (!_isAutomatic)
        {
            if (photonView.IsMine)
            {
                if (_allowShoot && _currentAmmo > 0 && !_isReloading)
                {
                    photonView.RPC("Shoot", RpcTarget.All);
                    ShootRaycast();
                }
                else if (_currentAmmo == 0)
                {
                    args.interactorObject.transform.GetComponent<ActionBasedController>().SendHapticImpulse(_reloadHapticAmplitude, _reloadHapticDuration);
                    _audioSource.PlayOneShot(_emptyAudio);
                    if (!_isReloading)
                    {
                        ammoText.text = "Reload";
                        ammoText.color = Color.red;
                    }
                }
            }
        }
        else
        {
            _firing = true;

            if (_currentAmmo == 0)
            {
                _firing = false;
                args.interactorObject.transform.GetComponent<ActionBasedController>().SendHapticImpulse(_reloadHapticAmplitude, _reloadHapticDuration);
                _audioSource.PlayOneShot(_emptyAudio);
                if (!_isReloading)
                {
                    ammoText.text = "Reload";
                    ammoText.color = Color.red;
                }
            }
        }
    }

    IEnumerator SingleFireDelay()
    {
        _allowShoot = false;
        yield return new WaitForSeconds(1 / _fireRate);
        _allowShoot = true;
    }

    [PunRPC]
    private void Shoot() //this is for audio and effects only
    {

        if (_currentAmmo > 0) { _currentAmmo -= 1; }
        ammoText.text = _currentAmmo.ToString();

        ps.Play();
        _audioSource.PlayOneShot(_shootAudio);
    }

    private void ShootRaycast()
    {
        bool enemyKilled = false;
        Ray ray = new Ray(shootTransform.position, shootTransform.forward);
        if (_canHurt && Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (hit.transform.root.TryGetComponent(out CF_Player enemyPlayer))
            {
                if (!_friendlyFire)
                {
                    if (enemyPlayer.team != ownerTeam) { enemyPlayer.TakeDamage(_gunDamage, ownerName, out enemyKilled); }
                }
                else
                {
                    enemyPlayer.TakeDamage(_gunDamage, ownerName, out enemyKilled);
                }

                if (enemyKilled)
                {
                    Debug.Log("You killed " + enemyPlayer.playerName);
                }
            }
        }
        if (!_isAutomatic) StartCoroutine(SingleFireDelay());
    }

    private void OnReload(InputAction.CallbackContext ctx)
    {
        if (isTwoHand)
        {
            if (_interactable2.isSelected && photonView.IsMine)
            {
                photonView.RPC("Reload", RpcTarget.All);
            }
        }
        else
        {
            if (_interactable.isSelected && photonView.IsMine)
            {
                photonView.RPC("Reload", RpcTarget.All);
            }
        }
        
    }

    private void BindReload(SelectEnterEventArgs args)
    {
        reloadReference.action.performed += OnReload;
        if (isTwoHand)
        {
            ownerName = _interactable2.ownerName;
            ownerTeam = _interactable2.belongsTo;
        }
        else
        {
            ownerName = _interactable.ownerName;
            ownerTeam = _interactable.belongsTo;
        }
    }

    private void UnbindReload(SelectExitEventArgs args)
    {
        reloadReference.action.performed -= OnReload;
        ownerName = "";
        ownerTeam = Team.NONE;
    }

    [PunRPC]
    private void Reload()
    {
        _allowShoot = false;
        _isReloading = true;
        _audioSource.PlayOneShot(_reloadAudio);
        StartCoroutine(ReloadDelay());
    }

    IEnumerator ReloadDelay()
    {
        _allowShoot = false;
        _isReloading = true;

        ammoText.text = "Reloading";
        ammoText.color = Color.yellow;
        ammoText.fontSize -= 1;

        yield return new WaitForSeconds(_reloadTime);

        _currentAmmo = _ammoCount;
        ammoText.text = _currentAmmo.ToString();
        ammoText.color = Color.white;
        ammoText.fontSize += 1;
        _allowShoot = true;
        _isReloading = false;
    }

}
