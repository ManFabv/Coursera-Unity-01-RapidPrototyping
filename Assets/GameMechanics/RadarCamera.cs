using UnityEngine;
using System.Collections;

public class RadarCamera : MonoBehaviour
{
    public Transform followCamera;
    public float horizontalOffset = 1f;
    public float verticalOverride = 0f;

    private Transform localTransform;

    private void Awake()
    {
        localTransform = this.GetComponent<Transform>();

        if(followCamera == null)
        {
            Camera aux = Camera.main;

            if (aux != null)
                followCamera = aux.transform;
        }

        if (followCamera == null)
            StartCoroutine(DeactivateDelayed());
    }

    private IEnumerator DeactivateDelayed()
    {
        yield return new WaitForSeconds(0.25f);

        if (this.isActiveAndEnabled == true)
            this.enabled = false;

        StopAllCoroutines();
    }

    // Update is called once per frame
    void LateUpdate ()
    {
        if (followCamera == null)
            return;

        Vector3 newPosition = followCamera.transform.position;

        newPosition.y = verticalOverride;
        newPosition.x += horizontalOffset;

        localTransform.position = newPosition;
	}
}
