using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    Vector3 gunPos = new (-0.295f, 0.302f, -0.017f);

    /// <summary>
    /// This method is called when the scene first loads and sets the position of the enemies gun to the correct position.
    /// </summary>
    private void Awake()
    {
        transform.localPosition = gunPos;
    }
}
