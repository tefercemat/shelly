using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /************************/
    /*   Health & Damage    */
    /************************/
    // Enemy Health
    public int enemyHealth = 1;
    public int enemyCurrentHealth;
    // Damage Done to Player
    public int playerDamage = 1;
    public int HeadDamage = 1;
    [SerializeField]
    private SpriteRenderer _enemySprite;
    private Shader _hitshader;
    private Shader _shaderSpritesDefault;
    public float impactDuration = 0.05f;
    public float stunnedTime = 3f;   // how long to wait at a waypoint
    public string stunnedLayer = "StunnedEnemy";  // name of the layer to put enemy on when stunned
    public string playerLayer = "Player";  // name of the player layer to ignore collisions with when stunned
    public bool isStunned = false;  // flag for isStunned
     // store the layer number the enemy is on (setup in Awake)
    int _enemyLayer;
    // store the layer number the enemy should be moved to when stunned
    int _stunnedLayer;
    private bool alreadyAttacked = false;
    /*  ==  End ==   */


    /************************/
    /*        Audio         */
    /************************/
    // AudioSource Reference
    private AudioSource _audio;
    // AudioClip(s)
    public AudioClip awakeJumpSFX;
    public AudioClip deadSFX;
    /*  ==  End ==   */

    /************************/
    /*         SFXx         */
    /************************/
    // SFXs
    public GameObject explosion;
    /*  ==  End ==   */

    /************************/
    /*       Animation      */
    /************************/
    // Animator Reference
    private Animator _animator;
    // Animation States
    const string CRAB_IDLE = "Crab-Idle";
    //const string CRAB_HURT = "Crab-Idle";
    const string CRAB_WALK = "Crab-Walk";
    const string CRAB_SLEEP = "Crab-Sleep";
    const string CRAB_AWAKE = "Crab-Awake";
    //const string CRAB_DIE = "Crab-Die";
    private string _currentState;
    /*  ==  End ==   */

    /************************/
    /*     Moving Enemy     */
    /************************/
    // Moving enemy Setup
    [Range(0f, 10f)]
    public float moveSpeed = 4f;
    public GameObject[] myWaypoints;
    public float waitAtWaypointTime = 1f;
    public bool loopWaypoints = true;
    // movement tracking
    [SerializeField]
    private int _myWaypointIndex = 0; // used as index for My_Waypoints
    private float _moveTime;
    private float _vx = 0f;
    private bool _moving = true;
    /*  ==  End ==   */

    /************************/
    /*        Physics       */
    /************************/
    // Enemy Rigidbody2D reference
    private Rigidbody2D _rb;
    // Enemy Transform reference
    private Transform _transform;
    // Direction checker
    private bool _facingRight;
    public float jumpSpeed = 3.0f;
    //Awake & Sleep
    private Vector2 startPosition;
    public float wakeUpDistance = 5f;
    private bool _isAsleep;
    public GameObject player;
    public float minTimeAwake = 5f;
    private float _awakeTimer;
    /*  ==  End ==   */


    private void Awake()
    {

        // Get references to components
        // Transform
        _transform = GetComponent<Transform>();

        _enemySprite = GetComponent<SpriteRenderer>();
        if (_enemySprite == null)
            Debug.LogError("EnemySprite component missing from this gameobject" + this.GetType());
        _hitshader = Shader.Find("GUI/Text Shader");
        _shaderSpritesDefault = Shader.Find("Sprites/Default"); // or whatever sprite shader is being used

        // Rigidbody
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
            Debug.LogError("Rigidbody2D component missing from this gameobject" + this.GetType());
        // Animator
        _animator = GetComponent<Animator>();
        if (_animator == null)
            Debug.LogError("Rigidbody2D component missing from this gameobject" + this.GetType());
        _audio = GetComponent<AudioSource>();
        // AudioSource
        if (_audio == null)
            Debug.LogError("AudioSource component missing from this gameobject" + this.GetType());

        // Init entry animation
        _currentState = CRAB_SLEEP;

        // Setup default direction
        _facingRight = true;

        // Setup moving defaults
        _moveTime = 0f;
        _moving = false;
        _isAsleep = true;
        _awakeTimer = minTimeAwake;

        // determine the enemies specified layer
        _enemyLayer = this.gameObject.layer;

        // determine the stunned enemy layer number
        _stunnedLayer = LayerMask.NameToLayer(stunnedLayer);

        // make sure collision are off between the playerLayer and the stunnedLayer
        // which is where the enemy is placed while stunned
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(playerLayer), _stunnedLayer, true);
    }

    private void Start()
    {
        startPosition = transform.position;
        enemyCurrentHealth = enemyHealth;
    }

    // Update is called once per frame
    void Update()
    {
        _awakeTimer -= Time.deltaTime;

        if (!isStunned)
        {
            if (Time.time >= _moveTime)
            {
                EnemyMovement();
            }
            else
            {
                ChangeAnimationState(CRAB_IDLE);
            }
        }
        if (Vector2.Distance(transform.position, player.transform.position) <= wakeUpDistance) // if within distance
        {
            _awakeTimer = minTimeAwake;
            if (_isAsleep) // and sleeping
            {
                WakeUp(true); // time to wake up
            }
        }  else if (Vector2.Distance(transform.position, player.transform.position) > wakeUpDistance) // if outside distance
        {
            WakeUp(false); // go to sleep
        }
    }

    private void LateUpdate()
    {
        // get the current scale
        Vector3 localScale = transform.localScale;

        if (_rb.velocity.x > 0) // moving right so face right{
            _facingRight = true;
        else if (_rb.velocity.x < 0) // moving left so face left
            _facingRight = false;

        // check to see if scale x is right for the player
        // if not, multiple by -1 to flip a sprite
        if (((_facingRight) && (localScale.x < 0)) || ((!_facingRight) && (localScale.x > 0)))
        {
            localScale.x *= -1;
        }
        // update the scale
        transform.localScale = localScale;
    }

    // Move the enemy through its rigidbody based on its waypoints
    void EnemyMovement()
    {
        // if there isn't anything in My_Waypoints
        if ((myWaypoints.Length != 0) && (_moving))
        {
            // determine distance between waypoint and enemy
            _vx = myWaypoints[_myWaypointIndex].transform.position.x - _transform.position.x;

            // make sure the enemy is facing the waypoint (based on previous movement)
            Flip(_vx);

            // if the enemy is close enough to waypoint, make it's new target the next waypoint
            if (Mathf.Abs(_vx) <= 0.05f)
            {
                // At waypoint so stop moving
                _rb.velocity = new Vector2(0, 0);
                ChangeAnimationState(CRAB_IDLE);

                // increment to next index in array
                _myWaypointIndex++;

                // reset waypoint back to 0 for looping
                if (_myWaypointIndex >= myWaypoints.Length)
                {
                    if (loopWaypoints)
                        _myWaypointIndex = 0;
                    else
                        _moving = false;
                }

                // setup wait time at current waypoint
                _moveTime = Time.time + waitAtWaypointTime;
            }
            else
            {
                // enemy is moving
                ChangeAnimationState(CRAB_WALK);
                // Set the enemy's velocity to moveSpeed in the x direction.
                _rb.velocity = new Vector2(_transform.localScale.x * moveSpeed, _rb.velocity.y);
            }
        }
    }

    // Flip the enemy to face torward the direction he is moving in
    void Flip(float _vx)
    {
        // get the current scale
        Vector3 localScale = _transform.localScale;

        if ((_vx > 0f) && (localScale.x < 0f))
            localScale.x *= -1;
        else if ((_vx < 0f) && (localScale.x > 0f))
            localScale.x *= -1;

        // update the scale
        _transform.localScale = localScale;
    }

    // Attack player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach(ContactPoint2D hitpos in collision.contacts)
        {
            if ((collision.gameObject.tag == "Player") && (!isStunned))
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                if (hitpos.normal.y == -1)
                {
                    TakeDamage(HeadDamage);
                }
                else if(!alreadyAttacked)
                {
                    player.ApplyPlayerDamage(playerDamage);
                    alreadyAttacked = true;
                    _rb.velocity = Vector2.zero;
                    // stop to enjoy killing the player
                    _moveTime = Time.time + stunnedTime;

                    this.gameObject.layer = _stunnedLayer;

                    // start coroutine to stand up
                    StartCoroutine(Stand());
                }
            }
        }
        alreadyAttacked = false;
    }
    
    public void TakeDamage(int damage)
    {
        enemyCurrentHealth -= damage;

        if (enemyCurrentHealth > 0)
        {
            Stunned();
            _enemySprite.material.shader = _hitshader;
            _enemySprite.material.color = Color.white;
            StartCoroutine(DisplayImpact(impactDuration));

            if (GameManager.gm) // if the gameManager is available, tell it to halt the game
            {
                GameManager.gm.StopTime(impactDuration);
            }
        } else {
            StartCoroutine("DieSlowly");
        }
    }

    IEnumerator DisplayImpact(float duration)
    {
        yield return new WaitForSeconds(duration);
        _enemySprite.material.shader = _shaderSpritesDefault;
        _enemySprite.material.color = Color.white;

    }


    public void WakeUp(bool doWakeUp)
    {
        if (doWakeUp)
        {
            _isAsleep = false;
            StartCoroutine("WakingUp");
            _awakeTimer = minTimeAwake;
        } else if ((!doWakeUp) && (_awakeTimer < 0))
        {
            ChangeAnimationState(CRAB_SLEEP);
            _moving = false;
            _isAsleep = true;
        }
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


    IEnumerator WakingUp()
    {

        _rb.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
        ChangeAnimationState(CRAB_AWAKE);
        if (awakeJumpSFX)
        {
            PlaySound(awakeJumpSFX);
        }
        yield return new WaitForSeconds(0.5f);
        ChangeAnimationState(CRAB_WALK);
        _moving = true;
    }

    IEnumerator DieSlowly()
    {
        if (explosion)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }
        if (deadSFX)
        {
            PlaySound(deadSFX);
        }
        yield return new WaitForSeconds(0.2f);
        Object.Destroy(this.gameObject);
    }

    // setup the enemy to be stunned
    public void Stunned()
    {
        if (!isStunned)
        {
            isStunned = true;

            // stop moving
            _rb.velocity = new Vector2(0, 0);
            ChangeAnimationState(CRAB_SLEEP);

            // switch layer to stunned layer so no collisions with the player while stunned
            this.gameObject.layer = _stunnedLayer;

            // start coroutine to stand up eventually
            StartCoroutine(Stand());
        }
    }

    // coroutine to unstun the enemy and stand back up
    IEnumerator Stand()
    {
        yield return new WaitForSeconds(stunnedTime);

        // no longer stunned
        isStunned = false;

        // switch layer back to regular layer for regular collisions with the player
        this.gameObject.layer = _enemyLayer;

        // provide the player with feedback
        ChangeAnimationState(CRAB_AWAKE);
    }
}
