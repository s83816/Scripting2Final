using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gun : MonoBehaviour
{
    private static Gun myInstance;
    public Vector3 WorldMouse;

    public int turnspeed;

    public PlatformBullets ammoscript;
    public GameObject preAimBulletPrefab;
    public GameObject trailPrefab;
    public GameObject ammo;
    public GameObject holderPrefab;
    private List<GameObject> pooledAmmos = new List<GameObject>();
    private List<GameObject> pooledPreAimBullet = new List<GameObject>();
    private List<GameObject> pooledTrails = new List<GameObject>();
    public List<GameObject> inAirAmmo = new List<GameObject>();
    private Transform ammoHolder;
    private Transform preAimBulletHolder;
    private Transform trailHolder;
    private int poolAmmoAmount = 10;
    private int preAimPooledAmount = 5;
    private int trailPooledAmount = 30;

    // Use this for initialization
    void Start()
    {
        ammoHolder = Instantiate(holderPrefab).transform;
        ammoHolder.name = "AmmoHolder";
        preAimBulletHolder = Instantiate(holderPrefab).transform;
        preAimBulletHolder.name = "PreAimHolder";
        trailHolder = Instantiate(holderPrefab).transform;
        trailHolder.name = "TrailHolder";
        for (int i = 0; i < poolAmmoAmount; i++)
        {
            pooledAmmos.Add(Instantiate(ammo));
            pooledAmmos[i].SetActive(false);
            pooledAmmos[i].transform.SetParent(ammoHolder);
        }
        for (int i = 0; i < preAimPooledAmount; i++)
        {
            pooledPreAimBullet.Add(Instantiate(preAimBulletPrefab));
            pooledPreAimBullet[i].SetActive(false);
            pooledPreAimBullet[i].transform.SetParent(preAimBulletHolder);
        }
        for (int i = 0; i < trailPooledAmount; i++)
        {
            pooledTrails.Add(Instantiate(trailPrefab));
            pooledTrails[i].SetActive(false);
            pooledTrails[i].transform.SetParent(trailHolder);
        }
        ammoscript = ammo.GetComponent<PlatformBullets>();
        PreAim();
    }  
    void Update()
    {
        if (PlayerControl.Instance.CanMove)
        {
            aim();
            if (Input.GetMouseButtonDown(1))
            {
                ActivateBullet();
            }
        }       
    }
    public void PreAim()
    {
        for(int i = 0; i < pooledPreAimBullet.Count; i++)
        {
            if (!pooledPreAimBullet[i].activeInHierarchy)
            {
                pooledPreAimBullet[i].transform.position = transform.position;
                pooledPreAimBullet[i].SetActive(true);
                break;
            }
        }
    }
    public static Gun Instance
    {
        get
        {
            if (myInstance == null)
            {
                myInstance = FindObjectOfType<Gun>();
            }
            return myInstance;
        }
    }
    public GameObject GetTrail
    {
        get
        {
            for(int i = 0; i < pooledTrails.Count; i++)
            {
                if (!pooledTrails[i].activeInHierarchy)
                {
                    return pooledTrails[i];
                }
            }
            return null;
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
                bool disable = false;
                for(int i = 0; i < inAirAmmo.Count; i++)
                {
                    if (!inAirAmmo[i].GetComponent<PlatformBullets>().Activated)
                    {
                        inAirAmmo[i].SetActive(false);
                        inAirAmmo.RemoveAt(i);
                        disable = true;
                        break;
                    }
                }
                if (!disable)
                {
                    if (inAirAmmo[0] != null)
                    {
                        inAirAmmo[0].SetActive(false);
                        inAirAmmo.RemoveAt(0);
                    }
                }
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
