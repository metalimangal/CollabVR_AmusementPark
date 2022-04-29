using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class UpdateUserInfo : MonoBehaviour
{
    private string userName;
    private int coinNum;
    private InputDevice targetDevice;
    
    public InputDeviceCharacteristics controllerCharacteristics;
    public Canvas cv;
    public Text txtCoin;

    // Start is called before the first frame update
    void Start()
    {
        coinNum = 1;
        TryInitialize();
    }

    void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }
    }

    bool pressed = false;

    // Update is called once per frame
    void Update()
    {
        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out pressed);
        if (pressed)
        {
            cv.enabled = true;
        }
        else
        {
            cv.enabled = false;
        }

        infoUpdate();
    }

    public void infoUpdate()
    {
        txtCoin.text = coinNum.ToString() + " coins";
    }

    public void addCoin()
    {
        coinNum++;
    }

    public void subCoin()
    {
        coinNum--;
    }

    public int getCoin()
    {
        return coinNum;
    }
}
