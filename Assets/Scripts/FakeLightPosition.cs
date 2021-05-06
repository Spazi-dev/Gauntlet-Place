using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FakeLightPosition : MonoBehaviour 
{
    [SerializeField]
    bool disableShadow = false;

    #region single instance check, only one instance should set globals
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
        float shadowScale = disableShadow ? 1f : 0f;
        Shader.SetGlobalFloat("Float_DisableShadow", shadowScale);
    }
}