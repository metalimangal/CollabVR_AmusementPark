using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalPlayerUIManager : MonoBehaviour
{
    [SerializeField]
    GameObject GoHome_Button;
    void Start()
    {
        //GoHome_Button.GetComponent<Button>().onClick.AddListener(VirtualWorldManager.Instance.LeaveRoomAndLoadHomeScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
