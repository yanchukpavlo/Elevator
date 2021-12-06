using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] float waitTime = 6;
    [SerializeField] float speed;
    [SerializeField] Transform[] points;
    [SerializeField] AudioSource[] sounds; 

    bool doorClosed;
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
    static readonly int AnimationDoorCloseOnly = Animator.StringToHash("closeOnly");

    private void Awake()
    {
        step = speed * Time.deltaTime;
        animator = GetComponent<Animator>();
        StartCoroutine(WaitToCloseDoor());
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
        doorClosed = false;
        animator.SetTrigger(AnimationDoorOpen);
        AudioPlay(Sound.Open);
        StartCoroutine(WaitToCloseDoor());
    }

    public void SetTarget(int target)
    {
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
                if (doorClosed) StartMove();
                else PrepareToMove();
            }
            else
            {
                if (doorClosed) OpenDoor();
            }
        }
    }

    void PrepareToMove()
    {
        StopAllCoroutines();
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
        OpenDoor();
        AudioStop(Sound.Move);
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

    void SetDoorClosed()
    {
        doorClosed = true;
    }

    IEnumerator WaitToCloseDoor()
    {
        yield return new WaitForSeconds(waitTime);
        animator.SetTrigger(AnimationDoorCloseOnly);
        AudioPlay(Sound.Close);
    }
}
