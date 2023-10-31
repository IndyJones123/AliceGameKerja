using UnityEngine;

public class ShootingCharacter : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab of the projectile to be shot
    public float shootingForce = 10f; // Force applied to the projectiles when shot

    void Update()
    {
        // Check for player input to shoot
        if (Input.GetMouseButtonDown(0)) // 0 represents the left mouse button
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Get the player's camera
        Camera playerCamera = Camera.main;

        // Calculate the spawn position at the center of the camera
        Vector3 spawnPosition = playerCamera.transform.position + playerCamera.transform.forward;

        // Instantiate a new projectile from the prefab at the calculated spawn position
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, playerCamera.transform.rotation);

        // Apply a forward force to the projectile
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.velocity = projectile.transform.forward * shootingForce;
    }
}
