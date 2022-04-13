using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentObjectState : MonoBehaviour
{
    public string name;
    public Text txt;

    private List<string> handsName = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        handsName.Add("coll_hands:b_l_hand");
        handsName.Add("coll_hands:b_r_hand");
        handsName.Add("GrabVolumeSmall");
        handsName.Add("GrabVolumeBig");

    }

    public void OnTriggerEnter(Collider other)
    {
        if (handsName.Contains(other.gameObject.name))
        {
            txt.GetComponent<UnityEngine.UI.Text>().text = "Selected object: " + name;
        }

        Debug.Log(other.gameObject.name);
    }

    public void OnTriggerExit(Collider other)
    {
        if (handsName.Contains(other.gameObject.name))
        {
            txt.GetComponent<UnityEngine.UI.Text>().text = " ";
        }
    }
}
