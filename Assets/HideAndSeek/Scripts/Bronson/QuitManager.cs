using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Adapted from https://gamedevbeginner.com/how-to-quit-the-game-in-unity/

public class QuitManager : MonoBehaviour
{
    [SerializeField]
    private bool enableQuitting = false;
    public void QuitGame()
    {
        Quit();
    }

    private void Update()
    {
        if (Input.GetButtonDown("QuitButton") && enableQuitButton)
        {
            Quit();
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;    //Quit the game if it is running in the editor
#else
        Application.Quit(); //Quit the game if it is running as an application
#endif
    }
}
