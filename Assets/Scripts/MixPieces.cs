using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixPieces : MonoBehaviour
{
    public GameObject[] pieces;


    // Start is called before the first frame update
    public void RandomPieces()
    {
        for (int i = 0; i < pieces.Length; i++)
        {
            pieces[i].transform.position = new Vector3(Random.Range(-1, 1), Random.Range(1, 3), -0.3f);
            pieces[i].transform.rotation = Quaternion.Euler(Random.Range(45, 90), Random.Range(90, 180), Random.Range(90, 180));
        }
    }

    void Start()
    {
        RandomPieces();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            RandomPieces();
        }
    }
}
