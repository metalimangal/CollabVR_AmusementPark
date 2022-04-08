using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureFlagScoreZone : MonoBehaviour
{
    [SerializeField]
    private Team belongsTo;
    // Start is called before the first frame update
    void Start()
    {
        var cubeRenderer = gameObject.GetComponentInChildren<Renderer>();
        if (belongsTo == Team.BLUE)
        {
            cubeRenderer.material.SetColor("_Color", Color.blue);
        }
        else if (belongsTo == Team.RED)
        {
            cubeRenderer.material.SetColor("_Color", Color.red);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        // If you get the enemy's flag into your zone, add the score
        if(other.tag == "Flag" && other.gameObject.GetComponent<CaptureFlagFlag>().flagBelongsTo != belongsTo)
        {
            CaptureFlagScoreManager.Instance.AddScore(belongsTo, 1);
            other.gameObject.GetComponent<CaptureFlagFlag>().ResetPosition();
        }
    }
}
