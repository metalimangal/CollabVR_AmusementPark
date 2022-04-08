using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureFlagSpawnArea : MonoBehaviour
{
    public Team teamSpawn;
    public Vector3 size;
    private Vector3 center;

    private void Awake()
    {
        center = gameObject.transform.position;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.4f);
        Gizmos.DrawCube(gameObject.transform.position, size);
    }
}
