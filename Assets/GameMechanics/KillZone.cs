using UnityEngine;

public class KillZone : MonoBehaviour
{
    public Transform mainCamera;
    public float killDepth = -10f;

    private Transform localTransform;

    private void Awake()
    {
        localTransform = this.GetComponent<Transform>();

        if (mainCamera == null)
        {
            Camera cam_aux = Camera.main;

            if (cam_aux != null)
                mainCamera = cam_aux.transform;
        }
    }

    void LateUpdate ()
    {
        if (mainCamera != null)
        {
            Vector3 newPosition = mainCamera.position;

            newPosition.y = killDepth;

            localTransform.position = newPosition;
        }
	}
}