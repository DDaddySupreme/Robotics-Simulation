using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    [Range(-2 * Mathf.PI, 2 * Mathf.PI)]
    public float theta = 0; // Angle about previous z, from old x to new x
    [Range(0.0f, 10.0f)]
    public float distance = 0; // Offset along previous z to common normal
    [Range(-5.0f, 5.0f)]
    public float radius = 0; // Length of the common normal. Also called a
    [Range(-2 * Mathf.PI, 2 * Mathf.PI)]
    public float alpha = 0; // Angle about common normal, from old z axis to new z axis

    public bool revolute;

    public Link prev = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    

    // Update is called once per frame
    void Update()
    {

        Matrix4x4 newMatrix = GetCoordinateSystem();

        transform.position = newMatrix.GetColumn(3);
        float w = Mathf.Sqrt(1.0f + newMatrix.m00 + newMatrix.m11 + newMatrix.m22) / 2.0f;
        transform.rotation = new Quaternion((newMatrix.m21 - newMatrix.m12) / (4.0f * w), (newMatrix.m02 - newMatrix.m20) / (4.0f * w), (newMatrix.m10 - newMatrix.m01) / (4.0f * w), w);

    }

    Matrix4x4 GetCoordinateSystem()
    {
        Matrix4x4 lhs = Matrix4x4.identity;
        Matrix4x4 rhs = Matrix4x4.identity;

        lhs.SetColumn(0, new Vector4(Mathf.Cos(theta), Mathf.Sin(theta), 0, 0));
        lhs.SetColumn(1, new Vector4(-Mathf.Sin(theta), Mathf.Cos(theta), 0, 0));
        // lhs column 2 = identity column 2
        lhs.SetColumn(3, new Vector4(0, 0, distance, 1));

        // rhs column 0 = identity column 0
        rhs.SetColumn(1, new Vector4(0, Mathf.Cos(alpha), Mathf.Sin(alpha), 0));
        rhs.SetColumn(2, new Vector4(0, -Mathf.Sin(alpha), Mathf.Cos(alpha), 0));
        rhs.SetColumn(3, new Vector4(radius, 0, 0, 1));

        Matrix4x4 prevMatrix = Matrix4x4.identity;
        if (prev != null)
        {
            prevMatrix = prev.GetCoordinateSystem();
        }

        return prevMatrix * lhs * rhs;
        
    }
}
