using System.Collections;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] private ColorChanger _colorChanger;

    private void OnCollisionEnter(Collision collision)
    {
        float minDeliteMinutes = 2f;
        float maxDeliteMinutes = 5f;

        if (collision.collider.TryGetComponent<Cube>(out Cube cube))
        {
            if (!cube.IsColorChangedThisLife)
            {
                _colorChanger.ChangeColor(cube);
            }

            if (cube.ReturnCoroutine == null)
                cube.SetReturnCoroutine(StartCoroutine(ReturnAfterDelay(UnityEngine.Random.Range(minDeliteMinutes, maxDeliteMinutes), cube)));
        }
    }

    private IEnumerator ReturnAfterDelay(float delay, Cube cube)
    {
        yield return new WaitForSeconds(delay);
        cube.RequestReturn();
    }
}