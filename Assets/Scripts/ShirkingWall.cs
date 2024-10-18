using UnityEngine;

public class ShrinkingWall : MonoBehaviour
{
    public Vector3 minScale = new Vector3(1, 1, 1); // Shkalla minimale
    public Vector3 maxScale = new Vector3(2, 2, 2); // Shkalla maksimale
    public float shrinkSpeed = 1f; // Shpejtësia e ngushtimit

    private bool isShrinking = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerTrigger"))
        {
            isShrinking = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerTrigger"))
        {
            isShrinking = false;
        }
    }
    void Update()
    {
        if (isShrinking)
        {
            // Llogarit shkallën e re
            transform.localScale = Vector3.Lerp(transform.localScale, maxScale, shrinkSpeed * Time.deltaTime);

            // Ndalon zgjerimin kur shkalla maksimale është arritur
            if (Vector3.Distance(transform.localScale, maxScale) < 0.01f)
            {
                isShrinking = true;
            }
        }
        
    }
}
