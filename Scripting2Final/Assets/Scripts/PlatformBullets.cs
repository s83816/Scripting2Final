using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformBullets : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 Endposition;
    public ProjectileType projT = ProjectileType.Horizontal;

    Rigidbody rb;
    public Dictionary<ProjectileType, GameObject> platform = new Dictionary<ProjectileType, GameObject>();
    public List<GameObject> platforms = new List<GameObject>();
    public GameObject bulletMesh;
    public float slowdown;

    public int angle;

    public float StartingTime;

    public bool isstopped;

    public bool canTransform = true;

    protected bool activated = false;

    public float platformExistTime = 15f;
    public float platformExistCount = 0;
    public Color StartingColor;
    public Color decayColor;
    private Color lerpedColor;

    public Renderer[] ChildMaterials;

    protected float moveImpulse = 40f;

    //public Renderer[] ChildrenRenderer;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
            //isstopped = true;
            //GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().drag = 0.5f;
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
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        activated = false;
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
        isstopped = false;
    }
    void OnEnable()
    {
        GetComponent<Rigidbody>().drag = 0.1f;
        GetComponent<Rigidbody>().isKinematic = false;
        startPosition = transform.position;
        startPosition.z = 0f;
        Endposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Endposition.z = 0f;
        Vector3 dir = ((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position)).normalized;
        dir.z = 0f;
        //Debug.Log(dir);
        GetComponent<Rigidbody>().AddForce(dir * moveImpulse, ForceMode.Impulse);
        StartingTime = Time.time;
        switch (projT)
        {
            case ProjectileType.Horizontal:
                bulletMesh.GetComponent<Renderer>().material.color = Color.blue;
                StartingColor = Color.blue;
                break;
            case ProjectileType.Verticle:
                bulletMesh.GetComponent<Renderer>().material.color = Color.green;
                StartingColor = Color.green;
                break;
        }
    }
    public void SetProjType(ProjectileType type)
    {
        projT = type;
    }
    void Update()
    {

        if (!platform[projT].activeInHierarchy)
        {

            if (isstopped == false)
            {
                if (Vector3.Distance(transform.position, Endposition) < 0.5f)
                {
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
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
        if(other.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            isstopped = true;
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
    public void ActivatePlatform()
    {
        if (canTransform)
        {
            activated = true;
            GetComponent<Rigidbody>().isKinematic = true;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            platform[projT].SetActive(true);
            bulletMesh.SetActive(false);
            //rb.isKinematic = true;

        }
    }
    public bool Activated
    {
        get
        {
            return activated;
        }
    }
}
