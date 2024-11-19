using UnityEngine;

public class MirrorCamera : MonoBehaviour
{
    public Camera playerCamera;  // FPS-kamera (spelarkamera)
    public Transform mirrorPlane; // Spegelplanet (t.ex. ett plane eller wall object)

    void Update()
    {
        // H�mta spegelplanet och dess normal (det plan som reflekterar)
        Vector3 planeNormal = mirrorPlane.forward;
        Vector3 planePosition = mirrorPlane.position;

        // Ber�kna reflekterad position f�r spegelkameran
        Vector3 reflectedPosition = ReflectPosition(playerCamera.transform.position, planePosition, planeNormal);

        // Uppdatera spegelkamerans position
        transform.position = reflectedPosition;

        // Reflektera spelarens rotation (inklusive spegelrotation)
        Quaternion reflectedRotation = ReflectRotation(playerCamera.transform.rotation, planeNormal);
        transform.rotation = reflectedRotation;
    }

    // Metod f�r att spegelv�nda positionen relativt en plan yta
    private Vector3 ReflectPosition(Vector3 originalPosition, Vector3 planePosition, Vector3 planeNormal)
    {
        // Ber�kna vektorn mellan objektet och planet
        Vector3 toPlane = originalPosition - planePosition;

        // Reflektera vektorn �ver planet
        float distance = Vector3.Dot(toPlane, planeNormal);
        return originalPosition - 2f * distance * planeNormal;
    }

    // Metod f�r att spegelv�nda rotationen
    private Quaternion ReflectRotation(Quaternion originalRotation, Vector3 planeNormal)
    {
        // Reflektera rotationen �ver planet
        Quaternion rotation = originalRotation;

        // F�r att spegelv�nda rotationen p� planet
        rotation = Quaternion.Inverse(Quaternion.LookRotation(planeNormal)) * rotation;

        return rotation;
    }
}
