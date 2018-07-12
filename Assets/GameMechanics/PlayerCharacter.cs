using UnityEngine;
using UnityStandardAssets._2D;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(PlatformerCharacter2D))]
[RequireComponent(typeof(Platformer2DUserControl))]
public class PlayerCharacter : MonoBehaviour
{
    public HUD hud;

    private Rigidbody2D rb;
    private Transform localTransform;

    private int lives = GameplayConstants.STARTING_LIVES;
    private int distanceScore = 0;
    private int enemyScore = 0;

    private Vector2 spawnPosition = Vector2.zero;

    private float spawnHeight = 10;

    private GameObject targetFollow;

    private ManageMovement manageMovement;

    public Camera2DFollow cameraFollow;

    private float maxSize;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = this.GetComponent<SpriteRenderer>();

        localTransform = this.GetComponent<Transform>();

        rb = this.GetComponent<Rigidbody2D>();

        manageMovement = this.GetComponentInChildren<ManageMovement>();

        targetFollow = GameObject.FindGameObjectWithTag(GameplayConstants.TAG_CAMERA_FOLLOW);

        if (cameraFollow == null)
            cameraFollow = GameObject.FindObjectOfType<Camera2DFollow>();

        maxSize = sr.bounds.max.x;
    }

    void Start ()
    {
        spawnPosition = rb.position;

        SpriteRenderer sp = this.GetComponent<SpriteRenderer>();

        spawnHeight = sp.bounds.extents.y * GameplayConstants.SPAWN_POS_Y_MULTIPLIER;

        SetInitialHUD();
    }

    private void SetInitialHUD()
    {
        if (hud != null)
        {
            hud.UpdateLives(lives);
            hud.UpdateEnemies(enemyScore);
            hud.UpdateScore(0);
        }
    }

    void Update()
    {
        distanceScore = Mathf.Max(distanceScore, (int)localTransform.position.x);

        int totalScore = distanceScore * GameplayConstants.SCORE_DISTANCE_MULTIPLIER + enemyScore * GameplayConstants.SCORE_ENEMY_MULTIPLIER;

        hud.UpdateScore(totalScore);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        UpdateSpawnPosition(col);

        if (col.gameObject.tag.CompareTo(GameplayConstants.TAG_Enemy) == 0)
        {
            Vector3 enemyPos = col.collider.bounds.center;

            float enemyWidth = col.collider.bounds.extents.x;

            if (enemyPos.y < localTransform.position.y && Mathf.Abs(enemyPos.x - localTransform.position.x) < enemyWidth)
            {
                Enemy enemy = col.gameObject.GetComponent<Enemy>();

                if (enemy != null && hud != null)
                {
                    enemyScore += enemy.Squash();

                    hud.UpdateEnemies(enemyScore);
                }
            }
        }
    }

    private void UpdateSpawnPosition(Collision2D col)
    {
        if (col.gameObject.tag.CompareTo(GameplayConstants.TAG_Ground) == 0)
        {
            RectTransform rect_aux = col.gameObject.GetComponent<RectTransform>();

            if (rect_aux != null)
            {
                Vector2 posCol = col.transform.position;

                if (cameraFollow != null)
                {
                    float screenLeftMargin = cameraFollow.MinimumDistance;

                    posCol.x = Mathf.Max(posCol.x, screenLeftMargin) + maxSize;
                }
                
                posCol.y += rect_aux.rect.yMin;

                spawnPosition = new Vector2(posCol.x, posCol.y + GameplayConstants.SPAWN_POS_Y_MULTIPLIER);
            }

            else
            {
                Vector2 posCol = col.transform.position;

                Collider2D col_aux = col.gameObject.GetComponent<Collider2D>();

                if (col_aux != null)
                {
                    float alturaPlataforma = col_aux.bounds.size.y;

                    spawnPosition = new Vector2(posCol.x, posCol.y + alturaPlataforma);
                }

                else
                    spawnPosition = posCol;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.CompareTo(GameplayConstants.TAG_KillZone) == 0)
        {
            KillCharacter();
        }
    }

    private void KillCharacter()
    {
        lives -= 1;

        if(hud != null)
            hud.UpdateLives(lives);

        if (lives > 0)
        {
            LoaderManager.Instance.StartLoader();

            rb.MovePosition(spawnPosition + (spawnHeight * Vector2.up));

            rb.velocity = Vector2.zero;

            if (cameraFollow != null)
                cameraFollow.PlayerLostLife();
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

        if (manageMovement != null)
            manageMovement.GameOver();

        if (hud != null)
            hud.GameOver();

        if (targetFollow != null)
        {
            Vector3 posicion = new Vector3(spawnPosition.x, spawnPosition.y + spawnHeight, targetFollow.transform.position.z);
            Quaternion rotacion = Quaternion.identity;

            targetFollow.transform.SetPositionAndRotation(posicion, rotacion);
        }

        GameObject.Destroy(this.gameObject);
    }

    public void StopMovement(bool horizontalAndVertical)
    {
        if (horizontalAndVertical)
            rb.velocity = Vector2.zero;
        else
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
    }
}
