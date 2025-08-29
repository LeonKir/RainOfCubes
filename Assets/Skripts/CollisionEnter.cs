using System.Collections;
using UnityEngine;

public class CollisionEnter : MonoBehaviour
{
    [SerializeField] private ColorChanger _colorChanger;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Cube>(out Cube cube))
        {
            if (!cube.IsColorChangedThisLife)
            {
                _colorChanger.ChangeColor(cube);
            }

            if (cube.ReturnCoroutine == null)
                cube.SetReturnCoroutine();
        }
    }
}
