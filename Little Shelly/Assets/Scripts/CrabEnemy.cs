using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabEnemy : MonoBehaviour
{
    /************************/
    /*   Health & Damage    */
    /************************/
    // Enemy Health
    public int enemyHealth = 1;
    // Damage Done to Player
    public int playerDamage = 1;
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
    const string CRAB_WALK = "Crab-Walk";
    const string CRAB_SLEEP = "Crab-Sleep";
    const string CRAB_AWAKE = "Crab-Awake";
    const string CRAB_DIE = "Crab-Die";
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
    }

    private void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _awakeTimer -= Time.deltaTime;
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

        if (Time.time >= _moveTime)
        {
            EnemyMovement();
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

    // flip the enemy to face torward the direction he is moving in
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
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
			PlayerController player = collision.gameObject.GetComponent<PlayerController>();
			
				// Make sure the enemy is facing the player on attack
				//Flip(collision.transform.position.x-_transform.position.x);
				
				// attack sound
				//PlaySound(attackSFX);
				
				// stop moving
				_rb.velocity = new Vector2(0, 0);

            // apply damage to the player
				player.ApplyPlayerDamage (playerDamage);
				
				// stop to enjoy killing the player
				//_moveTime = Time.time + stunnedTime;
			
		}
	}

    public void TakeDamage()
    {
        StartCoroutine("DieSlowly");
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
}
