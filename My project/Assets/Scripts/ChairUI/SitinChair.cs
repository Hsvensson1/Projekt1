using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;  // Lägg till för VideoPlayer

public class ChairInteraction : MonoBehaviour
{
    public GameObject uiPanel;  // UI som ska visas när spelaren interagerar med stolen
    public Transform playerSeatPosition;  // Där spelaren ska "sitta" (en markerad plats på stolen)
    public GameObject player;  // Referens till spelaren (som inkluderar kameran och rigidbody)
    public Transform screenLookAt;  // Objektet kameran ska rotera mot när spelaren sitter
    public float sitHeightOffset = -0.5f;  // Hur mycket spelaren ska flyttas ned när de sätter sig
    public GameObject[] lights;  // Lamporna som ska slås av och på

    // VideoPlayer-komponenter
    public VideoPlayer videoPlayer1;  // Första VideoPlayern
    public VideoPlayer videoPlayer2;  // Andra VideoPlayern
    public RawImage screenDisplay;    // Skärm som ska visa video

    private bool isNearChair = false;  // Kontrollera om spelaren är nära stolen
    private bool isSitting = false;   // Kontrollera om spelaren är "satt"
    private bool areLightsOn = true;  // Kontrollera om lamporna är på eller av

    private Vector3 savedPlayerPosition;  // För att spara spelarens position innan de sätter sig
    private Vector3 originalCameraPosition;  // För att spara spelarens ursprungliga kameraposition

    private MonoBehaviour playerMovementScript;  // För att kunna inaktivera rörelsekomponenten

    void Start()
    {
        originalCameraPosition = player.transform.GetChild(0).localPosition;  // Spara spelarens ursprungliga kameraposition
        playerMovementScript = player.GetComponent<MonoBehaviour>();

        // Kontrollera om rörelseskript saknas
        if (playerMovementScript == null)
        {
            Debug.LogWarning("Inget rörelseskript funnet på spelaren. Du kanske måste lägga till ett!");
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

        // Rotera kameran mot skärmen
        if (isSitting && screenLookAt != null)
        {
            Vector3 direction = screenLookAt.position - player.transform.GetChild(0).position; // Skapar en riktning mot skärmen
            Quaternion targetRotation = Quaternion.LookRotation(direction);  // Beräknar den riktning kameran ska ha
            player.transform.GetChild(0).rotation = Quaternion.Slerp(player.transform.GetChild(0).rotation, targetRotation, Time.deltaTime * 5f);  // Rotera kameran mot skärmen med mjuk övergång
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

        // Spara spelarens aktuella position innan de sätter sig
        savedPlayerPosition = player.transform.position;

        // Flytta spelaren till stolen
        Vector3 sitPosition = playerSeatPosition.position;
        sitPosition.y += sitHeightOffset; // Justera höjden för en sittande position
        player.transform.position = sitPosition;

        // Justera kamerans position utan att påverka spelarens position
        player.transform.GetChild(0).localPosition = originalCameraPosition + new Vector3(0, sitHeightOffset, 0);

        // Inaktivera rörelsekomponenten
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
        uiPanel.SetActive(false);  // Dölj UI-panelen

        // Återaktivera rörelsekomponenten
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }

        // Dölja muspekaren
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Återställ spelarens position till positionen de var på innan de satte sig
        player.transform.position = savedPlayerPosition;

        // Återställ kamerans position
        player.transform.GetChild(0).localPosition = originalCameraPosition;
    }

    private void ToggleUI()
    {
        uiPanel.SetActive(!uiPanel.activeSelf);
    }

    // Funktion som togglar lamporna på och av
    public void ToggleLights()
    {
        areLightsOn = !areLightsOn;

        foreach (GameObject light in lights)
        {
            light.SetActive(areLightsOn);  // Om lamporna ska vara på eller av
        }
    }

    // Funktion för att spela första videon
    public void PlayVideo1()
    {
        if (videoPlayer1 != null)
        {
            videoPlayer1.Play();  // Spela video 1
            videoPlayer2.Stop();  // Stoppa video 2 om den spelas
        }
    }

    // Funktion för att spela andra videon
    public void PlayVideo2()
    {
        if (videoPlayer2 != null)
        {
            videoPlayer2.Play();  // Spela video 2
            videoPlayer1.Stop();  // Stoppa video 1 om den spelas
        }
    }
}
