using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models;
using Assets.Scripts.Serialization;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    [Serializable]
    public class PrototypeManager : Singleton<PrototypeManager>
    {
        public List<ProtoypeIndex> Index;

        //public Dictionary<string, IPrototype> Prototypes;
        public List<LoadedPrototype> Prototypes;

        [Serializable]
        public class LoadedPrototype
        {
            public string Uri;
            public IPrototype Prototype;
        }
        
        public bool Loaded = false;

        private void RegisterPrototype(string uri, IPrototype prototype)
        {
            if (Prototypes == null)
            {
                //Prototypes = new Dictionary<string, IPrototype>();
                Prototypes = new List<LoadedPrototype>();
            }

            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentNullException("uri");
            }

            if (prototype == null)
            {
                throw new ArgumentNullException("prototype");
            }

            if (Prototypes.Any(lp => lp.Uri == uri))
            {
                Debug.LogWarning("Double assignement for prototype: " + uri);
            }
            
            Prototypes.Add(new LoadedPrototype
            {
                Uri = uri,
                Prototype = prototype
            });
        }

        public T GetPrototype<T>(string uri) where T : class, IPrototype
        {
            var loadedProto = Prototypes.FirstOrDefault(lp => lp.Uri == uri);
            if (loadedProto == null)
            {
                Debug.LogError("No prototype found for: " + uri + " (returning null)");
                return default(T);
            }

            var proto = loadedProto.Prototype as T;
            if(proto == null)
            {
                Debug.LogError("A prototype found for: " + uri + " but not of the type"+ typeof(T) +" (returning null)");
                return default(T);
            }

            return proto;
        }

        public List<T> GetAllPrototypes<T>()
        {
            return Prototypes.Where(p => p.Prototype is T).Select(p => (T) p.Prototype).ToList();
        }

        public IEnumerator LoadPrototypes(Action onLoadCompleted, Func<string, bool> filter = null)
        {
            if (filter == null)
            {
                filter = (type) => true;
            }

            Index = new List<ProtoypeIndex>();
            var sub = Load<List<ProtoypeIndex>, ProtoypeIndex>(Index, "Index.xml");
            foreach (var s in sub)
            {
                yield return s;
            }

            foreach (var protoypeIndex in Index.Where(i=>filter(i.ProtoypeType)))
            {
                var index = protoypeIndex;

                //Simple switch for now to gain time on LD
                switch (index.ProtoypeType)
                {
                    case "level":
                        sub = Load<Level>(protoypeIndex.Path, (data) =>
                        {
                            data.Uri = index.Uri;
                            RegisterPrototype(index.Uri, data);
                        });
                        break;
                    case "element":
                        sub = Load<ElementPrototype>(protoypeIndex.Path, (data) => RegisterPrototype(index.Uri, data));
                        break;
                    case "receipe":
                        sub = Load<ElementPrototypeReceipe>(protoypeIndex.Path, (data) => RegisterPrototype(index.Uri, data));
                        break;
                    case "monster":
                        sub = Load<MonsterPrototype>(protoypeIndex.Path, (data) => RegisterPrototype(index.Uri, data));
                        break;
                    default:
                        Debug.LogErrorFormat("PrototypeType not registered: {0}", index.ProtoypeType);
                        break;
                }
                foreach (var s in sub)
                {
                    yield return s;
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
