using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;
    public Vector3 cameraOffset;

    void Update()
    {
        transform.position = player.position + cameraOffset;
    }
}
