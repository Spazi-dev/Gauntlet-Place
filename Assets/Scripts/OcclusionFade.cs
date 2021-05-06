using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcclusionFade : MonoBehaviour
{
    [Header("Note: This script requires an object tagged with PlayerEffectTarget!")]
    [Space]
    GameObject playerEffectTarget;
    Transform fadePositionTarget;
    public Component[] meshRenderers;
/*     bool objectVisible = true;
    bool objectWasVisible = true; */
    float fadeSpeed = 3.5f;
    float transparency = 1f;
    float opaque = 1f, invisible = 0f;

    void Start()
    {
        if (playerEffectTarget == null)
        playerEffectTarget = GameObject.FindWithTag("PlayerEffectTarget");
        fadePositionTarget = playerEffectTarget.GetComponent<Transform>();

        meshRenderers = GetComponentsInChildren<MeshRenderer>();

        if (fadePositionTarget.position.z < transform.position.z) //Skip fading at start
        {
            transparency = opaque;
        }
            else
        {
            transparency = invisible;
        }
    }
    private void Update()
    {
        
        if (fadePositionTarget.position.z < transform.position.z)
        //if (fadePositionTarget.position.z < transform.position.z || Vector3.Distance(fadePositionTarget.position, transform.position) > 8f)
        {
            transparency = Mathf.MoveTowards(transparency, opaque, fadeSpeed * Time.deltaTime);
        }
            else
        {
            transparency = Mathf.MoveTowards(transparency, invisible, fadeSpeed * Time.deltaTime);
        }
        SetTransparency(); //Transparency is being set on every frame on every mesh. Refactor so it's only set when bool objectVisible changes?
        //objectVisible = false;
    }

    private void SetTransparency()
    {
        foreach (MeshRenderer rend in meshRenderers)
            rend.material.SetFloat("Float_DitherTransparency", transparency);
    }
}
