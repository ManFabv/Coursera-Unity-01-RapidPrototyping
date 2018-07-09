using UnityEngine;

public class RadarScreen : MonoBehaviour
{
    public Camera radarCam;
    public int boarderSize = 2;
    private RectTransform rTransform;

	void Awake ()
    {
        rTransform = this.GetComponent<RectTransform>();

        if (rTransform == null)
        {
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
