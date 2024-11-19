using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;  // L�gg till f�r VideoPlayer

public class ChairInteraction : MonoBehaviour
{
    public GameObject uiPanel;  // UI som ska visas n�r spelaren interagerar med stolen
    public Transform playerSeatPosition;  // D�r spelaren ska "sitta" (en markerad plats p� stolen)
    public GameObject player;  // Referens till spelaren (som inkluderar kameran och rigidbody)
    public Transform screenLookAt;  // Objektet kameran ska rotera mot n�r spelaren sitter
    public float sitHeightOffset = -0.5f;  // Hur mycket spelaren ska flyttas ned n�r de s�tter sig
    public GameObject[] lights;  // Lamporna som ska sl�s av och p�

    // VideoPlayer-komponenter
    public VideoPlayer videoPlayer1;  // F�rsta VideoPlayern
    public VideoPlayer videoPlayer2;  // Andra VideoPlayern
    public RawImage screenDisplay;    // Sk�rm som ska visa video

    private bool isNearChair = false;  // Kontrollera om spelaren �r n�ra stolen
    private bool isSitting = false;   // Kontrollera om spelaren �r "satt"
    private bool areLightsOn = true;  // Kontrollera om lamporna �r p� eller av

    private Vector3 savedPlayerPosition;  // F�r att spara spelarens position innan de s�tter sig
    private Vector3 originalCameraPosition;  // F�r att spara spelarens ursprungliga kameraposition

    private MonoBehaviour playerMovementScript;  // F�r att kunna inaktivera r�relsekomponenten

    void Start()
    {
        originalCameraPosition = player.transform.GetChild(0).localPosition;  // Spara spelarens ursprungliga kameraposition
        playerMovementScript = player.GetComponent<MonoBehaviour>();

        // Kontrollera om r�relseskript saknas
        if (playerMovementScript == null)
        {
            Debug.LogWarning("Inget r�relseskript funnet p� spelaren. Du kanske m�ste l�gga till ett!");
        }
    }

    void Update()
    {
        if (isNearChair && Input.GetKeyDown(KeyCode.E) && !isSitting)
        {
            SitOnChair();
        }
        else if (isSitting && Input.GetKeyDown(KeyCode.Q))
        {
            ToggleUI();
        }
        else if (isSitting && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
        {
            StandUp();
        }

        // Rotera kameran mot sk�rmen
        if (isSitting && screenLookAt != null)
        {
            Vector3 direction = screenLookAt.position - player.transform.GetChild(0).position; // Skapar en riktning mot sk�rmen
            Quaternion targetRotation = Quaternion.LookRotation(direction);  // Ber�knar den riktning kameran ska ha
            player.transform.GetChild(0).rotation = Quaternion.Slerp(player.transform.GetChild(0).rotation, targetRotation, Time.deltaTime * 5f);  // Rotera kameran mot sk�rmen med mjuk �verg�ng
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearChair = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearChair = false;
        }
    }

    private void SitOnChair()
    {
        isSitting = true;
        uiPanel.SetActive(true);  // Visa UI-panelen

        // Spara spelarens aktuella position innan de s�tter sig
        savedPlayerPosition = player.transform.position;

        // Flytta spelaren till stolen
        Vector3 sitPosition = playerSeatPosition.position;
        sitPosition.y += sitHeightOffset; // Justera h�jden f�r en sittande position
        player.transform.position = sitPosition;

        // Justera kamerans position utan att p�verka spelarens position
        player.transform.GetChild(0).localPosition = originalCameraPosition + new Vector3(0, sitHeightOffset, 0);

        // Inaktivera r�relsekomponenten
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }

        // Visa muspekaren
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void StandUp()
    {
        isSitting = false;
        uiPanel.SetActive(false);  // D�lj UI-panelen

        // �teraktivera r�relsekomponenten
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }

        // D�lja muspekaren
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // �terst�ll spelarens position till positionen de var p� innan de satte sig
        player.transform.position = savedPlayerPosition;

        // �terst�ll kamerans position
        player.transform.GetChild(0).localPosition = originalCameraPosition;
    }

    private void ToggleUI()
    {
        uiPanel.SetActive(!uiPanel.activeSelf);
    }

    // Funktion som togglar lamporna p� och av
    public void ToggleLights()
    {
        areLightsOn = !areLightsOn;

        foreach (GameObject light in lights)
        {
            light.SetActive(areLightsOn);  // Om lamporna ska vara p� eller av
        }
    }

    // Funktion f�r att spela f�rsta videon
    public void PlayVideo1()
    {
        if (videoPlayer1 != null)
        {
            videoPlayer1.Play();  // Spela video 1
            videoPlayer2.Stop();  // Stoppa video 2 om den spelas
        }
    }

    // Funktion f�r att spela andra videon
    public void PlayVideo2()
    {
        if (videoPlayer2 != null)
        {
            videoPlayer2.Play();  // Spela video 2
            videoPlayer1.Stop();  // Stoppa video 1 om den spelas
        }
    }
}
