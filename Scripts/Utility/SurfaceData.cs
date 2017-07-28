using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceData : ScriptableObject
{
    [SerializeField]
    private Surface defaultSurface;

    [SerializeField]
    private Surface[] surfaces;

    public Surface GetSurface(RaycastHit hitInfo)
    {
        return defaultSurface;
    }
}
