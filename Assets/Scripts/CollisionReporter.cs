using UnityEngine;

public class CollisionReporter : MonoBehaviour
{
    [SerializeField] private ColorChanger _colorChanger;

    private Cube _cube;

    private void Awake()
    {
        _cube = GetComponent<Cube>();

        if (_colorChanger == null)
            _colorChanger = FindObjectOfType<ColorChanger>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out Ground ground))
        {
            if (_cube.HasCollided == false)
            {
                _colorChanger.ChangeColor(_cube);
            }

            if (_cube.ReturnCoroutine == null)
                _cube.SetReturnCoroutine();
        }
    }
}