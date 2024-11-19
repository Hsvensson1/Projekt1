using UnityEngine;
using System.Collections;

public class RoomTriggerAudio : MonoBehaviour
{
    [Header("Player Settings")]
    public GameObject player; // Dra in ditt Player GameObject här.

    [Header("Audio Sources")]
    public AudioSource worldAudio;  // Bakgrundsljud för världen.
    public AudioSource room1Audio; // Bakgrundsljud för rum 1.
    public AudioSource room2Audio; // Bakgrundsljud för rum 2.

    [Header("Fade Settings")]
    public float fadeDuration = 1f; // Tid för fade-in/out i sekunder.
    public float worldVolume = 0.2f; // Maxvolym för världsljudet.
    public float room1Volume = 0.5f; // Maxvolym för rum 1-ljudet.
    public float room2Volume = 0.5f; // Maxvolym för rum 2-ljudet.

    private AudioSource currentRoomAudio; // Håller reda på det ljud som spelas just nu.
    private Coroutine fadeCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        // Kontrollera att det är spelaren som triggar
        if (other.gameObject == player)
        {
            if (this.name == "Room1Trigger" && room1Audio != null)
            {
                PlayRoomAudio(room1Audio, room1Volume);
            }
            else if (this.name == "Room2Trigger" && room2Audio != null)
            {
                PlayRoomAudio(room2Audio, room2Volume);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Kontrollera att det är spelaren som triggar
        if (other.gameObject == player)
        {
            if (this.name == "Room1Trigger" && room1Audio != null)
            {
                StopRoomAudio(room1Audio);
                ResumeWorldAudio();
            }
            else if (this.name == "Room2Trigger" && room2Audio != null)
            {
                StopRoomAudio(room2Audio);
                ResumeWorldAudio();
            }
        }
    }

    private void PlayRoomAudio(AudioSource roomAudio, float targetVolume)
    {
        if (currentRoomAudio != null && currentRoomAudio != roomAudio)
        {
            StartFade(currentRoomAudio, 0f); // Fadea ut nuvarande rums-ljud.
        }

        currentRoomAudio = roomAudio; // Sätt det nya rums-ljudet som aktivt.
        StartFade(worldAudio, 0f); // Fadea ut världsljudet.
        StartFade(roomAudio, targetVolume); // Fadea in rums-ljudet.
    }

    private void StopRoomAudio(AudioSource roomAudio)
    {
        if (currentRoomAudio == roomAudio)
        {
            StartFade(roomAudio, 0f); // Fadea ut rums-ljudet.
            currentRoomAudio = null; // Ingen aktiv rums-ljud längre.
        }
    }

    private void ResumeWorldAudio()
    {
        if (worldAudio != null)
        {
            StartFade(worldAudio, worldVolume); // Fadea in världsljudet.
        }
    }

    private void StartFade(AudioSource audioSource, float targetVolume)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeAudio(audioSource, targetVolume));
    }

    private IEnumerator FadeAudio(AudioSource audioSource, float targetVolume)
    {
        float startVolume = audioSource.volume;
        float elapsed = 0f;

        if (targetVolume > 0f && !audioSource.isPlaying)
        {
            audioSource.Play();
        }

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / fadeDuration);
            yield return null;
        }

        audioSource.volume = targetVolume;

        if (targetVolume == 0f && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
