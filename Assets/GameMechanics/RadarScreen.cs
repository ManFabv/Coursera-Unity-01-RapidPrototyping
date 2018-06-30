using UnityEngine;
using UnityEngine.UI;

public class RadarScreen : MonoBehaviour
{
    public Camera radarCam;
    public int boarderSize = 2;
    private RectTransform rTransform;

	// Use this for initialization
	void Awake ()
    {
        rTransform = this.GetComponent<RectTransform>();

        if (rTransform == null)
        {
            Debug.LogWarning("No radar rect!");
            this.enabled = false;
        }
        else
        {
            SizeRect();
        }
	}

    void SizeRect()
    {
        rTransform.sizeDelta = new Vector2(radarCam.scaledPixelWidth + boarderSize, radarCam.scaledPixelHeight + boarderSize);
    }
}
