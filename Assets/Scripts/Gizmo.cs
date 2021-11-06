using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo : MonoBehaviour
{
    public Color Color = Color.magenta;
    public Vector3 Size = new Vector3(.25f, .25f, .0f);
    void OnDrawGizmos()
    {
        Gizmos.color = Color;
        Gizmos.DrawCube(transform.position, Size);
    }
}
