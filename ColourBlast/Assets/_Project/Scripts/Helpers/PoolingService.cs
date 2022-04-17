using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ColourBlast.Helpers
{
    public class PoolingService
    {
        private int _capacity;
        private int _size;
        private GameObject _prefab;

        private Stack<GameObject> _poolingObjects = new Stack<GameObject>();
        
        public PoolingService(int capacity , GameObject prefab)
        {
            _capacity = capacity;
            _prefab = prefab;
        }
        
        public void Init()
        {
            for (int i = 0; i < _capacity; i++)
            {
                Create();
            }
        }

        public GameObject Spawn(Vector3 position, Vector3 rotation)
        {          
            if(CanSpawn())
            {
                var poolObject =  _poolingObjects.Pop();
                poolObject.transform.SetPositionAndRotation(position,Quaternion.Euler(rotation));
                poolObject.SetActive(true);          
                return poolObject;
            }
            return null;     
        }

        public void Release(GameObject gameObject)
        {
            gameObject.SetActive(false);
            _poolingObjects.Push(gameObject);
        }

        private void Create()
        {
            var pooledObject = GameObject.Instantiate(_prefab);
            pooledObject.SetActive(false);
            _poolingObjects.Push(pooledObject);
            _size++;
        }

        private bool CanSpawn()
        {          
            if(_size < _capacity)
            {
                Create();
            }

            if(_poolingObjects.Any())
            {
                return true;
            }
            return false;
        }

    }
}
