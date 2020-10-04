using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHeadBump : MonoBehaviour
{
    // if Player hits the stun point of the enemy, then call Stunned on the enemy
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			// tell the enemy to be stunned
			this.GetComponentInParent<CrabEnemy>().TakeDamage();

			// Make the player bounce off the enemy
			//other.gameObject.GetComponent<PlayerController>().EnemyBounce();
		}
	}
}
