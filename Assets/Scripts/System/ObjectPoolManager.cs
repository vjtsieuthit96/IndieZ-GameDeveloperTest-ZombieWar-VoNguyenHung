using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;
public class ObjectPoolManager : MonoBehaviour
{

    /* Read Me:
        - To Use this Pool Manager, simply call the static method SpawnObject from anywhere in your code
        (make sure this script is attached to a GameObject in your scene).
            Example:
                1. GameObject myObject = ObjectPoolManager.SpawnObject(myPrefab, spawnPosition, spawnRotation, ObjectPoolManager.PoolType.GameObject);
                2. ParticleSystem myParticle = ObjectPoolManager.SpawnObject(myParticlePrefab, spawnPosition, spawnRotation, ObjectPoolManager.PoolType.ParticleSystem);

        - If it does not have PoolType specified, it will default to GameObject type.
        - When you are done with the object and want to return it to the pool, call ReturnObectToPool:
            Example:
                1. ObjectPoolManager.ReturnObjectToPool(myObject);
                2. ObjectPoolManager.ReturnObjectToPool(myParticle, ObjectPoolManager.PoolType.ParticleSystem);
        *** Notice: if you have multiple pool types, make sure to specify the CORRECT PoolType when returning the object. ***
        
        - To make Object spawned as child of a parent transform, you can use the overload method with Transform parent parameter.

            Example: you have a gun transform and want to spawn a bullet in this position as child of the gun
                1. GameObject myObject = ObjectPoolManager.SpawnObject(myPrefab, parentTransform, spawnRotation, ObjectPoolManager.PoolType.GameObject);
        - When returning the object to pool, you can just call the same method as before and they go back to where they belong.
            Example:
                1. ObjectPoolManager.ReturnObjectToPool(myObject); 

    */


    // use for dont destroy object
    // like you can use this for audio manager or game manager
    [SerializeField] private bool _addToDontDestroyOnLoad = false;

    // empty Holder 
    private GameObject _emptyHolder;

    // empty GameObjects for different pool types
    private static GameObject _particleSystemsEmpty;
    private static GameObject _gameObjectsEmpty;
    private static GameObject _soundFXEmpty;

    private static Dictionary<GameObject, ObjectPool<GameObject>> _objectPools;
    private static Dictionary<GameObject, GameObject> _cloneToPrefabMap;

    // if you have any pool type, make sure to add it here
    // enum tell the pooler where to spawn the object
    public enum PoolType
    {
        ParticleSystem,
        GameObject,
        SoundFX
    }

    public static PoolType PoolingType;

    private void Awake()
    {
        _objectPools = new Dictionary<GameObject, ObjectPool<GameObject>>();
        _cloneToPrefabMap = new Dictionary<GameObject, GameObject>();

        SetupEmpties();
    }

    private void SetupEmpties()
    {
        // Create the main Holder;
        _emptyHolder = new GameObject("Object Pools");

        // Create other child holders
        _particleSystemsEmpty = new GameObject("Particle Effects");
        _particleSystemsEmpty.transform.SetParent(_emptyHolder.transform);

        _gameObjectsEmpty = new GameObject("Game Objects");
        _gameObjectsEmpty.transform.SetParent(_emptyHolder.transform);

        _soundFXEmpty = new GameObject("Sound FX");
        _soundFXEmpty.transform.SetParent(_emptyHolder.transform);

        // if true, make the root object dont destroy on load
        if (_addToDontDestroyOnLoad)
        {
            DontDestroyOnLoad(_emptyHolder.transform.root);
        }
    }

