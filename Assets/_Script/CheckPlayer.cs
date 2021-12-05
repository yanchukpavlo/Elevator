using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class CheckPlayer : MonoBehaviour
{
    Elevator elevator;

    private void Awake()
    {
        elevator = GetComponentInParent<Elevator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out StarterAssets.ThirdPersonController controller))
        {
            elevator.OpenDoor();
        }
    }
}
