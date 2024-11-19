using UnityEngine;
using System.Collections;

public class TriggerAudioPlayWithFade : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource audioSource; // Dra in det AudioSource-objekt som du vill spela ljud från.

    [Header("Fade Settings")]
    public float fadeDuration = 1f; // Tid för fade-in och fade-out i sekunder.

    [Header("Optional")]
    public bool playOnEnter = true; // Om ljudet ska spelas när spelaren går in i triggern (standard: true)
    public bool stopOnExit = false; // Om ljudet ska stoppas när spelaren lämnar triggern (standard: false)

    [Header("Volume Control")]
    [Range(0f, 1f)] public float volume = 1f; // Volymen kan justeras via slider i Inspectorn (0 = tyst, 1 = max volym)

    private static AudioSource currentlyPlayingAudio; // Håller reda på det ljud som spelas just nu.

    private void Start()
    {
        if (audioSource != null)
        {
            // Sätt volymen på ljudkällan från början
            audioSource.volume = volume;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Kontrollera om det är spelaren som går in i triggern
        if (other.CompareTag("Player") && playOnEnter)
        {
            if (audioSource != null && !audioSource.isPlaying)
            {
                // Stoppa alla andra ljud som spelas
                if (currentlyPlayingAudio != null && currentlyPlayingAudio.isPlaying)
                {
                    StartCoroutine(FadeOut(currentlyPlayingAudio)); // Fade out det ljudet
                }

                // Starta det nya ljudet med fade in
                currentlyPlayingAudio = audioSource;
                StartCoroutine(FadeIn(audioSource)); // Fade in för det nya ljudet
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Om spelaren lämnar triggern och vi vill stoppa ljudet
        if (other.CompareTag("Player") && stopOnExit)
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                StartCoroutine(FadeOut(audioSource)); // Fade out det ljudet
            }
        }
    }

    private IEnumerator FadeIn(AudioSource audioSource)
    {
        audioSource.volume = 0f; // Börja med volymen på 0 för fade in
        audioSource.Play(); // Spela ljudet

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(0f, volume, elapsed / fadeDuration); // Öka volymen till den definierade volymen
            elapsed += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = volume; // Sätt volymen till önskad nivå när fade in är klar
    }

    private IEnumerator FadeOut(AudioSource audioSource)
    {
        float startVolume = audioSource.volume;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeDuration); // Minska volymen
            elapsed += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = 0f; // Sätt volymen till 0 när fade out är klar
        audioSource.Stop(); // Stoppa ljudet
    }

    // Metod för att justera volymen när det behövs
    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp(newVolume, 0f, 1f); // Se till att volymen är mellan 0 och 1
        if (audioSource != null)
        {
            audioSource.volume = volume; // Uppdatera volymen på ljudkällan
        }
    }
}
