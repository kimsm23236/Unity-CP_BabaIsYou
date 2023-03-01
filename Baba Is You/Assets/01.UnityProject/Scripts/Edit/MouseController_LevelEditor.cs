using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class MouseController_LevelEditor : MonoBehaviour
{
    public static Ray ray;
    private GameObject actualObject_;
    // Start is called before the first frame update
    void Start()
    {
        actualObject_ = default;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
