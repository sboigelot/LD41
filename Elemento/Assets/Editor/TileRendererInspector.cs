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
                myScript.Build(GameManager.Instance.Game.CurrentLevel, GameManager.Instance.Game.CurrentLevel.Tiles);
            }
        }
    }
}