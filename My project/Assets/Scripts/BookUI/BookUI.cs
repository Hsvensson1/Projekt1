using UnityEngine;
using TMPro;  // Om du använder TextMeshPro istället för vanlig Text

public class BookInteraction : MonoBehaviour
{
    [Header("Book Settings")]
    public GameObject bookObject; // Boken i världen
    public float interactionDistance = 3f; // Avstånd för interaktion

    [Header("UI Settings")]
    public GameObject bookPanel; // Panel för UI-spriten av den öppna boken
    public TextMeshProUGUI[] pages; // TextMeshPro sidor i boken (eller använd Text om du inte har TMP)

    [Header("Navigation Buttons")]
    public GameObject nextPageButton; // Knapp för nästa sida
    public GameObject previousPageButton; // Knapp för föregående sida

    [Header("Player Settings")]
    public Camera playerCamera; // Spelarens kamera
    public GameObject player; // Spelaren (för att inaktivera kontroller)

    private int currentPage = 0; // Spårar aktuell sida
    private bool isBookOpen = false; // Spårar om boken är öppen

    void Start()
    {
        // Dölja alla sidor förutom den första
        for (int i = 1; i < pages.Length; i++)
        {
            pages[i].gameObject.SetActive(false);
        }

        // Sätt upp knappens funktionalitet
        nextPageButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(NextPage);
        previousPageButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(PreviousPage);
    }

    void Update()
    {
        // Kontrollera interaktion
        if (Input.GetKeyDown(KeyCode.E))
        {
            float distance = Vector3.Distance(player.transform.position, bookObject.transform.position);
            if (distance <= interactionDistance)
            {
                if (!isBookOpen)
                {
                    OpenBook();
                }
                else
                {
                    CloseBook();
                }
            }
        }
    }

    private void OpenBook()
    {
        // Aktivera bokens UI
        bookPanel.SetActive(true);

        // Lås kameran och aktivera muspekaren
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Inaktivera spelarens rörelse och kamerarörelse
        if (player != null)
        {
            var playerController = player.GetComponent<MovePlayer>();
            if (playerController != null)
            {
                playerController.enabled = false;
            }

            var lookController = player.GetComponent<LookFirstPerson>();
            if (lookController != null)
            {
                lookController.enabled = false;
            }
        }

        // Markera att boken är öppen
        isBookOpen = true;
    }

    private void CloseBook()
    {
        // Stäng bokens UI
        bookPanel.SetActive(false);

        // Lås muspekaren igen och lås upp kameran
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Återaktivera spelarens rörelse och kamerarörelse
        if (player != null)
        {
            var playerController = player.GetComponent<MovePlayer>();
            if (playerController != null)
            {
                playerController.enabled = true;
            }

            var lookController = player.GetComponent<LookFirstPerson>();
            if (lookController != null)
            {
                lookController.enabled = true;
            }
        }

        // Markera att boken är stängd
        isBookOpen = false;
    }

    // Funktionen för att bläddra till nästa sida
    private void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            pages[currentPage].gameObject.SetActive(false);
            currentPage++;
            pages[currentPage].gameObject.SetActive(true);
        }
    }

    // Funktionen för att bläddra till föregående sida
    private void PreviousPage()
    {
        if (currentPage > 0)
        {
            pages[currentPage].gameObject.SetActive(false);
            currentPage--;
            pages[currentPage].gameObject.SetActive(true);
        }
    }
}
