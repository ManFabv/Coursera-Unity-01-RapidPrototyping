using UnityEngine;
using UnityStandardAssets._2D;

[RequireComponent(typeof(BoxCollider2D))]
public class ManageMovement : MonoBehaviour
{
    private Platformer2DUserControl userControl;

    private PlayerCharacter playerCharacter;

    private void Awake()
    {
        playerCharacter = GameObject.FindObjectOfType<PlayerCharacter>();

        if(this.transform.parent != null)
            userControl = this.transform.parent.GetComponent<Platformer2DUserControl>();

        if (userControl == null)
            userControl = GameObject.FindObjectOfType<Platformer2DUserControl>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.CompareTo(GameplayConstants.TAG_Ground) == 0 || collision.gameObject.layer == GameplayConstants.LAYER_CAMERA_MARGIN)
        {
            if (userControl != null)
                userControl.CanMoveForward = false;

            if (playerCharacter != null)
                playerCharacter.StopMovement(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.CompareTo(GameplayConstants.TAG_Ground) == 0)
        {
            if (userControl != null)
                userControl.CanMoveForward = true;
        }
    }

    public void GameOver()
    {
        if(this.isActiveAndEnabled == true)
            this.enabled = false;
    }
}