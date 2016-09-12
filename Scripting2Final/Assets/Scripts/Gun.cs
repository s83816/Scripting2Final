using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gun : MonoBehaviour
{
    public Vector3 WorldMouse;

    public int turnspeed;

    public PlatformBullets ammoscript;

    public GameObject ammo;
    private List<GameObject> pooledAmmos = new List<GameObject>();
    public List<GameObject> inAirAmmo = new List<GameObject>();
    private int poolAmmoAmount = 10;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < poolAmmoAmount; i++)
        {
            pooledAmmos.Add(Instantiate(ammo));
            pooledAmmos[i].SetActive(false);
        }
        ammoscript = ammo.GetComponent<PlatformBullets>();
    }

    // Update is called once per frame
    void Update()
    {
        aim();
        if (Input.GetMouseButtonDown(1))
        {
            ActivateBullet();
        }
    }
    void ActivateBullet()
    {
        for (int i = 0; i < inAirAmmo.Count; i++)
        {
            if (inAirAmmo[i].activeInHierarchy && !inAirAmmo[i].GetComponent<PlatformBullets>().Activated)
            {
                inAirAmmo[i].GetComponent<PlatformBullets>().ActivatePlatform();
                break;
            }
        }
    }
    void aim()
    {
        WorldMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(WorldMouse);

        WorldMouse.z = transform.position.z;
        transform.LookAt(WorldMouse);
        //transform.LookAt(Input.mousePosition);


        for (int i = 0; i < inAirAmmo.Count; i++)
        {
            if (!inAirAmmo[i].activeInHierarchy)
            {
                inAirAmmo.RemoveAt(i);
                i--;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {

            if (inAirAmmo.Count == 3)
            {
                if (inAirAmmo[0] != null)
                {
                    inAirAmmo[0].SetActive(false);
                }
                inAirAmmo.RemoveAt(0);
                for (int i = 0; i < pooledAmmos.Count; i++)
                {
                    if (!pooledAmmos[i].activeInHierarchy)
                    {
                        inAirAmmo.Add(pooledAmmos[i]);
                        pooledAmmos[i].transform.position = transform.position;
                        pooledAmmos[i].GetComponent<PlatformBullets>().SetProjType(PlayerControl.Instance.ProjType);
                        pooledAmmos[i].transform.rotation = transform.rotation;
                        pooledAmmos[i].SetActive(true);
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < pooledAmmos.Count; i++)
                {
                    if (!pooledAmmos[i].activeInHierarchy)
                    {
                        inAirAmmo.Add(pooledAmmos[i]);
                        pooledAmmos[i].transform.position = transform.position;
                        pooledAmmos[i].transform.rotation = transform.rotation;
                        pooledAmmos[i].GetComponent<PlatformBullets>().SetProjType(PlayerControl.Instance.ProjType);
                        pooledAmmos[i].SetActive(true);
                        break;
                    }
                }
            }
        }




    }
}
