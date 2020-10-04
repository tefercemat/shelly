using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerHealth = 4;

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

    // Animation States
    const string PLAYER_IDLE = "Shelly-Idle";
    const string PLAYER_RUN = "Shelly-Walk";
    const string PLAYER_JUMP = "Shelly-Jump";
    const string PLAYER_PUNCH = "Shelly-Punch";
    const string PLAYER_DIE = "Shelly-Die";
    const string PLAYER_LAND = "Shelly-Land";
    const string PLAYER_FALL = "Shelly-Fall";
    private string _currentState;

    private void Awake()
    {
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


    // Checking to see if the sprite should be flipped. Done in LateUpdate since the Animator may override the localScale.
    // This code will flip the player even if the animator is controlling scale
    void LateUpdate()
    {
        // get the current scale
        Vector3 localScale = transform.localScale;

        if (_movementInput > 0) // moving right so face right{
            _facingRight = true;
        else if (_movementInput < 0) // moving left so face left
            _facingRight = false;

        // check to see if scale x is right for the player
        // if not, multiple by -1 to flip a sprite
        if (((_facingRight) && (localScale.x < 0)) || ((!_facingRight) && (localScale.x > 0)))
        {
            if (IsGrounded())
            {
                CreateDust();
            }
            localScale.x *= -1;
        }
            

        // update the scale
        transform.localScale = localScale;
    }

    private void Jump()
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
        playerHealth -= damage;

        if (damage <= 0)
        {
            Debug.Log("Game Over");
        }
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

    void CreateDust()
    {
        dust.Play();
    }
}
