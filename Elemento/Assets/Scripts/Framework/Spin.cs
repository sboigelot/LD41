using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Framework
{
    public class Spin : MonoBehaviour
    {
        public float Speed = 10f;

        void Update()
        {
            transform.Rotate(Vector3.up, Speed * Time.deltaTime);
        }
    }
}
