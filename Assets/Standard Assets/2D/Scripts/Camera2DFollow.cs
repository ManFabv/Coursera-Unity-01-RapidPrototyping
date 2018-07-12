using UnityEngine;
using System.Collections;

namespace UnityStandardAssets._2D
{
    public class Camera2DFollow : MonoBehaviour
    {
        public Transform target;
        public float damping = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;
        public float minimumHeight = -10f;

        private float m_OffsetZ;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;
        private float minimumDistance;

        private bool mustFollow = true;

        public float MinimumDistance
        {
            get
            {
                minimumDistance = Mathf.Max(transform.position.x, minimumDistance);

                return minimumDistance;
            }
        }

        // Use this for initialization
        private void Start()
        {
            mustFollow = true;

            if(target == null)
            {
                GameObject aux_go = GameObject.FindGameObjectWithTag("Player");

                if (aux_go != null)
                    target = aux_go.transform;
            }

            if (target != null)
            {
                m_LastTargetPosition = target.position;
                m_OffsetZ = (transform.position - target.position).z;
                transform.parent = null;
            }

            else
            {
                StartCoroutine(DeactivateDelayed());
            }
        }

        private IEnumerator DeactivateDelayed()
        {
            yield return new WaitForSeconds(0.25f);

            if (this.isActiveAndEnabled == true)
                this.enabled = false;

            StopAllCoroutines();
        }

        public void PlayerLostLife()
        {
            if(target != null)
                StartCoroutine(GoToTarget());
        }

        private IEnumerator GoToTarget()
        {
            mustFollow = false;

            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            this.transform.position = new Vector3(target.position.x, target.position.y, this.transform.position.z);

            minimumDistance = this.transform.position.x;

            yield return new WaitForEndOfFrame();

            mustFollow = true;
        }

        // Update is called once per frame
        private void LateUpdate()
        {
            if (target == null)
                return;

            if (mustFollow == true)
            {
                minimumDistance = Mathf.Max(transform.position.x, minimumDistance);

                // only update lookahead pos if accelerating or changed direction
                float xMoveDelta = (target.position - m_LastTargetPosition).x;

                bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

                if (updateLookAheadTarget)
                {
                    m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
                }

                else
                {
                    m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
                }

                Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward * m_OffsetZ;
                Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

                newPos.x = Mathf.Max(newPos.x, minimumDistance);
                newPos.y = Mathf.Max(newPos.y, minimumHeight);

                transform.position = newPos;

                m_LastTargetPosition = target.position;
            }
        }
    }
}