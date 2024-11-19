using UnityEngine;

public class MyDoor : MonoBehaviour
{
    public bool closed = true;
    public bool mirror = false;
    public float openDegrees = 90f;
    public float openSpeed = 60f;

    // Private variables
    float closedDegrees;

    float closedOffset;
    float openedOffset;
    float currentOffset;

    void Start()
    {
        if (mirror)
            openedOffset = -openDegrees;
        else
            openedOffset = openDegrees;

        closedDegrees = transform.localEulerAngles.y;
    }


    void Update()
    {
        if (closed)
            currentOffset = Mathf.MoveTowards(currentOffset, closedOffset, openSpeed * Time.deltaTime);
        else
            currentOffset = Mathf.MoveTowards(currentOffset, openedOffset, openSpeed * Time.deltaTime);

        transform.localRotation = Quaternion.Euler(new Vector3(0f, currentOffset, 0f));
    }

    public void ToggleOpen()
    {
        closed = !closed;
    }

}
