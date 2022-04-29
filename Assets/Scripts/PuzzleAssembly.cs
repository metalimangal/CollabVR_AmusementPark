using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PuzzleAssembly : MonoBehaviour
{
    public GameObject targetSpot;
    private PhotonView photonView;
    public GameObject parent;
    private bool isAttached = false;


    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        float distPos = Vector3.Distance(transform.position, targetSpot.transform.position);

        if (!gameObject.GetComponent<OVRGrabbable>().isGrabbed)
        {
            //targetSpot.GetComponent<Renderer>().material.color = Color.magenta;
        }

        if (gameObject.GetComponent<OVRGrabbable>().isGrabbed)
        {
            //photonView.RPC("SetColor", RpcTarget.All);
            // targetSpot.GetComponent<Renderer>().material.color = Color.gray;
            // gameObject.GetComponent<Renderer>().material.color = Color.white;
        }

        if (distPos < 0.5)
        {
            transform.position = targetSpot.transform.position;
            transform.rotation = targetSpot.transform.rotation;
            if (!isAttached)
            {
                
                transform.parent = parent.transform;
                //targetSpot.SetActive(false);
                isAttached = true;
            }


            //targetSpot.SetActive(false);

        }
        else
        {
            //targetSpot.SetActive(true);
        }

    }

    [PunRPC]
    public void SetColor()
    {
        //targetSpot.GetComponent<Renderer>().material.color = Color.gray;
        //gameObject.GetComponent<Renderer>().material.color = Color.white;
    }
}
