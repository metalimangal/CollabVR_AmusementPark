using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    [Tooltip("Where to send the object(s) when the teleport action is called.  Can be reassigned in-game, if needed.")] public int teleportLocation = 0;
    [Tooltip("What objects to send to the target location.  Can be reassigned in-game, if needed.")] public List<Transform> objectsToTeleport;
    [SerializeField] public List<TeleportArea> teleportAreas;

    public void TeleportObjectsToArea()
    {
        TeleportArea area = teleportAreas[teleportLocation];    //Retrieve the current teleport destination
        float xSeparation = (area.maximumLocation.x - area.minimumLocation.x) / objectsToTeleport.Count;
        float ySeparation = (area.maximumLocation.y - area.minimumLocation.y) / objectsToTeleport.Count;
        float currentX = area.minimumLocation.x;
        float currentY = area.maximumLocation.y;
        foreach(Transform obj in objectsToTeleport){
            Vector3 newLoc = new Vector3(currentX, currentY, area.zLocation);
            obj.position = newLoc;
            obj.gameObject.SendMessage(area.teleportMessage);
            currentX += xSeparation;
            currentY += ySeparation;
        }
    }

    [System.Serializable]
    public class TeleportArea
    {
        [Tooltip("The minimum location in worldspace to send an object.")] public Vector2 minimumLocation = new Vector2(-1f,-1f);
        [Tooltip("The maximum location in worldspace to send an object.")] public Vector2 maximumLocation = new Vector2(1,1);
        [Tooltip("The height in world coordinates at which to send an object.")] public float zLocation = 0.1f;
        [Tooltip("Sends this message (if provided) to a teleported object after it reaches its destination.")] public string teleportMessage;
    }
}
