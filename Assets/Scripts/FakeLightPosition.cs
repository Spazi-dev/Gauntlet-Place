using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FakeLightPosition : MonoBehaviour 
{
    #region single instance check, only one instance can set global Vector3_LightPosition
    private static FakeLightPosition _instance;
    public static FakeLightPosition Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning("Only one FakeLightPosition should exist!", transform);
        } else {
            _instance = this;
        }
    }
    #endregion

    void Update()
    {
        Shader.SetGlobalVector("Vector3_LightPosition", transform.position);
    }
}