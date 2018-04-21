using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Models;
using Assets.Scripts.Serialization;

namespace Assets.Scripts.Managers
{
    [Serializable]
    public class PrototypeManager : Singleton<PrototypeManager>
    {
        // public List<WordPart> VowelPrototypes;

        // public GameSettings GameSettings;

        public bool Loaded = false;

        public IEnumerator LoadPrototypes(Action onLoadCompleted)
        {
            // VowelPrototypes = new List<WordPart>();
            // var sub = Load<List<WordPart>, WordPart>(VowelPrototypes, "Vowels.xml");
            // foreach (var s in sub)
            // {
            //     yield return s;
            // }

            // var settings = new List<GameSettings>();
            // sub = Load<List<GameSettings>, GameSettings>(settings, "GameSettings.xml");
            // foreach (var s in sub)
            // {
            //     yield return s;
            // }
            // GameSettings = settings.First();

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
    }
}
