using UnityEngine;
using UnityEngine.Events;

public class SimpleTrigger : MonoBehaviour
{
    public bool destroyOnTrigger;
    public string tagFilter;

    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagFilter))
        {
            onTriggerEnter.Invoke();

            if (destroyOnTrigger)
                Destroy(gameObject);
        }

    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tagFilter))
        {
            onTriggerExit.Invoke();

            if (destroyOnTrigger)
                Destroy(gameObject);
        }

    }
}
