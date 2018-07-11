using UnityEngine;

public class Walker : Enemy
{
    public float speed = 1f;

    [SerializeField]
    internal LayerMask mask = 1;

    internal bool movingRight;
    
    internal override void WakeUp()
    {
        base.WakeUp();
        movingRight = Random.Range(0, 2) < 1 ? true : false;
    }

    void FixedUpdate()
    {
        if (isVulnerable)
        {
            LookAhead();
            rb.velocity = new Vector2(speed * GetDirection(), rb.velocity.y);
        }
    }

    internal virtual void LookAhead()
    {
        this.circleColl.enabled = false; //deactivate collider to avoid self collision

        RaycastHit2D rayHit = Physics2D.Raycast(this.transform.position + GetDirection() * (maxSize+0.2f) * Vector3.right, Vector3.down * (maxSize+0.2f), maxSize*2.0f, mask);

        this.circleColl.enabled = true; //activate again collider

        if (rayHit.collider == null)
        {
            // Don't fall!
            ChangeDirection();
        }

        else
        {
            this.circleColl.enabled = false; //deactivate collider to avoid self collision

            rayHit = Physics2D.Raycast(this.transform.position, GetDirection() * Vector3.right * (maxSize+0.2f), (maxSize+0.2f), mask);

            this.circleColl.enabled = true; //activate again collider

            if (rayHit.collider != null)
            {
                // Don't run into walls!
                ChangeDirection();
            }
        }
    }

    internal void ChangeDirection()
    {
        movingRight = !movingRight;
    }

    internal float GetDirection()
    {
        return (movingRight ? 1f : -1f);
    }

    internal override void Setup()
    {
        base.Setup();

        Physics2D.IgnoreCollision(circleColl, circleColl, true);
    }
}
