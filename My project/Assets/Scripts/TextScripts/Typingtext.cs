using UnityEngine;
using TMPro;  // För TextMeshPro
using System.Collections;

public class TypingEffect : MonoBehaviour
{
    [TextArea(3, 10)]  // Gör så att textfältet blir större och ger möjlighet till radbrytningar
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
        uiText.text = "";  // Rensa texten innan den börjar skrivas

        // Gå igenom varje tecken i fullText
        foreach (char letter in fullText)
        {
            // Om vi hittar ett radbrytningstecken, behandla det som en ny rad
            if (letter == '\n')
            {
                uiText.text += "\n";  // Lägg till en riktig radbrytning
                yield return new WaitForSeconds(typingSpeed);  // Vänta innan vi fortsätter
                continue;
            }

            uiText.text += letter;  // Lägg till bokstaven till UI-texten
            yield return new WaitForSeconds(typingSpeed);  // Vänta innan nästa bokstav skrivs ut
        }
    }
}
