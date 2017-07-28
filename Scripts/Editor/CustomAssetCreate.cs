using UnityEditor;
using UnityEngine;

public class CustomAssetCreate
{
    [MenuItem("Assets/Create/Custom Asset/Surface Data")]
    public static void CreateSurfaceData()
    {
        ScriptableObject asset = ScriptableObject.CreateInstance(typeof(SurfaceData));
        ProjectWindowUtil.CreateAsset(asset, "New SurfaceData.asset");
    }

    [MenuItem("Assets/Create/Custom Asset/Input Data")]
    public static void CreateInputData()
    {
        ScriptableObject asset = ScriptableObject.CreateInstance(typeof(InputData));
        ProjectWindowUtil.CreateAsset(asset, "New InputData.asset");
    }
}