using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Models;
using Assets.Scripts.Serialization;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    [Serializable]
    public class PrototypeManager : Singleton<PrototypeManager>
    {
        public List<ProtoypeIndex> Index;

        public Dictionary<string, Prototype> Prototypes;

        public bool Loaded = false;

        private void RegisterPrototype(string uri, Prototype prototype)
        {
            if (Prototypes == null)
            {
                Prototypes = new Dictionary<string, Prototype>();
            }

            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentNullException("uri");
            }

            if (prototype == null)
            {
                throw new ArgumentNullException("prototype");
            }

            if (Prototypes.ContainsKey(uri))
            {
            }

            Prototypes[uri] = prototype;
        }

        public IEnumerator LoadPrototypes(Action onLoadCompleted)
        {
            Index = new List<ProtoypeIndex>();
            var sub = Load<List<ProtoypeIndex>, ProtoypeIndex>(Index, "Index.xml");
            foreach (var s in sub)
            {
                yield return s;
            }

            foreach (var protoypeIndex in Index)
            {
                var index = protoypeIndex;

                //Simple switch for now to gain time on LD
                switch (index.ProtoypeType)
                {
                    case "level":
                        sub = Load<Level>(protoypeIndex.Path, (data) => RegisterPrototype(index.Uri, data));
                        break;
                    default:
                        Debug.LogErrorFormat("PrototypeType not registered: {0}", index.ProtoypeType);
                        break;
                }
            }

            Loaded = true;
            if (onLoadCompleted != null)
            {
                onLoadCompleted();
            }
            yield break;
        }

        private IEnumerable Load<T, TI>(T store, string fileName) where T : class, IList<TI>, new() where TI : class, new()
        {
            var sub = DataSerializer.Instance.LoadFromStreamingAssets<T, TI>(store, "Data", fileName);
            foreach (var s in sub)
            {
                yield return s;
            }
        }

        private IEnumerable Load<T>(string fileName, Action<T> store)  where T : class, new()
        {
            var sub = DataSerializer.Instance.LoadFromStreamingAssets<T>("Data", fileName, store);
            foreach (var s in sub)
            {
                yield return s;
            }
        }
    }
}
