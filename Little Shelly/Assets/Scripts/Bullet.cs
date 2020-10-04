using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 20f;
    public Rigidbody2D _rb;
    private bool hasImpacted = false;

    // Start is called before the first frame update
    void Awake()
    {
        //_rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _rb.velocity = transform.right * speed;
    }

    private void FixedUpdate()
    {
        if (hasImpacted)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<CrabEnemy>().TakeDamage();
            hasImpacted = true;
        } else if (collision.tag == "Ground")
        {
            hasImpacted = true;
        }
        
    }
}
