using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Controllers.Game;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
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

            if (GUILayout.Button("Export Map"))
            {
                myScript.ExportMap();
                TextEditor te = new TextEditor {text = myScript.ExportMapString};
                te.SelectAll();
                te.Copy();
            }

            var level = GameManager.Instance.Game.CurrentLevel;
            if (level == null)
            {
                return;
            }

            var paths = level.MonsterPaths;

            foreach (var monsterPath in paths)
            {
                GUILayout.Label("MonsterPath[" + monsterPath.Id + "]");
                string checkpoints = "";
                foreach (var monsterCheckpoint in monsterPath.MonsterCheckpoints)
                {
                    GUILayout.BeginHorizontal();
                    int.TryParse(GUILayout.TextField("" + monsterCheckpoint.X), out monsterCheckpoint.X);
                    int.TryParse(GUILayout.TextField("" + monsterCheckpoint.Z), out monsterCheckpoint.Z);
                    GUILayout.EndHorizontal();
                    checkpoints += string.Format("<Checkpoint  X=\"{0}\" Z=\"{1}\"/>" + Environment.NewLine,
                        monsterCheckpoint.X, monsterCheckpoint.Z);
                }
                if (GUILayout.Button("Add checkpoint"))
                {
                    monsterPath.MonsterCheckpoints.Add(new MonsterCheckpoint());
                }
                if (GUILayout.Button("Export checkpoints"))
                {
                    TextEditor te = new TextEditor { text = checkpoints };
                    te.SelectAll();
                    te.Copy();
                }
            }

        }
    }
}
