using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CF_GroundDamage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        if(collision.transform.root.TryGetComponent(out CF_Player player))
        {
            StartCoroutine(DamagePlayer(player));
        }
        else if (collision.transform.root.TryGetComponent(out CF_Flag flag))
        {
            flag.ResetPosition();
        }
        
    }

    IEnumerator DamagePlayer(CF_Player player)
    {
        player.TakeDamage(50, "environment", out bool killed);
        yield return new WaitForSeconds(2);
        player.TakeDamage(50, "environment", out bool killed2);
    }
}
