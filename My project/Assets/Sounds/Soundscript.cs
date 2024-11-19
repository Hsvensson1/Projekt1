using UnityEngine;
using System.Collections;

public class RoomTriggerAudio : MonoBehaviour
{
    [Header("Player Settings")]
    public GameObject player; // Dra in ditt Player GameObject h�r.

    [Header("Audio Sources")]
    public AudioSource worldAudio;  // Bakgrundsljud f�r v�rlden.
    public AudioSource room1Audio; // Bakgrundsljud f�r rum 1.
    public AudioSource room2Audio; // Bakgrundsljud f�r rum 2.

    [Header("Fade Settings")]
    public float fadeDuration = 1f; // Tid f�r fade-in/out i sekunder.
    public float worldVolume = 0.2f; // Maxvolym f�r v�rldsljudet.
    public float room1Volume = 0.5f; // Maxvolym f�r rum 1-ljudet.
    public float room2Volume = 0.5f; // Maxvolym f�r rum 2-ljudet.

    private AudioSource currentRoomAudio; // H�ller reda p� det ljud som spelas just nu.
    private Coroutine fadeCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        // Kontrollera att det �r spelaren som triggar
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
        // Kontrollera att det �r spelaren som triggar
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

        currentRoomAudio = roomAudio; // S�tt det nya rums-ljudet som aktivt.
        StartFade(worldAudio, 0f); // Fadea ut v�rldsljudet.
        StartFade(roomAudio, targetVolume); // Fadea in rums-ljudet.
    }

    private void StopRoomAudio(AudioSource roomAudio)
    {
        if (currentRoomAudio == roomAudio)
        {
            StartFade(roomAudio, 0f); // Fadea ut rums-ljudet.
            currentRoomAudio = null; // Ingen aktiv rums-ljud l�ngre.
        }
    }

    private void ResumeWorldAudio()
    {
        if (worldAudio != null)
        {
            StartFade(worldAudio, worldVolume); // Fadea in v�rldsljudet.
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
