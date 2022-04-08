using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CaptureFlagScoreManager : MonoBehaviour
{
    public static CaptureFlagScoreManager Instance { get; private set; }

    private int scoreBlue = 0;
    private int scoreRed = 0;

    public TextMeshProUGUI scoreBlueText;
    public TextMeshProUGUI scoreRedText;

    private void Awake()
    {
        if (Instance == null) { Instance = this; } else { Debug.Log("Warning: multiple " + this + " in scene!"); }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreBlueText.text = scoreBlue.ToString();
        scoreRedText.text = scoreRed.ToString();

    }

    public int GetScore(Team team)
    {
        if (team == Team.BLUE)
        {
            return scoreBlue;
        }
        else if (team == Team.RED)
        {
            return scoreRed;
        }
        else 
        {
            Debug.Log("Invalid Team");
            return 0;
        }
        
    }

    public void AddScore(Team team, int score)
    {
        if (team == Team.BLUE)
        {
            scoreBlue += score;
            Debug.Log("Blue Scored!");
        }
        else if (team == Team.RED)
        {
            scoreRed += score;
            Debug.Log("Red Scored!");
        }
        else Debug.Log("Invalid team");
    }


}
public enum Team
{
    BLUE, RED
}