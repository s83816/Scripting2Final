using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum ShootDirection
{
    left = 0,
    right = 1,
    up = 2,
    down = 3
}
public class Turret : MonoBehaviour
{
    public GameObject bulletPrefab;
    public ShootDirection shootDir = ShootDirection.left;
    List<GameObject> pooledBullets = new List<GameObject>();
    private int bulletAmount = 20;
    private float shootForce = 9f;
    public float startDelay = 0.5f;
    public float shootFrequency = 1f;
    void Start()
    {
        for(int i = 0; i < bulletAmount; i++)
        {
            pooledBullets.Add(Instantiate(bulletPrefab));
            pooledBullets[i].transform.SetParent(transform);
            pooledBullets[i].transform.localPosition = Vector3.zero;
            pooledBullets[i].SetActive(false);
        }
        InvokeRepeating("Shoot", startDelay, shootFrequency);
    }

    void Shoot()
    {
        //Debug.Log("run");
        for(int i = 0; i < pooledBullets.Count; i++)
        {
            if (!pooledBullets[i].activeInHierarchy)
            {
                pooledBullets[i].transform.localPosition = Vector3.zero;
                pooledBullets[i].transform.localRotation = Quaternion.identity;
                pooledBullets[i].SetActive(true);
                Vector3 dir = Vector3.zero;
                switch (shootDir)
                {
                    case ShootDirection.left:
                        dir = Vector3.left;
                        break;
                    case ShootDirection.right:
                        dir = Vector3.right;
                        break;
                    case ShootDirection.up:
                        dir = Vector3.up;
                        break;
                    case ShootDirection.down:
                        dir = Vector3.down;
                        break;
                }
                pooledBullets[i].GetComponent<Rigidbody>().AddRelativeForce(dir * shootForce, ForceMode.Impulse);
                break;
            }
        }
    }
}
