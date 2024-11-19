using UnityEngine;

public class ExitOnClick : MonoBehaviour
{
    public KeyCode exitKey = KeyCode.E; // Knappen som används för att avsluta
    private bool isPlayerNearby = false;

    private void Update()
    {
        // Kontrollera om spelaren är nära och trycker på rätt knapp
        if (isPlayerNearby && Input.GetKeyDown(exitKey))
        {
            Debug.Log("Spelet avslutas...");
            Application.Quit(); // Avslutar spelet
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Kontrollera om spelaren går in i trigger-området
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Du kan nu avsluta spelet genom att trycka på 'E'.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // När spelaren lämnar trigger-området
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            Debug.Log("Du lämnade området, du kan inte längre avsluta spelet.");
        }
    }
}
