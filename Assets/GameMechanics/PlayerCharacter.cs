using UnityEngine;
using UnityStandardAssets._2D;

public class PlayerCharacter : MonoBehaviour
{
    public HUD hud;

    private Rigidbody2D rb;

    private int lives = GameplayConstants.STARTING_LIVES;
    private int distanceScore = 0;
    private int enemyScore = 0;
    //private PlatformerCharacter2D platformCharacter;
	
	void Start ()
    {
        rb = this.GetComponent<Rigidbody2D>();
        //platformCharacter = this.GetComponent<PlatformerCharacter2D>();

        SetInitialHUD();
    }

    private void SetInitialHUD()
    {
        hud.UpdateLives(lives);
        hud.UpdateEnemies(enemyScore);
        hud.UpdateScore(0);
    }

    void Update()
    {
        distanceScore = Mathf.Max(distanceScore, (int)this.transform.position.x);
        int totalScore = distanceScore * GameplayConstants.SCORE_DISTANCE_MULTIPLIER + enemyScore * GameplayConstants.SCORE_ENEMY_MULTIPLIER;

        hud.UpdateScore(totalScore);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == GameplayConstants.TAG_Enemy)
        {
            Vector3 enemyPos = col.collider.bounds.center;
            float enemyWidth = col.collider.bounds.extents.x;
            if (enemyPos.y < this.transform.position.y && Mathf.Abs(enemyPos.x - this.transform.position.x) < enemyWidth)
            {
                Enemy enemy = col.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemyScore += enemy.Squash();
                    hud.UpdateEnemies(enemyScore);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == GameplayConstants.TAG_KillZone)
        {
            KillCharacter();
        }
    }

    private void KillCharacter()
    {
        lives -= 1;
        hud.UpdateLives(lives);

        if (lives > 0)
        {
            rb.MovePosition(rb.position + GameplayConstants.RESPAWN_HEIGHT * Vector2.up);
            rb.velocity = Vector2.zero;
        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        Debug.Log("Game Over!");
    }
}
