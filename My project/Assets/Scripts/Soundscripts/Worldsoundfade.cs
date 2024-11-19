using UnityEngine;
using System.Collections;

public class WorldAudioFadeControl : MonoBehaviour
{
    [Header("World Audio Settings")]
    public AudioSource worldAudio; // Dra in din v�rldsljud (ambient-ljudet) h�r i Inspector.

    [Header("Fade Settings")]
    public float fadeDuration = 1f; // Tid f�r fade-effekten i sekunder.
    public float minVolume = 0.05f; // L�gsta volymen ljudet kan fadea till (t.ex. 0.05 ist�llet f�r 0).

    private float originalVolume; // F�r att lagra den ursprungliga volymen.
    private bool isFadedOut = false; // H�ller reda p� om ljudet �r fadeat ut.

    private void Start()
    {
        // Spara den ursprungliga volymen n�r spelet startar
        originalVolume = worldAudio.volume;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Kontrollera om det �r spelaren som g�r in i triggern
        if (other.CompareTag("Player"))
        {
            if (!isFadedOut)
            {
                // Fadea ut v�rldsljudet n�r spelaren g�r in i rummet f�rsta g�ngen
                StartCoroutine(FadeOut(worldAudio));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Kontrollera om det �r spelaren som l�mnar triggern
        if (other.CompareTag("Player"))
        {
            if (isFadedOut)
            {
                // Fadea tillbaka v�rldsljudet till normalt n�r spelaren g�r ut ur rummet
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
            audioSource.volume = Mathf.Lerp(minVolume, originalVolume, elapsed / fadeDuration); // �ka volymen gradvis
            elapsed += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = originalVolume; // S�tt volymen till max n�r fade in �r klar
        isFadedOut = false; // S�tt flaggan till false, ljudet �r nu inte fadeat ut
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

        audioSource.volume = minVolume; // S�tt volymen till minVolume n�r fade out �r klar
        isFadedOut = true; // S�tt flaggan till true, ljudet �r nu fadeat ut
    }
}
