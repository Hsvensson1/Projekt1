using UnityEngine;
using UnityEngine.Events;

public class MyButton : MonoBehaviour
{

    public new Transform camera;
    public float reach = 4f;

    public UnityEvent onPressed;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            bool pressed = TryPress();

            if (pressed)
                onPressed.Invoke();
        }
    }

    bool TryPress()
    {
        RaycastHit hitInfo;
        Ray ray = new Ray(camera.position, camera.forward);

        if (!Physics.Raycast(ray, out hitInfo, reach))
            return false;

        if (hitInfo.collider != gameObject.GetComponent<Collider>())
            return false;

        return true;
    }
}
