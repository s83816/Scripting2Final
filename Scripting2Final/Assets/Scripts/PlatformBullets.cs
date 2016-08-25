using UnityEngine;
using System.Collections;

public class PlatformBullets : MonoBehaviour {
    Rigidbody rb;
    public GameObject platform;
    public float bulletspeed;
    public float slowdown;

    public bool withtime;
    public float stoptime;

    public bool isstopped;

    public bool canTransform = true;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();

    }
	
	// Update is called once per frame
	void Update () {


        if (isstopped == true && Input.GetMouseButtonDown(1) && canTransform)
        {
            Instantiate(platform,transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (withtime == false && bulletspeed > 0)
        {
            bulletspeed -= slowdown;
            rb.velocity = transform.forward * bulletspeed;
        }

        if(bulletspeed <= 0)
        {
            bulletspeed = 0;
            isstopped = true;

            if (withtime == false)
                rb.velocity = transform.forward * bulletspeed;
        }
        

        if (withtime == true && stoptime > 0)
        {
            stoptime -= Time.deltaTime;
            rb.velocity = transform.forward * stoptime;
        }
        if(stoptime<=0)
        {
            stoptime = 0;
            isstopped = true;

            if (withtime == true)
                rb.velocity = transform.forward * stoptime;
        }
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(canTransform)
                canTransform = false;
        }
    }
    void OnTriggerStay(Collider other)
    {
        OnTriggerEnter(other);
    }
    void OnTriggerExit(Collider other)
    {
        canTransform = true;
    }
}
