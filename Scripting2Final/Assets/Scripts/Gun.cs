using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gun : MonoBehaviour
{
    public Vector3 WorldMouse;

    public int turnspeed;

    public GameObject ammo;
    public List<GameObject> inAirAmmo = new List<GameObject>();
    

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
            if (inAirAmmo.Count == 3)
            {
                if (inAirAmmo[0] != null)
                {
                    Destroy(inAirAmmo[0]);
                }
                inAirAmmo.RemoveAt(0);
                inAirAmmo.Add((GameObject)Instantiate(ammo, transform.position, transform.rotation));
            }
            else
            {
                inAirAmmo.Add((GameObject)Instantiate(ammo, transform.position, transform.rotation));
            }
            

        }


    }
}
