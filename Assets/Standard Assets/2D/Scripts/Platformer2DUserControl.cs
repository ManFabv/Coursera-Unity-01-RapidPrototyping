using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;
        private bool crouch;

        private float horizontalMovement;

        public bool CanMoveForward
        {
            get;
            set;
        }

        private void Awake()
        {
            CanMoveForward = true;

            m_Character = GetComponent<PlatformerCharacter2D>();
        }

        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            crouch = Input.GetKey(KeyCode.LeftControl);

            horizontalMovement = CrossPlatformInputManager.GetAxis("Horizontal");

            if (CanMoveForward == false)
            {
                if (m_Character.GetDirection() == horizontalMovement)
                    horizontalMovement = 0;
            }
        }

        private void FixedUpdate()
        {
            // Pass all parameters to the character control script.
            m_Character.Move(horizontalMovement, crouch, m_Jump);
            m_Jump = false;
        }
    }
}
