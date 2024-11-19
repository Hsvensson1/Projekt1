using UnityEngine;
using TMPro;  // F�r TextMeshPro
using System.Collections;

public class TypingEffect : MonoBehaviour
{
    [TextArea(3, 10)]  // G�r s� att textf�ltet blir st�rre och ger m�jlighet till radbrytningar
    public string fullText;  // Texten som ska skrivas ut

    public TextMeshProUGUI uiText;  // Referens till TextMeshPro-komponenten
    public float typingSpeed = 0.05f;  // Hur snabbt texten skrivs ut

    private void Start()
    {
        // Starta korutinen som skriver ut texten
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        uiText.text = "";  // Rensa texten innan den b�rjar skrivas

        // G� igenom varje tecken i fullText
        foreach (char letter in fullText)
        {
            // Om vi hittar ett radbrytningstecken, behandla det som en ny rad
            if (letter == '\n')
            {
                uiText.text += "\n";  // L�gg till en riktig radbrytning
                yield return new WaitForSeconds(typingSpeed);  // V�nta innan vi forts�tter
                continue;
            }

            uiText.text += letter;  // L�gg till bokstaven till UI-texten
            yield return new WaitForSeconds(typingSpeed);  // V�nta innan n�sta bokstav skrivs ut
        }
    }
}
