using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    public Transform rollingSphere;
    public float yOffset = 1f;

    // Update is called once per frame
    void Update()
    {
        if (rollingSphere != null) {
            Vector3 sphereTopPosition = rollingSphere.position;
            sphereTopPosition.y += yOffset;
            transform.position = sphereTopPosition;

            transform.rotation = Quaternion.Euler(0,0,0);
        }
        
    }
}
