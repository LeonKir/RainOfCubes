using System;
using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public event Action<Cube> ReturnRequested;

    [SerializeField] private Color _color = Color.white;

    private Renderer _renderer;
    private Rigidbody _rigidbody;
    private bool _colorChangedThisLife;
    private Coroutine _returnCoroutine;

    public void ResetColor()
    {
        if (_renderer != null)
            _renderer.material.color = _color;
    }

    public void ResetPhysicsAndTransform()
    {
        if (_rigidbody != null)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        transform.rotation = Quaternion.identity;
    }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();

        _renderer.material.color = _color;
    }

    private void OnEnable()
    {
        _colorChangedThisLife = false;
        _returnCoroutine = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        float minDeliteMinyte = 2f;
        float maxDeliteMinyte = 5f;

        if (collision.collider.TryGetComponent<Ground>(out _))
        {
            if (!_colorChangedThisLife && _renderer != null)
            {
                _renderer.material.color = UnityEngine.Random.ColorHSV();
                _colorChangedThisLife = true;
            }

            if (_returnCoroutine == null)
                _returnCoroutine = StartCoroutine(ReturnAfterDelay(UnityEngine.Random.Range(minDeliteMinyte, maxDeliteMinyte)));
        }
    }

    private IEnumerator ReturnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RequestReturn();
    }

    private void RequestReturn()
    {
        ReturnRequested?.Invoke(this);
    }

    private void OnDisable()
    {
        if (_returnCoroutine != null)
        {
            StopCoroutine(_returnCoroutine);
            _returnCoroutine = null;
        }
    }
}
