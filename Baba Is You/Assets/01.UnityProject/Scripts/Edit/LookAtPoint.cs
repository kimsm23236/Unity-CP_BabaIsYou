using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LookAtPoint : MonoBehaviour
{
    public Vector3 lookAtPoint = Vector3.zero;
    public Transform target = default;

    // Update is called once per frame
    void Update()
    {
        Look();
    }
    void Look()
    {
        if(target == null || target == default)
            return;

        transform.LookAt(target.position);
    }
}
