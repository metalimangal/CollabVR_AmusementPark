using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class CF_PlayerMovement : MonoBehaviour
{
    XROrigin _xrOrigin;
    CapsuleCollider _collider;
    // Start is called before the first frame update
    void Start()
    {
        _xrOrigin = GetComponent<XROrigin>();
        _collider = GetComponentInChildren<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        _collider.height = Mathf.Clamp(
            _xrOrigin.CameraInOriginSpaceHeight,
            1f,
            2.5f);
        Vector3 center = _xrOrigin.CameraInOriginSpacePos;
        _collider.center = new Vector3(
            center.x,
            _collider.height / 2,
            center.z);
    }
}
