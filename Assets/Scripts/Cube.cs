using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]

public class Cube : MonoBehaviour
{
    [SerializeField] private Color _color = Color.white;

    private bool _hasCollided;
    private Coroutine _returnCoroutine;
    private Renderer _renderer;
    private Rigidbody _rigidbody;

    public event Action<Cube> ReturnRequested;

    public bool HasCollided
    {
        get => _hasCollided;
        private set => _hasCollided = value;
    }

    public Coroutine ReturnCoroutine
    {
        get => _returnCoroutine;
        private set => _returnCoroutine = value;
    }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();

        _renderer.material.color = _color;
    }

    private void OnEnable()
    {
        SetColorChangedThisLife(false);
    }

    private void OnDisable()
    {
        if (ReturnCoroutine != null)
        {
            StopCoroutine(ReturnCoroutine);
        }
    }

    public void SetColorChangedThisLife(bool hasCollided)
    {
        _hasCollided = hasCollided;
    }

    public void SetReturnCoroutine()
    {
        float minDeliteMinutes = 2f;
        float maxDeliteMinutes = 5f;

        StartCoroutine(ReturnAfterDelay(UnityEngine.Random.Range(minDeliteMinutes, maxDeliteMinutes)));
    }

    private void RequestReturn()
    {
        ResetPhysicsAndTransform();
        ResetColor();
        ReturnRequested?.Invoke(this);
    }

    private IEnumerator ReturnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        RequestReturn();
    }

    private void ResetColor()
    {
        if (_renderer != null)
            _renderer.material.color = _color;
    }

    private void ResetPhysicsAndTransform()
    {
        if (_rigidbody != null)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        transform.rotation = Quaternion.identity;
    }
}