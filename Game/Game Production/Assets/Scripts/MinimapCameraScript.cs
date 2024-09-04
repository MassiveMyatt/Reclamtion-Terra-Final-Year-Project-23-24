using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MinimapCameraScript : MonoBehaviour
{
    public Transform target;
    /// <summary>
    /// This method keeps the minimap camera following the player at all times.
    /// </summary>
    void Update()
    {
        if (target != null)
        {
            Vector3 newPos = target.position;
            newPos.y = transform.position.y;
            transform.position = newPos;
        }
    }
}
