/*
 * 
 * This script is used to manage the spawning of the different players within the game
 * 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CF_SpawnManager : MonoBehaviour
{
    public static CF_SpawnManager Instance { get; private set; }
    GameObject[] redTeamSpawns;
    GameObject[] blueTeamSpawns;
    private static int redSpawnCounter = 0;
    private static int blueSpawnCounter = 0;

    private void Awake()
    {
        if (Instance == null) { Instance = this; } else { Debug.Log("Warning: multiple " + this + " in scene!"); }
        redTeamSpawns = GameObject.FindGameObjectsWithTag("RedSpawn");
        blueTeamSpawns = GameObject.FindGameObjectsWithTag("BlueSpawn");
    }

    private Transform GetNextRedSpawn()
    {
        Transform t = redTeamSpawns[redSpawnCounter].transform;
        if (redSpawnCounter < redTeamSpawns.Length)
            redSpawnCounter += 1;
        else redSpawnCounter = 0;
        return t;
    }

    private Transform GetNextBlueSpawn()
    {
        Transform t = blueTeamSpawns[blueSpawnCounter].transform;
        if (blueSpawnCounter < blueTeamSpawns.Length)
            blueSpawnCounter += 1;
        else blueSpawnCounter = 0;
        return t;
    }

    public Transform GetSpawn(Team team)
    {
        if (team == Team.BLUE)
        {
            return GetNextBlueSpawn();
        }
        else if (team == Team.RED)
        {
            return GetNextRedSpawn();
        }
        else return gameObject.transform;
    }
}
