using UnityEngine;
using System.Collections;

public class addforce : MonoBehaviour {
    Rigidbody rb;
    public float bulletspeed;
    public float slowdown;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();

    }
	
	// Update is called once per frame
	void Update () {
   
        if (bulletspeed > 0)
        {
            bulletspeed -= slowdown;
            //rb.AddForce(transform.forward * bulletspeed);
            rb.velocity = transform.forward * bulletspeed;
        }
        else
        {
            bulletspeed = 0;
            rb.velocity = transform.forward * bulletspeed;
        }
	}
}
