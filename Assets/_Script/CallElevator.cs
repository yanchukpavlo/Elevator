using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class CallElevator : MonoBehaviour
{
    [SerializeField] GameEvent @event;

    Animator animator;

    static readonly int AnimationOn = Animator.StringToHash("on");
    static readonly int AnimationOff = Animator.StringToHash("off");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out StarterAssets.ThirdPersonController controller))
        {
            animator.SetTrigger(AnimationOn);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out StarterAssets.ThirdPersonController controller))
        {
            animator.SetTrigger(AnimationOff);
        }
    }

    public void Call()
    {
        @event.Invoke();
    }
}
