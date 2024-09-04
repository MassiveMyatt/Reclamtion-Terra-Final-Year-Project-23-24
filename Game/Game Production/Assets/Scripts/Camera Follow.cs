using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3f;
    public Vector3 ofset;
    public Vector3 velocity = Vector3.zero;


    /// <summary>
    /// Update method is called once every frame. The method is used to make sure the camera follows the player
    /// wherever they move in the scene.
    /// </summary>
    void Update()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + ofset;

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);  
        }   
    }
}
