using UnityEngine;
using System.Collections;

public class PlatformBullets : MonoBehaviour
{
    Rigidbody rb;
    public GameObject platform;
    public GameObject bulletMesh;
    public float bulletspeed;
    private float tempBulletSpeed;
    public float slowdown;

    public bool withtime;
    public float stoptime;

    public bool isstopped;

    public bool canTransform = true;

    public float platformExistTime = 5f;
    public float platformExistCount = 0;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tempBulletSpeed = bulletspeed;
    }
    void OnDisable()
    {
        if(rb==null)
            rb = GetComponent<Rigidbody>();
        platform.SetActive(false);
        bulletMesh.SetActive(true);
        rb.isKinematic = false;
        platformExistCount = 0;
        tempBulletSpeed = bulletspeed;
    }
    void Update()
    {
        if (!platform.activeInHierarchy)
        {
            if (isstopped == true && Input.GetMouseButtonDown(1) && canTransform)
            {
                transform.rotation = Quaternion.identity;
                platform.SetActive(true);
                bulletMesh.SetActive(false);
                rb.isKinematic = true;
            }
            if (withtime == false && tempBulletSpeed > 0)
            {
                tempBulletSpeed -= slowdown;
                rb.velocity = transform.forward * tempBulletSpeed;
            }
            if (tempBulletSpeed <= 0)
            {
                tempBulletSpeed = 0;
                isstopped = true;

                if (withtime == false)
                    rb.velocity = transform.forward * tempBulletSpeed;
            }
            if (withtime == true && stoptime > 0)
            {
                stoptime -= Time.deltaTime;
                rb.velocity = transform.forward * stoptime;
            }
            if (stoptime <= 0)
            {
                stoptime = 0;
                isstopped = true;

                if (withtime == true)
                    rb.velocity = transform.forward * stoptime;
            }
        }
        else
        {
            if (platformExistCount >= platformExistTime)
            {
                gameObject.SetActive(false);
            }
            else
            {
                platformExistCount += Time.deltaTime;
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (!platform.activeInHierarchy)
        {
            if (other.CompareTag("Player"))
            {
                if (canTransform)
                    canTransform = false;
            }
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
