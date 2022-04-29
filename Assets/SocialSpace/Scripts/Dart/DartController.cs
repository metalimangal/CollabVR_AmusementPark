using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartController : MonoBehaviour
{
    public GameObject currentDart;

    private Vector3 dartPosition;
    private GameObject dart;

    // Start is called before the first frame update
    void Start()
    {
        dart = Resources.Load<GameObject>("Prefabs/Dart/Tip Dart");
        dartPosition = currentDart.transform.position;
        spawnDart();
    }

    void spawnDart()
    {
        currentDart = GameObject.Instantiate(dart, dartPosition, Quaternion.identity) as GameObject;
        //currentDart.transform.Rotate(0, 180, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(currentDart.transform.position, dartPosition) > 0.5)
        {
            spawnDart();
        }
    }
}
