using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginUIManager : MonoBehaviour
{

    public GameObject ConnectOptionsPanelGameObject;
    public GameObject ConnectWithNamePanelGameObject;

    void Start()
    {
        ConnectOptionsPanelGameObject.SetActive(true);
        ConnectWithNamePanelGameObject.SetActive(false);
    }

}
