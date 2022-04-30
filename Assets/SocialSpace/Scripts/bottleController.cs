using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class bottleController : MonoBehaviour
{
    private XRInteractionManager xrManager;
    private XRGrabInteractable xrGrab;
    private SphereCollider sphColl;
    public GameObject spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        xrManager = GameObject.Find("XR Interaction Manager").GetComponent<XRInteractionManager>();
        xrGrab = this.GetComponent<XRGrabInteractable>();
        sphColl = this.GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void enableCol() { xrGrab.interactionManager = xrManager; }

    public void disableCol() { xrGrab.interactionManager = null; }

    public void isEnoughCoin()
    {
        if(spawnManager.GetComponent<SpawnManagerSocial>().usrInfo.getCoin() > 0) 
        {
            enableCol();
            spawnManager.GetComponent<SpawnManagerSocial>().usrInfo.subCoin();
        }

        else 
        {
            disableCol();
        }
    }
}
