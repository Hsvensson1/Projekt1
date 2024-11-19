using UnityEngine;

public class FloatUpDown : MonoBehaviour
{
    public float amplitude = 0.2f;  // Hur h�gt objektet ska r�ra sig
    public float frequency = 1.0f;  // Hur snabbt objektet ska guppa

    private Vector3 initialPosition;

    void Start()
    {
        // Spara den initiala positionen
        initialPosition = transform.position;
    }

    void Update()
    {
        // Skapa en guppande r�relse med hj�lp av Mathf.Sin
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;

        // Uppdatera objektets position
        transform.position = new Vector3(initialPosition.x, initialPosition.y + yOffset, initialPosition.z);
    }
}

