using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public Vector3 WorldMouse;

    public int turnspeed;

    public GameObject ammo;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        aim();

    }

    void aim()
    {
        WorldMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(WorldMouse);

        WorldMouse.z = transform.position.z;
        transform.LookAt(WorldMouse);
        //transform.LookAt(Input.mousePosition);



        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(ammo, transform.position, transform.rotation);
        }


    }
}
