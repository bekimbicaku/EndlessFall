using UnityEngine;

public class ParticleCollisionHandler : MonoBehaviour
{
    
    public int takeDmg;
    
    public enum ObstacleType { Bullet, FireParticle, Water, Smoke, Lighten }
    public ObstacleType obstacleType;

    private bool hasPlayedSound = false;
    void OnParticleCollision(GameObject other)
    {

        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (!hasPlayedSound)
            {
                // Start the lighten sound when the player enters the collider and only play it once
                switch (obstacleType)
                {
                    case ObstacleType.Bullet:
                        break;
                    case ObstacleType.FireParticle:
                        SoundManager.instance.PlaySFX("Fire");
                        break;
                    case ObstacleType.Water:
                        SoundManager.instance.PlaySFX("Water");
                        break;
                    case ObstacleType.Smoke:
                        SoundManager.instance.PlaySFX("Smoke");
                        break;
                    case ObstacleType.Lighten:
                        SoundManager.instance.PlaySFX("Electricity");
                        break;
                }
                hasPlayedSound = true;
            }
            health.TakeDamage(takeDmg);

        }
    }

    void OnParticleSystemStopped()
    {
        hasPlayedSound = false; // Reset the flag when the particle system stops
    }

}
