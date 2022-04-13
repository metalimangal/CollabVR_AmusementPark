using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerProperty : MonoBehaviour
{
    public TMP_Dropdown td;
    public int type;

    // Start is called before the first frame update
    void Start()
    {
        type = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setPlayerProperty()
    {
        type = td.value;
    }

    public void keepPlayerProperty()
    {
        DontDestroyOnLoad(this);
    }
}
