using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartRaycast : MonoBehaviour
{
    private bool hasHit;
    private Rigidbody rb;
    //private Grabbable gb;
    public float throwForce = 10;
    private bool grabbed = false;
    private bool freeze = false;

    private Quaternion initialRotation;

    // Start is called before the first frame update
    void Start()
    {
        var Dart = this.transform.parent;
        initialRotation = Dart.transform.rotation;
        rb = Dart.transform.GetComponent<Rigidbody>();
        //gb = Dart.transform.GetComponent<OVRGrabbable>();

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.parent.rotation = initialRotation;

        //if (!hasHit && rb.velocity.magnitude == 0) shootRaycast();

        //if (hasHit && !gb.isGrabbed)
        //{
        //    rb.isKinematic = true;
        //    rb.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
        //    rb.useGravity = false;
        //}

        //if (!hasHit && grabbed && !gb.isGrabbed)
        //{
        //    grabbed = false;
        //    Debug.Log("Dart applied force: ");
        //    this.transform.parent.transform.GetComponent<ConstantForce>().enabled = true;
        //    rb.AddForce(new Vector3(0f, 0f, 1f) * OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch).magnitude, ForceMode.Impulse);
        //}

        //if (gb.isGrabbed)
        //{
        //    grabbed = true;
        //}

        //if (grabbed && !gb.isGrabbed)
        //    rb.constraints = RigidbodyConstraints.FreezePositionX;

        //if(rb.velocity.magnitude > 5 && !freeze)
        //{
        //    rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
        //    rb.velocity = rb.velocity * 2;
        //    freeze = true;
        //}
        //Debug.Log("Dart velocity: " + rb.velocity.magnitude);
    }

    private void shootRaycast()
    {
        hasHit = Physics.Raycast(this.transform.position, transform.forward, out RaycastHit hit, 0.1f);

        Debug.DrawRay(transform.position, transform.forward, Color.green);

        if (hit.collider != null)
            hasHit = hit.collider.name != "GrabVolumeSmall" && hit.collider.name != "GrabVolumeBig";

        if (hit.collider != null)
            Debug.Log(hit.collider.name);
    }
}
