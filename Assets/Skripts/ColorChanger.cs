using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public void ChangeColor(Cube cube)
    {
        var renderer = cube.GetComponent<Renderer>();
        if (!cube.IsColorChangedThisLife && renderer != null)
        {
            renderer.material.color = UnityEngine.Random.ColorHSV();
            cube.SetColorChangedThisLife(true);
        }
    }
}