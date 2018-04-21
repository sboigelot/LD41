using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    class PlayAllAnim : MonoBehaviour
    {
        public void Update()
        {
            var anim = GetComponent<Animator>();
            anim.Play("Default Take");
        }
    }
}
