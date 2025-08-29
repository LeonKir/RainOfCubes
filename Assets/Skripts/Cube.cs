using System;
using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public event Action<Cube> ReturnRequested;

    [SerializeField] private Color _color = Color.white;

    private bool _isColorChangedThisLife;
    private Coroutine _returnCoroutine;
    private Renderer _renderer;
    private Rigidbody _rigidbody;

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

    public bool IsColorChangedThisLife
    {
        get => _isColorChangedThisLife;
        private set => _isColorChangedThisLife = value;
    }

    public void SetColorChangedThisLife(bool colorChangedThisLife)
    {
        _isColorChangedThisLife = colorChangedThisLife;
    }

    public Coroutine ReturnCoroutine
    {
        get => _returnCoroutine;
        private set => _returnCoroutine = value;
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