﻿using System;
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
            if (Visual != null)
            {
                Destroy(Visual);
            }

            var prefab = PrefabManager.Instance.TilePrefabs[0];
            if (!string.IsNullOrEmpty(level.TileString))
            {
                var id = OverideTileId != -1 ? OverideTileId : tiles[X][Z];
                prefab = PrefabManager.Instance.TilePrefabs[id];
            }

            float height = 0;//MapRenderer.Instance.GetHeight(X,Z) - .3f;
            if (!string.IsNullOrEmpty(level.HeightmapString))
            {
                height = level.Heightmap[X][Z];
                if (height < 0.1f)
                {
                    return;
                }
                height -= 1.3f;
            }

            Visual = GameObject.Instantiate(prefab, 
                new Vector3(gameObject.transform.position.x, height, gameObject.transform.position.z), 
                Quaternion.identity,
                gameObject.transform);
        }
    }
}
