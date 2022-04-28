using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CF_HealthIndicator : MonoBehaviourPunCallbacks
{
    private float _health;
    public GameObject healthCylinder;
    public float minRotation = -109;
    public float maxRotation = 0;
    private Vector3 _currentRotation;
    public CF_Player _player;

    // Start is called before the first frame update
    void Start()
    {

    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        StartCoroutine(ScanPlayer());
    }

    // Update is called once per frame
    void Update()
    {
        if (_player != null)
        {
            _health = _player.health;
            _currentRotation = new Vector3(0, 0, Map(_health, 0, 100, minRotation, maxRotation));
            healthCylinder.transform.localEulerAngles = _currentRotation;
        }
    }

    float Map(float num, float numMin, float numMax, float outMin, float outMax)
    {
        float output;
        output = (num - numMin) * (outMax - outMin) / (numMax - numMin) + outMin;
        return output;
    }

    IEnumerator ScanPlayer()
    {
        while (_player == null)
        {
            yield return new WaitForSeconds(1);
            foreach (var p in FindObjectsOfType<CF_Player>())
            {
                if (p.photonView.IsMine)
                {
                    _player = p.GetComponent<CF_Player>();
                    break;
                }
            }
        }
    }
}
