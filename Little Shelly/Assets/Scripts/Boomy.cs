using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomy : MonoBehaviour
{
    public float speed = 15f;
    public int boomyDamage = 1;
    public float throwDistance = 5f;
    public GameObject weaponPoint;
    public Rigidbody2D rb;
    private State state;

    private enum State
    {
        Thrown,
        Recalling,
    }

    /************************/
    /*        Audio         */
    /************************/
    // AudioSource Reference
    private AudioSource _audio;
    // AudioClip(s)
    public AudioClip boomyHit;
    public float boomyHitVolume = 2f;
    public AudioClip boomyFly;
    public float boomyFlyVolume = 0.25f;
    /*  ==  End ==   */

    /************************/
    /*       Animation      */
    /************************/
    // Animator Reference
    private Animator _animator;
    private string _currentState;
    // Animation States
    const string BOOMY_FLY = "Boomy_Fly";
    public bool activateCamShake = false;


    // Start is called before the first frame update
    private void Awake()
    {
        weaponPoint = GameObject.FindWithTag("WeaponPoint");

        // Animator
        _animator = GetComponent<Animator>();
        if (_animator == null)
            Debug.LogError("Rigidbody2D component missing from this gameobject" + this.GetType());

        // AudioSource
        _audio = GetComponent<AudioSource>();
        if (_audio == null)
            Debug.LogError("AudioSource component missing from this gameobject" + this.GetType());

        state = State.Thrown;
    }

    void Start()
    {
        rb.velocity = transform.right * speed;
        PlaySound(boomyFly,boomyFlyVolume);
        ChangeAnimationState(BOOMY_FLY);
    }

    private void Update()
    {

        float distWithWeaponPoint = Vector2.Distance(transform.position, weaponPoint.GetComponent<Transform>().position);

        if ((distWithWeaponPoint > throwDistance) && (state == State.Thrown))
        {
            Recall();
        } else if ((distWithWeaponPoint < 0.5f) && (state == State.Recalling)){
            DestroyBoomy();
        }
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case State.Recalling:
                Vector2 dirToWepaonPoint = (weaponPoint.GetComponent<Transform>().position - transform.position).normalized;
                rb.velocity = dirToWepaonPoint * speed;
                break;
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(boomyDamage);
            if (activateCamShake)
            {
                CinemachineShake.Instance.ShakeCamera(10f, 0.5f);
            }
            PlaySound(boomyHit, boomyHitVolume);
        }
    }

    private void Recall()
    {
        state = State.Recalling;
    }

    private void DestroyBoomy()
    {
        Destroy(gameObject);
    }

    // play sound through the audiosource on the gameobject
    void PlaySound(AudioClip clip, float volume)
    {
        _audio.PlayOneShot(clip, volume);
    }

    void ChangeAnimationState(string newState)
    {
        if (_currentState == newState) return;
        _animator.Play(newState);
        _currentState = newState;
    }
}
