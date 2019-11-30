using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AutoRotator : MonoBehaviour
{
    public float DegreePerFrame;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float angle = DegreePerFrame * Mathf.Deg2Rad;

        Vector3 axis = new Vector3(0, 0, 1);
        transform.Rotate(axis, angle);
    }
}
