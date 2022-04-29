using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    [Tooltip("Where to send the object(s) when the teleport action is called.  Can be reassigned in-game, if needed.")] public int teleportLocation = 0;
    [Tooltip("What objects to send to the target location.  Can be reassigned in-game, if needed.")] public List<Transform> objectsToTeleport;
    [SerializeField] public List<TeleportArea> teleportAreas;

    private void TeleportObjectsToArea()
    {
        Debug.Log("Teleport called");
        Debug.Log("Objects to teleport: " + objectsToTeleport.Count.ToString());
        TeleportArea area = teleportAreas[teleportLocation];    //Retrieve the current teleport destination
        float xSeparation = (area.maximumLocation.x - area.minimumLocation.x) / area.separationDistance;
        float ySeparation = (area.maximumLocation.y - area.minimumLocation.y) / area.separationDistance;
        float currentX = area.minimumLocation.x;
        float currentY = area.maximumLocation.y;
        foreach(Transform obj in objectsToTeleport){
            Vector3 newLoc = new Vector3(currentX, area.zLocation, currentY);
            obj.position = newLoc;
            obj.gameObject.SendMessage(area.teleportMessage);
            currentX += xSeparation;
            if(currentX > area.maximumLocation.x)
            {
                currentX = area.minimumLocation.x;
                currentY += ySeparation;
                if(currentY > area.maximumLocation.y)
                {
                    Debug.LogWarning("Object teleported outside of the specified area.  Please reduce the separation distance or define a larger area.");
                }
            }
        }
    }

    [System.Serializable]
    public class TeleportArea
    {
        [Tooltip("The minimum location in worldspace to send an object.")] public Vector2 minimumLocation = new Vector2(-1f,-1f);
        [Tooltip("The maximum location in worldspace to send an object.")] public Vector2 maximumLocation = new Vector2(1,1);
        [Tooltip("The height in world coordinates at which to send an object.")] public float zLocation = 0.1f;
        [Tooltip("The distance to separate the teleported objects.")] public float separationDistance = 1.0f;
        [Tooltip("Sends this message (if provided) to a teleported object after it reaches its destination.")] public string teleportMessage;
    }
}