    private static void CreatePool(GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.GameObject)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () => CreateObject(prefab, pos, rot, poolType),
            actionOnGet: OnGetObject,
            actionOnRelease: OnReleaseObject,
            actionOnDestroy: OnDestroyObject
        );
        _objectPools.Add(prefab, pool);
    }
    //Create with parent reference
    private static void CreatePool(GameObject prefab, Transform parent, Quaternion rot, PoolType poolType = PoolType.GameObject)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () => CreateObject(prefab, parent, rot, poolType),
            actionOnGet: OnGetObject,
            actionOnRelease: OnReleaseObject,
            actionOnDestroy: OnDestroyObject
        );
        _objectPools.Add(prefab, pool);
    }


    private static GameObject CreateObject(GameObject prejab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.GameObject)
    {
        // ensure the object and non have awake  or on enable called guaranteed
        prejab.SetActive(false);

        GameObject obj = Instantiate(prejab, pos, rot);

        prejab.SetActive(true);

        GameObject parentObject = SetParentObject(poolType);
        obj.transform.SetParent(parentObject.transform);
        return obj;
    }
    //Create with parent reference
    private static GameObject CreateObject(GameObject prejab, Transform parent, Quaternion rot, PoolType poolType = PoolType.GameObject)
    {
        // ensure the object and non have awake  or on enable called guaranteed
        prejab.SetActive(false);

        GameObject obj = Instantiate(prejab, parent);

        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = rot;
        obj.transform.localScale = Vector3.one;

        prejab.SetActive(true);

        return obj;
    }
    private static void OnGetObject(GameObject obj)
    {
        // optional logic
    }

    private static void OnReleaseObject(GameObject obj)
    {
        obj.SetActive(false);

    }

    private static void OnDestroyObject(GameObject obj)
    {
        if (_cloneToPrefabMap.ContainsKey(obj))
        {
            _cloneToPrefabMap.Remove(obj);
        }
    }

    private static GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.ParticleSystem:

                return _particleSystemsEmpty;

            case PoolType.GameObject:

                return _gameObjectsEmpty;

            case PoolType.SoundFX:

                return _soundFXEmpty;

            default:
                return null;
        }
    }

    private static T SpawnObject<T>(GameObject objectToSpawn, Vector3 spawnPos, Quaternion spawnRotation, PoolType poolType = PoolType.GameObject) where T : Object
    {
        // check the pool already exists
        if (!_objectPools.ContainsKey(objectToSpawn))
        {
            CreatePool(objectToSpawn, spawnPos, spawnRotation, poolType);
        }
        // let get game object grabbing object to spawn from dictionary and callint Unity get method form the pool system
        GameObject obj = _objectPools[objectToSpawn].Get();

        if (obj != null)
        {
            // add mapping if not already present
            if (!_cloneToPrefabMap.ContainsKey(obj))
            {
                _cloneToPrefabMap.Add(obj, objectToSpawn);
            }


            obj.transform.position = spawnPos;
            obj.transform.rotation = spawnRotation;
            obj.SetActive(true);

            if (typeof(T) == typeof(GameObject))
            {
                return obj as T;
            }
            T component = obj.GetComponent<T>();
            if (component == null)
            {
                Debug.LogError($"Object {objectToSpawn.name} does not have component of type {typeof(T)}");
                return null;
            }
            return component;
        }
        return null;
    }

    public static T SpawnObject<T>(T objectToSpawn, Transform parent, Quaternion spawnRotation, PoolType poolType = PoolType.GameObject) where T : Component
    {
        return SpawnObject<T>(objectToSpawn.gameObject, parent, spawnRotation, poolType);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Transform parent, Quaternion spawnRotation, PoolType poolType = PoolType.GameObject)
    {
        return SpawnObject<GameObject>(objectToSpawn, parent, spawnRotation, poolType);
    }

    public static T SpawnObject<T>(T objectToSpawn, Vector3 spawnPos, Quaternion spawnRotation, PoolType poolType = PoolType.GameObject) where T : Component
    {
        return SpawnObject<T>(objectToSpawn.gameObject, spawnPos, spawnRotation, poolType);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPos, Quaternion spawnRotation, PoolType poolType = PoolType.GameObject)
    {
        return SpawnObject<GameObject>(objectToSpawn, spawnPos, spawnRotation, poolType);
    }
    private static T SpawnObject<T>(GameObject objectToSpawn, Transform parent, Quaternion spawnRotation, PoolType poolType = PoolType.GameObject) where T : Object
    {
        // check the pool already exists
        if (!_objectPools.ContainsKey(objectToSpawn))
        {
            CreatePool(objectToSpawn, parent, spawnRotation, poolType);
        }
        // let get game object grabbing object to spawn from dictionary and callint Unity get method form the pool system
        GameObject obj = _objectPools[objectToSpawn].Get();

        if (obj != null)
        {
            // add mapping if not already present
            if (!_cloneToPrefabMap.ContainsKey(obj))
            {
                _cloneToPrefabMap.Add(obj, objectToSpawn);
            }

            // setting parent transform
            obj.transform.SetParent(parent);
            // setting local position and rotation
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = spawnRotation;
            obj.SetActive(true);

            if (typeof(T) == typeof(GameObject))
            {
                return obj as T;
            }
            T component = obj.GetComponent<T>();
            if (component == null)
            {
                Debug.LogError($"Object {objectToSpawn.name} does not have component of type {typeof(T)}");
                return null;
            }
            return component;
        }
        return null;
    }


    public static void ReturnObjectToPool(GameObject obj, PoolType poolType = PoolType.GameObject)
    {
        if (_cloneToPrefabMap.TryGetValue(obj, out GameObject prefab))
        {
            GameObject parentObject = SetParentObject(poolType);
            if (obj.transform.parent != parentObject.transform)
            {
                obj.transform.SetParent(parentObject.transform);
            }
        }

        if (_objectPools.TryGetValue(prefab, out ObjectPool<GameObject> pool))
        {
            pool.Release(obj);
        }
        else
        {
            Debug.LogWarning("No pool found for the returned object: " + obj.name);
        }
    }
}