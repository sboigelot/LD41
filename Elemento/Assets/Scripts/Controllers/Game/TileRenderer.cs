using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Controllers.Game
{
    public class TileRenderer : MonoBehaviour
    {
        public int X;
        public int Z;
        public int TileId;
        public int OverideTileId = -1;
        
        public GameObject Visual;

        public void Build(Level level, int[][] tiles)
        {
            var prefab = PrefabManager.Instance.TilePrefabs[0];
            if (!string.IsNullOrEmpty(level.TileString))
            {
                Debug.Log("rendering tile: " + tiles[X][Z]);
                prefab = PrefabManager.Instance.TilePrefabs[OverideTileId != -1? OverideTileId: tiles[X][Z]];
            }

            if (Visual != null)
            {
                Destroy(Visual);
            }

            Visual = GameObject.Instantiate(prefab, 
                gameObject.transform.position, 
                Quaternion.identity,
                gameObject.transform);
        }
    }
}
