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
        public float OverideTileHeigth = -1;

        public GameObject Visual;

        public void Build(Level level)
        {
            if (Visual != null)
            {
                Destroy(Visual);
            }

            var prefab = PrefabManager.Instance.TilePrefabs[0];
            if (!string.IsNullOrEmpty(level.TileString))
            {
                var id = OverideTileId != -1 ? OverideTileId : level.Tiles[X][Z];
                level.Tiles[X][Z] = id;
                OverideTileId = id;

                prefab = PrefabManager.Instance.TilePrefabs[id];
            }

            float height = 0;//MapRenderer.Instance.GetHeight(X,Z) - .3f;
            if (!string.IsNullOrEmpty(level.HeightmapString))
            {
                height = OverideTileHeigth != -1 ? OverideTileHeigth : level.Heightmap[X][Z];
                level.Heightmap[X][Z] = height;
                OverideTileHeigth = height;

                if (height < 0.1f)
                {
                    return;
                }
                height -= 1.3f;
                height += UnityEngine.Random.Range(-0.1f, +0.1f);
            }

            Visual = GameObject.Instantiate(prefab, 
                new Vector3(gameObject.transform.position.x, height, gameObject.transform.position.z), 
                Quaternion.identity,
                gameObject.transform);
        }
    }
}
