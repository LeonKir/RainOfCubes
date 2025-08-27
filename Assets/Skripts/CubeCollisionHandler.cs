using UnityEngine;
using UnityEngine.Pool;

public class PooledCube : MonoBehaviour
{
    private IObjectPool<GameObject> _pool;
    private Renderer _renderer;
    private bool _colorChangedThisLife;

    public void Init(IObjectPool<GameObject> pool) => _pool = pool;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        _colorChangedThisLife = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            if (!_colorChangedThisLife && _renderer != null)
            {
                _renderer.material.color = Random.ColorHSV();
                _colorChangedThisLife = true;
            }

            Invoke(nameof(ReturnToPool), Random.Range(2f, 5f));
        }
    }

    private void ReturnToPool()
    {
        if (_pool != null)
            _pool.Release(gameObject);
        else
            gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}