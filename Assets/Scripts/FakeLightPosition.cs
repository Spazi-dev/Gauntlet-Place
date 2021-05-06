using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FakeLightPosition : MonoBehaviour 
{
    [Header("Note: This script requires an object tagged with PlayerEffectTarget!")]
    [Space]

    [SerializeField]
    bool disableShadow = false;
    //[SerializeField]
    GameObject playerEffectTarget;
    Transform lightPositionTarget;

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
    private void Start()
    {
        if (playerEffectTarget == null)
        playerEffectTarget = GameObject.FindWithTag("PlayerEffectTarget");
        lightPositionTarget = playerEffectTarget.GetComponent<Transform>();
    }
    void Update()
    {
        Shader.SetGlobalVector("Vector3_LightPosition", lightPositionTarget.position);
        float shadowScale = disableShadow ? 1f : 0f; //This doesn't need to be checked in final build, if shadows are always meant to be enabled
        Shader.SetGlobalFloat("Float_DisableShadow", shadowScale); //...nor this set every frame
    }
}