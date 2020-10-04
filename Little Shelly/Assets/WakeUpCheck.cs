using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeUpCheck : MonoBehaviour
{
    //

    public GameObject wakeUpObject;
    public bool isAsleep;


    private void Awake()
    {
        // Object to wake up
        if (wakeUpObject == null)
            Debug.LogError("wakeUpObject is missing from this gameobject" + this.GetType());

        // Sleep at first
        isAsleep = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Wake when Player enters
        isAsleep = true;
        if (collision.tag == "Player")
        {
            if (wakeUpObject)
                wakeUpObject.GetComponent<CrabEnemy>().WakeUp(isAsleep);
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Let the enemy to go back to sleep
        isAsleep = false;
        if (collision.tag == "Player")
        {
            if (wakeUpObject)
                wakeUpObject.GetComponent<CrabEnemy>().WakeUp(isAsleep);

        }
    }
}
