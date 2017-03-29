
using UnityEngine;
using System.Collections;
//实例化物体类
[System.Serializable]
public class InstantObj 
{
    public string Name;
    public string FatherName=null;
    public bool isResetTransform;
    public Vector3 localPos;
    public Vector3 LocalEuler;
    public string addComponentArray; 
}
