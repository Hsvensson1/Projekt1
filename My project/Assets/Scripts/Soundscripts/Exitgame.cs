using UnityEngine;

public class ExitOnClick : MonoBehaviour
{
    public KeyCode exitKey = KeyCode.E; // Knappen som anv�nds f�r att avsluta
    private bool isPlayerNearby = false;

    private void Update()
    {
        // Kontrollera om spelaren �r n�ra och trycker p� r�tt knapp
        if (isPlayerNearby && Input.GetKeyDown(exitKey))
        {
            Debug.Log("Spelet avslutas...");
            Application.Quit(); // Avslutar spelet
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Kontrollera om spelaren g�r in i trigger-omr�det
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Du kan nu avsluta spelet genom att trycka p� 'E'.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // N�r spelaren l�mnar trigger-omr�det
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            Debug.Log("Du l�mnade omr�det, du kan inte l�ngre avsluta spelet.");
        }
    }
}
