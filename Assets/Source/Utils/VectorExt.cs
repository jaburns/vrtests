using UnityEngine;

static public class VectorExt
{
    static public Vector3 WithX(this Vector3 v, float x) { return new Vector3(x, v.y, v.z); }
    static public Vector3 WithY(this Vector3 v, float y) { return new Vector3(v.x, y, v.z); }
    static public Vector3 WithZ(this Vector3 v, float z) { return new Vector3(v.x, v.y, z); }

    static public Vector3 WithXY(this Vector3 v, float x, float y) { return new Vector3(x, y, v.z); }
    static public Vector3 WithYZ(this Vector3 v, float y, float z) { return new Vector3(v.x, y, z); }
    static public Vector3 WithXZ(this Vector3 v, float x, float z) { return new Vector3(x, v.y, z); }

    static public Vector3 WithXY(this Vector3 v, Vector3 o) { return new Vector3(o.x, o.y, v.z); }
    static public Vector3 WithYZ(this Vector3 v, Vector3 o) { return new Vector3(v.x, o.y, o.z); }
    static public Vector3 WithXZ(this Vector3 v, Vector3 o) { return new Vector3(o.x, v.y, o.z); }
}