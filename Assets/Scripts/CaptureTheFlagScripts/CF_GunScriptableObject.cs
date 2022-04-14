using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "ScriptableObjects/GunData", order = 1)]
public class CF_GunScriptableObject : ScriptableObject
{
    [Header("Gun Parameters")]
    [Tooltip("Ammo Count (rounds per mag)")]
    public int ammoCount;
    [Tooltip("Bullets per second")]
    public float fireRate;
    [Tooltip("Reload time (s)")]
    public float reloadTime;
    public int damage;

    [Header("Audio Clips")]
    public AudioClip shootAudio;
    public AudioClip emptyAudio;
    public AudioClip reloadAudio;

}
