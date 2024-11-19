using UnityEngine;
using System.Collections;

public class TriggerAudioPlayWithFade : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource audioSource; // Dra in det AudioSource-objekt som du vill spela ljud fr�n.

    [Header("Fade Settings")]
    public float fadeDuration = 1f; // Tid f�r fade-in och fade-out i sekunder.

    [Header("Optional")]
    public bool playOnEnter = true; // Om ljudet ska spelas n�r spelaren g�r in i triggern (standard: true)
    public bool stopOnExit = false; // Om ljudet ska stoppas n�r spelaren l�mnar triggern (standard: false)

    [Header("Volume Control")]
    [Range(0f, 1f)] public float volume = 1f; // Volymen kan justeras via slider i Inspectorn (0 = tyst, 1 = max volym)

    private static AudioSource currentlyPlayingAudio; // H�ller reda p� det ljud som spelas just nu.

    private void Start()
    {
        if (audioSource != null)
        {
            // S�tt volymen p� ljudk�llan fr�n b�rjan
            audioSource.volume = volume;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Kontrollera om det �r spelaren som g�r in i triggern
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
                StartCoroutine(FadeIn(audioSource)); // Fade in f�r det nya ljudet
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Om spelaren l�mnar triggern och vi vill stoppa ljudet
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
        audioSource.volume = 0f; // B�rja med volymen p� 0 f�r fade in
        audioSource.Play(); // Spela ljudet

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(0f, volume, elapsed / fadeDuration); // �ka volymen till den definierade volymen
            elapsed += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = volume; // S�tt volymen till �nskad niv� n�r fade in �r klar
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

        audioSource.volume = 0f; // S�tt volymen till 0 n�r fade out �r klar
        audioSource.Stop(); // Stoppa ljudet
    }

    // Metod f�r att justera volymen n�r det beh�vs
    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp(newVolume, 0f, 1f); // Se till att volymen �r mellan 0 och 1
        if (audioSource != null)
        {
            audioSource.volume = volume; // Uppdatera volymen p� ljudk�llan
        }
    }
}
