using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Controllers.Framework
{
    public abstract class UiItemList<T> : MonoBehaviour
    {
        public GameObject ItemTemplate;

        public Transform ItemPanel;

        public void ReBuild()
        {
            var datas = GetData().ToList();

            for (var i = 0; i < ItemPanel.childCount; i++)
            {
                var child = ItemPanel.GetChild(i);
                var childHolder = child.GetComponent<UiItemListItem>();
                if (childHolder != null && datas.Contains((T)childHolder.Data))
                {
                    datas.Remove((T)childHolder.Data);
                }
                else
                {
                    Destroy(child.gameObject);
                }
            }

            foreach (T data in datas)
            {
                var itemObject = Instantiate(ItemTemplate, Vector3.zero, Quaternion.identity, ItemPanel);
                var itemHolder = itemObject.AddComponent<UiItemListItem>();
                itemHolder.Data = data;
                Prepare(itemObject, data);
            }
        }

        protected abstract List<T> GetData();

        protected abstract void Prepare(GameObject itemObject, T data);
    }
}
