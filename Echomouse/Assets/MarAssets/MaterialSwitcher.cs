using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwitcher : MonoBehaviour
{
    [SerializeField] private Material darkMaterial;

    private void Start()
    {
        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer mesh in meshes)
        {
            mesh.material = darkMaterial;
        }
    }
}
