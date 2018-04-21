using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Controllers.Game;
using UnityEditor;

namespace Assets.Editor
{
    [CustomEditor(typeof(MapRenderer))]
    public class MapRendererInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            MapRenderer myScript = (MapRenderer)target;
            if (GUILayout.Button("Re-Render"))
            {
                myScript.Build();
            }
        }
    }
}
