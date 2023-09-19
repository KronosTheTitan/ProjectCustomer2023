using UnityEngine;

/// <summary>
/// This class changes the color of a Renderer component to a random color when the game starts.
/// </summary>
public class RandomColor : MonoBehaviour
{
    [SerializeField] private Renderer rend;

    private void Start()
    {
        // Check if the Renderer component is assigned.
        if (rend != null)
        {
            // Generate a random color.
            Color randomColor = new Color(Random.value, Random.value, Random.value);

            // Set the material's color to the random color.
            rend.material.color = randomColor;
        }
    }
}