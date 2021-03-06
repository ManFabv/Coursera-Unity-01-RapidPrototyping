using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour
{
    public int maxHealth = 1;

    internal bool isVulnerable;
    protected int health;
    internal Rigidbody2D rb;
    internal CircleCollider2D circleColl;
    internal SpriteRenderer sr;
    internal Color originalColor;

    internal float maxSize;

    void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        circleColl = this.GetComponent<CircleCollider2D>();
        sr = this.GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        
        Setup();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.CompareTo(GameplayConstants.TAG_KillZone) == 0)
        {
            GoToSleep();
        }
        else if (col.tag.CompareTo(GameplayConstants.TAG_WakeField) == 0)
        {
            WakeUp();
        }
    }

    internal virtual void Setup()
    {
        // Any additional commands at Awaken.
        SetupSize();

        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    internal void SetupSize()
    {
        maxSize = circleColl.radius * transform.localScale.x;
    }

    internal virtual void WakeUp()
    {
        isVulnerable = true;
        rb.WakeUp();
    }

    internal virtual void GoToSleep()
    {
        rb.Sleep();
        this.gameObject.SetActive(false);
    }

    public virtual void Spawn(Vector3 position)
    {
        isVulnerable = false;
        health = maxHealth;
        circleColl.isTrigger = false;
        this.transform.position = position;
        SetScaleByHealth(maxHealth);
        sr.color = originalColor;
        rb.velocity = Vector2.zero;
    }

    internal virtual void SetScaleByHealth(int currentHealth)
    {
        float healthScalar = GameplayConstants.ENEMY_SCALE * Mathf.Pow(GameplayConstants.HEALTH_SIZE_SCALAR, currentHealth - 1);
        this.transform.localScale =  healthScalar * Vector3.one;

        SetupSize();
    }
    
    public virtual int Squash()
    {
        health -= 1;
        if (health < 1)
        {
            Die();
            return 1;
        }
        else
        {
            SetScaleByHealth(health);
            StartCoroutine(Invulnerable());
            return 0;
        }
    }

    internal IEnumerator Invulnerable()
    {
        isVulnerable = false;
        yield return new WaitForSeconds(0.5f);
        isVulnerable = true;
    }

    public virtual void Die()
    {
        sr.color = Color.black;
        isVulnerable = false;
        rb.velocity = 10f * Vector2.up;
        StartCoroutine(DelayedDeath());
    }

    internal IEnumerator DelayedDeath()
    {
        // Give the player a chance to jump off.
        yield return new WaitForSeconds(0.5f);
        circleColl.isTrigger = true;
    }
}
