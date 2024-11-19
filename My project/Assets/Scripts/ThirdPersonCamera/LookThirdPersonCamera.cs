using UnityEngine;

public class Lock3rdPerson : MonoBehaviour
{
    public Transform camerasFocus;
    public Vector3 offset;

    void Start()
    {

    }

    void Update()
    {
        transform.LookAt(camerasFocus);
        transform.position = Vector3.Lerp(transform.position, camerasFocus.position + offset, 0.1f * Time.deltaTime);
    }
}