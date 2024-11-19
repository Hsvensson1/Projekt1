using UnityEngine;
using TMPro;  // Om du anv�nder TextMeshPro ist�llet f�r vanlig Text

public class BookInteraction : MonoBehaviour
{
    [Header("Book Settings")]
    public GameObject bookObject; // Boken i v�rlden
    public float interactionDistance = 3f; // Avst�nd f�r interaktion

    [Header("UI Settings")]
    public GameObject bookPanel; // Panel f�r UI-spriten av den �ppna boken
    public TextMeshProUGUI[] pages; // TextMeshPro sidor i boken (eller anv�nd Text om du inte har TMP)

    [Header("Navigation Buttons")]
    public GameObject nextPageButton; // Knapp f�r n�sta sida
    public GameObject previousPageButton; // Knapp f�r f�reg�ende sida

    [Header("Player Settings")]
    public Camera playerCamera; // Spelarens kamera
    public GameObject player; // Spelaren (f�r att inaktivera kontroller)

    private int currentPage = 0; // Sp�rar aktuell sida
    private bool isBookOpen = false; // Sp�rar om boken �r �ppen

    void Start()
    {
        // D�lja alla sidor f�rutom den f�rsta
        for (int i = 1; i < pages.Length; i++)
        {
            pages[i].gameObject.SetActive(false);
        }

        // S�tt upp knappens funktionalitet
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

        // L�s kameran och aktivera muspekaren
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Inaktivera spelarens r�relse och kamerar�relse
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

        // Markera att boken �r �ppen
        isBookOpen = true;
    }

    private void CloseBook()
    {
        // St�ng bokens UI
        bookPanel.SetActive(false);

        // L�s muspekaren igen och l�s upp kameran
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // �teraktivera spelarens r�relse och kamerar�relse
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

        // Markera att boken �r st�ngd
        isBookOpen = false;
    }

    // Funktionen f�r att bl�ddra till n�sta sida
    private void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            pages[currentPage].gameObject.SetActive(false);
            currentPage++;
            pages[currentPage].gameObject.SetActive(true);
        }
    }

    // Funktionen f�r att bl�ddra till f�reg�ende sida
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
