using UnityEngine;

public class MirrorCamera : MonoBehaviour
{
    public Camera playerCamera;  // FPS-kamera (spelarkamera)
    public Transform mirrorPlane; // Spegelplanet (t.ex. ett plane eller wall object)

    void Update()
    {
        // Hämta spegelplanet och dess normal (det plan som reflekterar)
        Vector3 planeNormal = mirrorPlane.forward;
        Vector3 planePosition = mirrorPlane.position;

        // Beräkna reflekterad position för spegelkameran
        Vector3 reflectedPosition = ReflectPosition(playerCamera.transform.position, planePosition, planeNormal);

        // Uppdatera spegelkamerans position
        transform.position = reflectedPosition;

        // Reflektera spelarens rotation (inklusive spegelrotation)
        Quaternion reflectedRotation = ReflectRotation(playerCamera.transform.rotation, planeNormal);
        transform.rotation = reflectedRotation;
    }

    // Metod för att spegelvända positionen relativt en plan yta
    private Vector3 ReflectPosition(Vector3 originalPosition, Vector3 planePosition, Vector3 planeNormal)
    {
        // Beräkna vektorn mellan objektet och planet
        Vector3 toPlane = originalPosition - planePosition;

        // Reflektera vektorn över planet
        float distance = Vector3.Dot(toPlane, planeNormal);
        return originalPosition - 2f * distance * planeNormal;
    }

    // Metod för att spegelvända rotationen
    private Quaternion ReflectRotation(Quaternion originalRotation, Vector3 planeNormal)
    {
        // Reflektera rotationen över planet
        Quaternion rotation = originalRotation;

        // För att spegelvända rotationen på planet
        rotation = Quaternion.Inverse(Quaternion.LookRotation(planeNormal)) * rotation;

        return rotation;
    }
}
