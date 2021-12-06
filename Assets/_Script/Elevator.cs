using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Transform[] points;
    [SerializeField] AudioSource[] sounds; 

    bool canSetup = true;
    bool move;
    float step;
    Transform currentTarget;
    Vector3 direction;
    Vector3 tempVector;
    Animator animator;

    enum Sound
    {
        Start,
        Move,
        Open,
        Close
    }

    static readonly int AnimationDoorOpen = Animator.StringToHash("open");
    static readonly int AnimationDoorClose = Animator.StringToHash("close");

    private void Awake()
    {
        step = speed * Time.deltaTime;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (move)
        {
            tempVector = currentTarget.position - transform.position;
            if (tempVector.magnitude < step)
            {
                EndMove();
                return;
            }

            Move();
        }
    }

    public void OpenDoor()
    {
        animator.SetTrigger(AnimationDoorOpen);
    }

    public void SetTarget(int target)
    {
        Debug.Log("Try set new target.");
        if (canSetup)
        {
            if (points[target] == null)
            {
                Debug.LogError("New target can`t be NULL.");
                Debug.Break();
            }

            currentTarget = points[target];

            tempVector = currentTarget.position - transform.position;
            if (tempVector.magnitude > step)
            {
                direction = currentTarget.position - transform.position;
                direction = direction.normalized;
                PrepareToMove();
            }
        }
    }

    void PrepareToMove()
    {
        canSetup = false;
        animator.SetTrigger(AnimationDoorClose);
        AudioPlay(Sound.Close);
    }

    void StartMove()
    {
        move = true;
        AudioPlay(Sound.Start);
        AudioPlay(Sound.Move);
    }

    void Move()
    {
        transform.Translate(direction * step, Space.World);
    }

    void EndMove()
    {
        move = false;
        AudioStop(Sound.Move);
        AudioPlay(Sound.Open);
        animator.SetTrigger(AnimationDoorOpen);
    }

    void AudioPlay(Sound sound)
    {
        sounds[(int)sound].Play();
    }

    void AudioStop(Sound sound)
    {
        sounds[(int)sound].Stop();
    }

    void SetSetupTrue()
    {
        canSetup = true;
    }
}
