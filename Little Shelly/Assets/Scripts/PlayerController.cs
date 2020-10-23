using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerMaxHealth = 4;
    public int playerCurrentHealth;
    public HealthBar healthBar;

    public float speed = 5.0f;
    public float jumpSpeed = 6.0f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public ParticleSystem dust;

    private PlayerControls playerControls;
    private Rigidbody2D _rb;
    private Animator _animator;
    private float _movementInput;
    private bool _facingRight;
    private bool _isFalling;
    private bool _canDoubleJump;
    private bool _allowJump;

    // Audio & SFXs
    private AudioSource _audio;
    public AudioClip coinSFX;
    public AudioClip jumpSFX;
    public AudioClip isHitSFX;

    // Animation States
    const string PLAYER_IDLE = "Shelly-Idle";
    const string PLAYER_RUN = "Shelly-Walk";
    const string PLAYER_JUMP = "Shelly-Jump";
    const string PLAYER_GROUND_ATTACK = "Shelly-Ground-Sword-Attack";
    const string PLAYER_PUNCH = "Shelly-Punch";
    const string PLAYER_DIE = "Shelly-Die";
    const string PLAYER_LAND = "Shelly-Land";
    //const string PLAYER_FALL = "Shelly-Fall";
    const string PLAYER_FALL = "Shelly-Jump";
    private string _currentState;

    // Attack
    private bool _isAttacking = false;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public float attackRate = 2f;
    //float nextAttackTime = 0f;
    public LayerMask enemyLayers;
    const int ATTACK_SWORD = 1;
    const int ATTACK_BOOMERANG = 2;
    // Sprite Impact
    [SerializeField]
    private SpriteRenderer _playerSprite;
    private Shader _playerHitshader;
    private Shader _playerShaderSpritesDefault;
    public float playerImpactDuration = 0.05f;
    private float _impactX = -4f;
    private float _impactY = 4f;

    private void Awake()
    {
        _playerSprite = GetComponent<SpriteRenderer>();
        if (_playerSprite == null)
            Debug.LogError("EnemySprite component missing from this gameobject" + this.GetType());
        _playerHitshader = Shader.Find("GUI/Text Shader");
        _playerShaderSpritesDefault = Shader.Find("Sprites/Default");


        playerControls = new PlayerControls();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        _audio = gameObject.AddComponent<AudioSource>();


        

        _facingRight = true;
        _isFalling = false;
        _canDoubleJump = true;
        _allowJump = false;
        _currentState = PLAYER_IDLE;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {

        playerControls.PlatformsInput.Jump.performed += _ => Jump();
        //playerControls.PlatformsInput.Attack.performed += _ => Attack(ATTACK_SWORD);
        playerControls.PlatformsInput.Attack.performed += _ => Attack(ATTACK_BOOMERANG);
        //playerControls.PlatformsInput.Recall.performed += _ => boomerang.GetComponent<Boomerang>().Recall();

        playerCurrentHealth = playerMaxHealth;
        healthBar.SetMaxHealth(playerMaxHealth);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        // Read the movement value
        _movementInput = playerControls.PlatformsInput.Move.ReadValue<float>();

        // Move the player
        Vector3 currentPosition = transform.position;
        currentPosition.x += _movementInput * speed * Time.deltaTime;
        transform.position = currentPosition;

        if ((_movementInput > 0) && !_facingRight) // if moving right but facing left
        {
            Flip();
            _facingRight = true;
        }
        else if ((_movementInput < 0) && _facingRight) // if moving left but facing right
        {
            Flip();
            _facingRight = false;
        }

        if (!_isAttacking)
        {
            if (IsGrounded())
            {
                if (_isFalling)
                {
                    StartCoroutine("Landing");
                }
                else if (_movementInput > 0)
                {
                    ChangeAnimationState(PLAYER_RUN);
                }
                else if (_movementInput < 0)
                {
                    ChangeAnimationState(PLAYER_RUN);
                }
                else
                {
                    ChangeAnimationState(PLAYER_IDLE);
                }
            }
            else
            {
                if (_rb.velocity.y > 0)
                {
                    ChangeAnimationState(PLAYER_JUMP);
                }
                else
                {
                    ChangeAnimationState(PLAYER_FALL);
                    _isFalling = true;
                }
            }
        }
    }

    // Checking to see if the sprite should be flipped.
    void Flip()
    {
        _facingRight = !_facingRight;
        // new way to rotate the player
        transform.Rotate(0f, 180f, 0f);
        if (IsGrounded())
        {
            CreateDust();
        }
    }


    public void Attack(int damage)
    {
        if(damage == 1)
        {
            _isAttacking = true;
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy)
                {
                    enemy.GetComponent<Enemy>().TakeDamage(damage);
                }
            }
            StartCoroutine("GroundAttack");
        } else if (damage == 2) {
            this.GetComponent<Weapon>().Shoot();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
         Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void Jump()
    {
        if (IsGrounded())
        {
            _allowJump = true;
            CreateDust();
            _canDoubleJump = true;
        }
        else if (_canDoubleJump)
        {
            _allowJump = true;
            _canDoubleJump = false;
        }

        if (_allowJump)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, 0f);
            _rb.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
            ChangeAnimationState(PLAYER_JUMP);
            PlaySound(jumpSFX);
            _allowJump = false;
        }
    }

    public void ApplyPlayerDamage(int damage)
    {
        playerCurrentHealth -= damage;
        healthBar.SetHealth(playerCurrentHealth);

        if (playerCurrentHealth <= 0)
        {
            if (GameManager.gm)
            {
                // if the gameManager is available, tell it to reset the game
                GameManager.gm.GameOverMenu();
            }
        } else
        {
            _playerSprite.material.shader = _playerHitshader;
            _playerSprite.material.color = Color.red;
            PlaySound(isHitSFX);
            StartCoroutine(DisplayImpact(playerImpactDuration));


            if (!_facingRight)
            {
                _impactX = -_impactX;
            }
            _rb.AddForce(new Vector2(_impactX, _impactY), ForceMode2D.Impulse);

            if (GameManager.gm)
            {
                // if the gameManager is available, tell it to halt the game
                GameManager.gm.StopTime(playerImpactDuration);
            }
        }
    }

    IEnumerator DisplayImpact(float duration)
    {
        yield return new WaitForSeconds(duration);

        
        _playerSprite.material.shader = _playerShaderSpritesDefault;
        //_playerSprite.color = Color.white;
        _playerSprite.material.color = Color.white;

    }

    private bool IsGrounded()
    {
        return Physics2D.Linecast(transform.position, groundCheck.position, groundLayer);
    }

    // play sound through the audiosource on the gameobject
    void PlaySound(AudioClip clip)
    {
        _audio.PlayOneShot(clip);
    }

    void ChangeAnimationState(string newState)
    {
        if (_currentState == newState) return;
        _animator.Play(newState);
        _currentState = newState;
    }

    IEnumerator Landing()
    {
        ChangeAnimationState(PLAYER_LAND);
        yield return new WaitForSeconds(0.2f);
        _isFalling = false;
    }

    IEnumerator GroundAttack()
    {
        ChangeAnimationState(PLAYER_GROUND_ATTACK);
        yield return new WaitForSeconds(0.5f);
        _isAttacking = false;
    }

    void CreateDust()
    {
        dust.Play();
    }
}
