using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Matrix4x4 m = Matrix4x4.identity;
{
    m.m00, m.m01, m.m02, m.m03
    m.m10, m.m11, m.m12, m.m13
    m.m20, m.m21, m.m22, m.m23
    m.m30, m.m31, m.m32, m.m33
}

// 위와 아래는 동일

int[,] matrix = new int[4, 4];
{
    { matrix[0, 0], matrix[0, 1], matrix[0, 2], matrix[0, 3]},
    { matrix[1, 0], matrix[1, 1], matrix[1, 2], matrix[1, 3]},
    { matrix[2, 0], matrix[2, 1], matrix[2, 2], matrix[2, 3]},
    { matrix[3, 0], matrix[3, 1], matrix[3, 2], matrix[3, 3]}
}
*/

public class Matrix : MonoBehaviour
{
    public static Matrix4x4 Translate(Vector3 position)
    {
        // ** 항등 행렬(단위 행렬)
        Matrix4x4 m = Matrix4x4.identity;  // 초기값(항등 행렬)으로 초기화

        m.m03 = position.x;
        m.m13 = position.y;
        m.m23 = position.z;

        return m;
    }

    public static Matrix4x4 Rotate(Vector3 _rotate)
    {
        var rad = _rotate * Mathf.Deg2Rad;

        return RotateX(rad.x) * RotateY(rad.y) * RotateZ(rad.z);
    }

    public static Matrix4x4 RotateX(float _x)
    {
        Matrix4x4 m = Matrix4x4.identity;

        //      X    Y    Z    W
        //  X   1    0    0    0
        //  Y   0   cos -sin   0
        //  Z   0   sin  cos   0
        //  W   0    0    0    1

        m.m11 = m.m22 = Mathf.Cos(_x);
        m.m21 = Mathf.Sin(_x);
        m.m12 = -m.m21;

        return m;
    }

    public static Matrix4x4 RotateY(float _y)
    {
        Matrix4x4 m = Matrix4x4.identity;

        //      X    Y    Z    W
        //  X  cos   0   sin   0
        //  Y   0    1    0    0
        //  Z -sin   0   cos   0
        //  W   0    0    0    1

        m.m00 = m.m22 = Mathf.Cos(_y);
        m.m02 = Mathf.Sin(_y);
        m.m20 = -m.m02;

        return m;
    }

    public static Matrix4x4 RotateZ(float _z)
    {
        Matrix4x4 m = Matrix4x4.identity;

        //      X    Y    Z    W
        //  X  cos -sin   0    0
        //  Y  sin  cos   0    0
        //  Z   0    0    1    0
        //  W   0    0    0    1

        m.m00 = m.m11 = Mathf.Cos(_z);
        m.m10 = Mathf.Sin(_z);
        m.m01 = -m.m10;

        return m;
    }

    public static Matrix4x4 Scale(Vector3 _scale)
    {
        Matrix4x4 m = Matrix4x4.identity;

        m.m00 = _scale.x;
        m.m11 = _scale.y;
        m.m22 = _scale.z;

        //      X    Y    Z    W
        //  X   sx   0    0    0
        //  Y   0    sy   0    0
        //  Z   0    0    sz   0
        //  W   0    0    0    1

        return m;
    }
}
