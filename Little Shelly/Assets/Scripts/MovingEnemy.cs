using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : MonoBehaviour
{
	[Range(0f, 10f)]
    public float moveSpeed = 4f;
    public GameObject[] myWaypoints;
    public float waitAtWaypointTime = 1f;
    public bool loopWaypoints = true;
	public Animation walkAnimation;

    private Transform _transform;
    private Rigidbody2D _rigidbody;
	private Animator _animator;

    // movement tracking
    [SerializeField]
    private int _myWaypointIndex = 0; // used as index for My_Waypoints
    private float _moveTime;
    private float _vx = 0f;
    private bool _moving = true;

    private void Awake()
    {
        // get a reference to the components we are going to be changing and store a reference for efficiency purposes
        _transform = GetComponent<Transform>();

        _rigidbody = GetComponent<Rigidbody2D>();
        if (_rigidbody == null) // if Rigidbody is missing
            Debug.LogError("Rigidbody2D component missing from this gameobject");

		_animator = GetComponent<Animator>();
		if (_animator == null) // if Animator is missing
			Debug.LogError("Animator component missing from this gameobject");


		// setup moving defaults
		_moveTime = 0f;
        _moving = true;

    }

    

    // Update is called once per frame
    void Update()
    {
		if (Time.time >= _moveTime)
		{
			EnemyMovement();
		}
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
				_rigidbody.velocity = new Vector2(0, 0);

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
				//_animator.Play("Crab-Walk");
				//if (_rigidbody.velocity.x == 0) Debug.Log("I m stuck!!");
				// Set the enemy's velocity to moveSpeed in the x direction.
				_rigidbody.velocity = new Vector2(_transform.localScale.x * moveSpeed, _rigidbody.velocity.y);
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
}
