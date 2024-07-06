using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YugantLoyaLibrary.Game2048
{
    public enum PoolObjectType
    {
        FloatingText
    }

    [Serializable]
    public struct ObjectTypeStruct
    {
        public PoolObjectType objType;
        public GameObject objPrefab;
        public Transform poolTypeContainer;
        public int size;
    }

    public class ObjectPoolSystem : MonoBehaviour
    {
        public static ObjectPoolSystem Instance;

        public List<ObjectTypeStruct> poolObjList;

        private Dictionary<PoolObjectType, List<GameObject>> _poolDict =
            new Dictionary<PoolObjectType, List<GameObject>>();


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            FillPool();
        }

        void FillPool()
        {
            foreach (ObjectTypeStruct typeStruct in poolObjList)
            {
                List<GameObject> tempList = new List<GameObject>();

                for (int i = 0; i < typeStruct.size; i++)
                {
                    GameObject obj = Instantiate(typeStruct.objPrefab, typeStruct.poolTypeContainer);
                    obj.SetActive(false);
                    tempList.Add(obj);
                }
                
                if (_poolDict.TryAdd(typeStruct.objType, tempList))
                {
                    
                }
            }
        }

        public GameObject GetObjectByType(PoolObjectType type)
        {
            GameObject objToSend = null;

            if (_poolDict.TryGetValue(type, out List<GameObject> objList))
            {
                objToSend = objList.First(temp => !temp.activeInHierarchy);

                if (objToSend != null)
                {
                    objToSend.SetActive(true);
                }
            }

            return objToSend;
        }

        public void ReturnToPool(PoolObjectType type, GameObject obj)
        {
            obj.SetActive(false);
            List<GameObject> tempList = GetValueListFromDictionary(type);

            if (tempList == null) return;

            tempList.Add(obj);

            if (_poolDict.ContainsKey(type))
            {
                _poolDict[type] = tempList;
            }
        }

        public List<GameObject> GetValueListFromDictionary(PoolObjectType type)
        {
            _poolDict.TryGetValue(type, out List<GameObject> objList);
            return objList;
        }
    }
}