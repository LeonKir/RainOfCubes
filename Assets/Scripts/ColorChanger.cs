using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public void ChangeColor(Cube cube)
    {
        var renderer = cube.GetComponent<Renderer>();

        if (cube.HasCollided == false && renderer != null)
        {
            renderer.material.color = UnityEngine.Random.ColorHSV();

            cube.SetColorChangedThisLife(true);
        }
    }
}