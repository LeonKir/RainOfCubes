using UnityEngine;

public class CollisionReporter : MonoBehaviour
{
    [SerializeField] private ColorChanger _colorChanger;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out Cube cube))
        {
            if (cube.IsColorChangedThisLife == false)
            {
                _colorChanger.ChangeColor(cube);
            }

            if (cube.ReturnCoroutine == null)
                cube.SetReturnCoroutine();
        }
    }
}