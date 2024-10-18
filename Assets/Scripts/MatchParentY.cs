using UnityEngine;

public class MatchParentY : MonoBehaviour
{
    public float speed = 1.0f; // Speed of movement

    private Vector3 startPos;
    private Vector3 endPos;
    private bool movingToRight = true;

    void Start()
    {
        startPos = new Vector3(530f, transform.position.y, transform.position.z);
        endPos = new Vector3(540f, transform.position.y, transform.position.z);
    }

    void Update()
    {
        if (movingToRight)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);
            if (transform.position.x >= endPos.x)
            {
                movingToRight = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);
            if (transform.position.x <= startPos.x)
            {
                movingToRight = true;
            }
        }
    }

}