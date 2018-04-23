using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Serialization
{
    public class DataSerializer : Singleton<DataSerializer>
    {
        public IEnumerable LoadFromStreamingAssets<T, TI>(T store, string folder, string fileName) 
            where T : class, IList<TI>, new()
            where TI : class, new()
        {
            string dataPath = Path.Combine(Application.streamingAssetsPath, folder);
            string filePath = Path.Combine(dataPath, fileName);

            var sub = Load<T, TI>(store, filePath);
            foreach (var s in sub)
            {
                yield return s;
            }
        }

        public IEnumerable LoadFromStreamingAssets<T>(string folder, string fileName, Action<T> store)
            where T : class, new()
        {
            string dataPath = Path.Combine(Application.streamingAssetsPath, folder);
            string filePath = Path.Combine(dataPath, fileName);

            var sub = Load<T>(filePath, store);
            foreach (var s in sub)
            {
                yield return s;
            }
        }

        public IEnumerable LoadFromAppData<T, TI>(T store, string folder, string fileName)
            where T : class, IList<TI>, new()
            where TI : class, new()
        {
            string dataPath = Application.persistentDataPath;
            string filePath = Path.Combine(dataPath, fileName);

            var sub = Load<T, TI>(store, filePath);
            foreach (var s in sub)
            {
                yield return s;
            }
        }
        
        public IEnumerable Load<T, TI>(T store, string fileName)
            where T : class, IList<TI>, new()
            where TI : class, new()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            var protocol = "";
#else
            var protocol = "file:///";
#endif

            //Debug.Log("Loading file: " + fileName);
            var www = new WWW(protocol + fileName);
            yield return www;

            if(!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError(www.error);
                yield break;
            }

            using (var memoryStream = new MemoryStream(www.bytes))
            {
                var datas = DeSerialize<T>(memoryStream);

                foreach (var data in datas)
                {
                    store.Add(data);
                }
            }
        }

        public IEnumerable Load<T>(string fileName, Action<T> store)
            where T : class, new()
        {

#if UNITY_WEBGL && !UNITY_EDITOR
            var protocol = "";
#else
            var protocol = "file:///";
#endif

            //Debug.Log("Loading file: " + fileName);
            var www = new WWW(protocol + fileName);
            yield return www;

            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError(www.error);
                yield break;
            }

            using (var memoryStream = new MemoryStream(www.bytes))
            {
                var data = DeSerialize<T>(memoryStream);
                store(data);
            }
        }

        public T DeSerialize<T>(Stream stream) where T : class
        {
            try
            {
                var extraTypes = GetExtraTypes();
                var serializer = new XmlSerializer(typeof(T), extraTypes);
                return serializer.Deserialize(stream) as T;
            }
            catch (Exception e)
            {
                Debug.LogError("Error deserializin object of type " + typeof(T) + Environment.NewLine + e);
                throw;
            }
        }

        private Type[] GetExtraTypes()
        {
            return new[]
            {
                typeof(object),
                // typeof(WordPart),
                // typeof(DialogLine),
            };
        }
    }
}