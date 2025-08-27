using UnityEngine;
using UnityEngine.Pool;

public class GameObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private float _repeatRate;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _poolMaxSize;

    private ObjectPool<GameObject> _objectPool;

    private void Awake()
    {
        _objectPool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject obj = Instantiate(_prefab);

                if (!obj.TryGetComponent(out PooledCube pooled))
                    pooled = obj.AddComponent<PooledCube>();

                pooled.Init(_objectPool);

                return obj;
            },
            actionOnGet: obj =>
            {
                obj.transform.position = GetRandomPointInBox(_boxCollider);

                if (obj.TryGetComponent(out Rigidbody rb))
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }

                if (obj.TryGetComponent(out Renderer rend))
                {
                    rend.material.color = Color.white;
                }

                obj.SetActive(true);
            },
            actionOnRelease: obj => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private static Vector3 GetRandomPointInBox(BoxCollider box)
    {
        Vector3 boundsMin = box.bounds.min;
        Vector3 boundsMax = box.bounds.max;

        float x = Random.Range(boundsMin.x, boundsMax.x);
        float y = Random.Range(boundsMin.y, boundsMax.y);
        float z = Random.Range(boundsMin.z, boundsMax.z);

        return new Vector3(x, y, z);
    }

    private void Start()
    {
        InvokeRepeating(nameof(SpawnCube), 0f, _repeatRate);
    }

    private void SpawnCube()
    {
        GameObject obj = _objectPool.Get();
    }
}