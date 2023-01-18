using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(Texture3DGen))]

public class Texture3DGenerator : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Texture3DGen gen = (Texture3DGen)target;

        if(GUILayout.Button("Generate Texture"))
        {
            gen.CreateTexture3D();
        }
    }


    
}
