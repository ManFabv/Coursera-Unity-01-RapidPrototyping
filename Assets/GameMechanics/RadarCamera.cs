using UnityEngine;

public class RadarCamera : MonoBehaviour
{
    public Transform followCamera;
    public float horizontalOffset = 1f;
    public float verticalOverride = 0f;
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 newPosition = followCamera.transform.position;
        newPosition.y = verticalOverride;
        newPosition.x += horizontalOffset;
        this.transform.position = newPosition;
	}
}
