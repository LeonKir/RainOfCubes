using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class GameObjectSpawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField, Min(0.01f)] private float _repeatRate = 0.5f;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 50;

    private ObjectPool<Cube> _objectPool;
    private Coroutine _spawnRoutine;

    private void Awake()
    {
        _objectPool = new ObjectPool<Cube>(
            createFunc: CreateCube,
            actionOnGet: OnGetCube,
            actionOnRelease: OnReleaseCube,
            actionOnDestroy: OnDestroyCube,
            collectionCheck: false,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
        );
    }

    private void Start()
    {
        _spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    private void OnDisable()
    {
        if (_spawnRoutine != null)
        {
            StopCoroutine(_spawnRoutine);
            _spawnRoutine = null;
        }
    }

    private Cube CreateCube()
    {
        return Instantiate(_prefab);
    }

    private void OnGetCube(Cube cube)
    {
        cube.transform.position = GetRandomPointInBox(_boxCollider);

        if (cube.TryGetComponent(out Rigidbody rb))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        cube.ReturnRequested -= HandleReturnRequested;
        cube.ReturnRequested += HandleReturnRequested;

        cube.gameObject.SetActive(true);
    }

    private void OnReleaseCube(Cube cube)
    {
        cube.ResetPhysicsAndTransform();

        cube.ReturnRequested -= HandleReturnRequested;
        cube.gameObject.SetActive(false);
        cube.ResetColor();
    }

    private void OnDestroyCube(Cube cube)
    {
        Destroy(cube.gameObject);
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnCube();
            yield return new WaitForSeconds(_repeatRate);
        }
    }

    private void SpawnCube()
    {
        _objectPool.Get();
    }

    private void HandleReturnRequested(Cube cube)
    {
        _objectPool.Release(cube);
    }

    private static Vector3 GetRandomPointInBox(BoxCollider box)
    {
        Vector3 min = box.bounds.min;
        Vector3 max = box.bounds.max;
        return new Vector3(
            UnityEngine.Random.Range(min.x, max.x),
            UnityEngine.Random.Range(min.y, max.y),
            UnityEngine.Random.Range(min.z, max.z)
        );
    }
}