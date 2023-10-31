using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 2f; // Lifetime of the projectile in seconds

    void Start()
    {
        // Start the countdown to destroy the projectile after the specified lifetime
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the projectile collides with another object
        // You can further refine this condition based on the specific objects you want the projectile to collide with

        // Destroy the projectile when it collides with another object
        Destroy(gameObject);
    }
}
