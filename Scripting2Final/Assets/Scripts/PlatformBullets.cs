using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformBullets : MonoBehaviour
{
    public GameObject Gun;
    public Vector3 startPosition;
    public Vector3 Endposition;
    public ProjectileType projT = ProjectileType.Horizontal;


    Rigidbody rb;
    public Dictionary<ProjectileType, GameObject> platform = new Dictionary<ProjectileType, GameObject>();
    public List<GameObject> platforms = new List<GameObject>();
    public GameObject bulletMesh;
    public float bulletspeed;
    private float tempBulletSpeed;
    public float slowdown;

    public int angle;

    public float StartingTime;

    public bool isstopped;

    public bool canTransform = true;

    public float platformExistTime = 10f;
    public float platformExistCount = 0;
    public Color StartingColor;
    public Color decayColor;
    private Color lerpedColor;

    public Renderer[] ChildMaterials;


    //public Renderer[] ChildrenRenderer;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tempBulletSpeed = bulletspeed;
        ChildMaterials = GetComponentsInChildren<Renderer>();
        if (platforms.Count >= 2)
        {
            platform.Add(ProjectileType.Horizontal, platforms[0]);
            platform.Add(ProjectileType.Verticle, platforms[1]);
        }

    }


    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Floor"))
        {
            isstopped = true;
        }
        if (other.collider.CompareTag("MovingPlatform"))
        {
            gameObject.SetActive(false);
        }
        if (other.collider.CompareTag("EnemyBullet"))
        {
            other.collider.GetComponent<EnemyBullet>().IsHit();
        }
    }
    void OnDisable()
    {

        if (rb == null)
            rb = GetComponent<Rigidbody>();
        if (platform.Count > 0)
        {
            for (int i = 0; i < platforms.Count; i++)
            {
                platforms[i].SetActive(false);
            }
        }
        //GetComponent<Rigidbody>().velocity = Vector3.zero;
        bulletMesh.SetActive(true);
        //rb.isKinematic = false;
        platformExistCount = 0;
        tempBulletSpeed = bulletspeed;
        isstopped = false;
    }
    void OnEnable()
    {

        startPosition = transform.position;
        startPosition.z = 0f;

        Endposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Endposition.z = 0f;
        StartingTime = Time.time;
    }
    public void SetProjType(ProjectileType type)
    {
        projT = type;
    }
    void Update()
    {

        if (!platform[projT].activeInHierarchy)
        {
            if (isstopped == true && Input.GetMouseButtonDown(1) && canTransform)
            {
                transform.rotation = Quaternion.Euler(0, 0, angle);
                platform[projT].SetActive(true);
                bulletMesh.SetActive(false);
                //rb.isKinematic = true;

            }

            if (transform.position == Endposition)
            {
                isstopped = true;

            }

            if (isstopped == false)
            {
                this.transform.position = Vector3.MoveTowards(transform.position, Endposition, Time.deltaTime * bulletspeed);

            }


        }
        else
        {
            ChildMaterials = GetComponentsInChildren<Renderer>();

            if (platformExistCount >= platformExistTime)
            {
                Disable();
            }
            else
            {

                platformExistCount += Time.deltaTime;
                if (platformExistCount / platformExistTime < 1f)
                    lerpedColor = Color.Lerp(StartingColor, decayColor, platformExistCount / platformExistTime);


                foreach (var r in ChildMaterials)
                {
                    // Do something with the renderer here...
                    r.material.color = lerpedColor;
                }


            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (!platform[projT].activeInHierarchy)
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
    public void Disable()
    {
        lerpedColor = StartingColor;
        gameObject.SetActive(false);
    }
}
