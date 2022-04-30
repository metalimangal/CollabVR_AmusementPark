/*
    This script is placed on the Main Camera and controls the cross hair hit detection.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayCaster : MonoBehaviour {
    public Text output;
    public GameObject crosshair;
    public ParticleController particleController;

    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot;

    private Camera mainCamera = null;

    void Start()
    {
        mainCamera = Camera.main;
        hotSpot = new Vector2(cursorTexture.width * 0.5f, cursorTexture.height * 0.5f);
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }
    // Update is called once per frame 
    void Update ()
    {
        if(mainCamera == null)
        {
            Debug.LogError("RayCaster script was unable to get a camera from Camera.main");
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 500) == true)
        {
            // update crosshair
            Vector3 newPos = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.transform.position.z);
            output.text = hitInfo.transform.gameObject.name + "- " + newPos.x + " ," + newPos.y;
            // check for hits
            if (Input.GetAxis("Fire1") > 0.0f)
            {
                // hit box
                if (hitInfo.transform.gameObject.tag == "ExplodingBox")
                {
                    particleController.PlayExplosion(hitInfo.transform.position);
                    // destroy box, could just disable/hide box instead for reuse later
                    Destroy(hitInfo.transform.gameObject);
                }
                // hit balloon
                if (hitInfo.transform.gameObject.tag == "Balloon")
                {
                    particleController.PlayBalloonPop(hitInfo.transform.position);
                    particleController.PlayWaterDrop(hitInfo.transform.position);
                    // enable cloth on balloon so it droops
                    hitInfo.transform.gameObject.GetComponent<Cloth>().enabled = true;
                }
            }
        }
    }
}
