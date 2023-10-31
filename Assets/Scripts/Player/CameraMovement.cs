using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;  // the player's transform
    public float distance = 10.0f;  // the distance between the camera and the player
    public float height = 5.0f;  // the height offset between the camera and the player
    public float smoothSpeed = 0.5f;  // the speed of camera movement

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        // calculate the camera's target position based on the player's position and the camera's offset
        Vector3 targetPosition = target.position + Vector3.up * height - target.forward * distance;

        // smoothly move the camera towards the target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothSpeed);

        // make the camera look at the player
        transform.LookAt(target);
    }
}