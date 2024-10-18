using UnityEngine;

public class Fire : MonoBehaviour
{
    public int damageAmount = 10; // Amount of damage to deal to the player per second
    public BoxCollider2D fireCollider; // The trigger collider for the fire
    public ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        fireCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // Update the collider size based on the particle system's current size
        UpdateColliderSize();
    }

    private void UpdateColliderSize()
    {
        ParticleSystem.ShapeModule shape = ps.shape;
        fireCollider.size = new Vector2(shape.scale.x, shape.scale.y);
        fireCollider.offset = new Vector2(shape.position.x, shape.position.y);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount * Time.deltaTime); // Apply damage over time
            }
        }
    }
}
