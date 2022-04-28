using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUserInfo : MonoBehaviour
{
    private string userName;
    private int coinNum;
    
    public Canvas cv;
    public Text txtCoin;

    // Start is called before the first frame update
    void Start()
    {
        coinNum = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("x"))
        {
            cv.enabled = !cv.enabled;
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
