using Assets.Scripts.Controllers.Game;
using Assets.Scripts.Managers;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [CustomEditor(typeof(TileRenderer))]
    public class TileRendererInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            TileRenderer myScript = (TileRenderer)target;
            if (GUILayout.Button("Re-Render"))
            {
                myScript.Build(GameManager.Instance.Game.CurrentLevel);
            }
            if (GUILayout.Button("Copy Coodinates"))
            {
                TextEditor te = new TextEditor {text = string.Format("X=\"{0}\" Z=\"{1}\"", myScript.X, myScript.Z)};
                te.SelectAll();
                te.Copy();
            }

            GUILayout.Label("Heigth");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("h++"))
            {
                myScript.OverideTileHeigth++;
                myScript.Build(GameManager.Instance.Game.CurrentLevel);
            }
            if (GUILayout.Button("h--"))
            {
                myScript.OverideTileHeigth--;
                myScript.Build(GameManager.Instance.Game.CurrentLevel);
            }
            GUILayout.EndHorizontal();
            
            GUILayout.Label("Model");
            GUILayout.BeginVertical();
            var allTiles = PrefabManager.Instance.TilePrefabs;
            for (var index = 0; index < allTiles.Count; index++)
            {
                var tile = allTiles[index];
                if (GUILayout.Button(tile.name))
                {
                    myScript.OverideTileId = index;
                    myScript.Build(GameManager.Instance.Game.CurrentLevel);
                }
            }
            GUILayout.EndVertical();
        }
    }
}