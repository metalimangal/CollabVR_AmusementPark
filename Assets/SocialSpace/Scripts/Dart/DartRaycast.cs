using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartRaycast : MonoBehaviour
{
    private bool hasHit = false;
    private Rigidbody rb;
    //private Grabbable gb;
    public float throwForce = 10;
    private bool freeze = false;
    private DartInteractor interactor;
    private bool onTarget = false;

    private Quaternion initialRotation;

    // Start is called before the first frame update
    void Start()
    {
        var Dart = this.transform.parent;
        initialRotation = Dart.transform.rotation;
        rb = Dart.transform.GetComponent<Rigidbody>();
        interactor = Dart.transform.GetComponent<DartInteractor>();
        //gb = Dart.transform.GetComponent<OVRGrabbable>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!freeze)
        {
            this.transform.parent.rotation = initialRotation;

            if (!hasHit && !interactor.isGrabbed) shootRaycast();

            if (hasHit && !interactor.isGrabbed)
            {
                rb.isKinematic = true;
                rb.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
                rb.useGravity = false;
                freeze = true;
                if (!onTarget)
                {
                    Destroy(this.transform.parent.gameObject, 5);
                }
                interactor.desactivateInteractable();
            }

            if (interactor.isRelease && rb.velocity.magnitude > 1.0f)
            {
                rb.velocity += rb.velocity * 0.05f;

            }
        }
    }

    private void shootRaycast()
    {
        hasHit = Physics.Raycast(this.transform.position, transform.forward, out RaycastHit hit, 0.05f);

        Debug.DrawRay(transform.position, transform.forward, Color.green);

        if (hit.collider != null)
        {
            hasHit = hit.collider != null && !hit.collider.name.Contains("Tip Dart") && !hit.collider.name.Contains("Collision");

            Debug.Log(hit.collider.name);

            if (hit.collider.name == "Dart Table")
            {
                onTarget = true;
            }
        }
    }
}
