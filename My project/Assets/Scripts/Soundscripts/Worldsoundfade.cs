using UnityEngine;
using System.Collections;

public class WorldAudioFadeControl : MonoBehaviour
{
    [Header("World Audio Settings")]
    public AudioSource worldAudio; // Dra in din världsljud (ambient-ljudet) här i Inspector.

    [Header("Fade Settings")]
    public float fadeDuration = 1f; // Tid för fade-effekten i sekunder.
    public float minVolume = 0.05f; // Lägsta volymen ljudet kan fadea till (t.ex. 0.05 istället för 0).

    private float originalVolume; // För att lagra den ursprungliga volymen.
    private bool isFadedOut = false; // Håller reda på om ljudet är fadeat ut.

    private void Start()
    {
        // Spara den ursprungliga volymen när spelet startar
        originalVolume = worldAudio.volume;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Kontrollera om det är spelaren som går in i triggern
        if (other.CompareTag("Player"))
        {
            if (!isFadedOut)
            {
                // Fadea ut världsljudet när spelaren går in i rummet första gången
                StartCoroutine(FadeOut(worldAudio));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Kontrollera om det är spelaren som lämnar triggern
        if (other.CompareTag("Player"))
        {
            if (isFadedOut)
            {
                // Fadea tillbaka världsljudet till normalt när spelaren går ut ur rummet
                StartCoroutine(FadeIn(worldAudio));
            }
        }
    }

    private IEnumerator FadeIn(AudioSource audioSource)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play(); // Spela ljudet om det inte redan spelas
        }

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(minVolume, originalVolume, elapsed / fadeDuration); // Öka volymen gradvis
            elapsed += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = originalVolume; // Sätt volymen till max när fade in är klar
        isFadedOut = false; // Sätt flaggan till false, ljudet är nu inte fadeat ut
    }

    private IEnumerator FadeOut(AudioSource audioSource)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(originalVolume, minVolume, elapsed / fadeDuration); // Minska volymen gradvis
            elapsed += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = minVolume; // Sätt volymen till minVolume när fade out är klar
        isFadedOut = true; // Sätt flaggan till true, ljudet är nu fadeat ut
    }
}
