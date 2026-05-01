using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    Animator animator;
    public GameObject refresh;

    void Start()
    {
        animator = refresh.GetComponent<Animator>();
    }

    public void Play()
    {
        animator.Play("Refresh"); // Name of the animation state
    }
}
