using UnityEngine;
using System.Collections;

public class PlatformBullets : MonoBehaviour
{
    public GameObject Gun;
    public Vector3 startPosition;
    public Vector3 Endposition;



    Rigidbody rb;
    public GameObject platform;
    public GameObject bulletMesh;
    public float bulletspeed;
    private float tempBulletSpeed;
    public float slowdown;

    public int angle;

    public float StartingTime;

    public bool isstopped;

    public bool canTransform = true;

    public float platformExistTime = 5f;
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
    }
    void OnDisable()
    {
        
        if (rb == null)
            rb = GetComponent<Rigidbody>();
        platform.SetActive(false);
        bulletMesh.SetActive(true);
        rb.isKinematic = false;
        platformExistCount = 0;
        tempBulletSpeed = bulletspeed;
        isstopped = false;
    }
    void OnEnable()
    {
        foreach (var r in ChildMaterials)
        {
            r.material.color = StartingColor;
        }
        startPosition = transform.position;
        startPosition.z = 0f;

        Endposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Endposition.z = 0f;
        StartingTime = Time.time;
    }
    void Update()
    {
        
        if (!platform.activeInHierarchy)
        {
            if (isstopped == true && Input.GetMouseButtonDown(1) && canTransform)
            {
                transform.rotation = Quaternion.EulerAngles(0,0, angle);
                platform.SetActive(true);
                bulletMesh.SetActive(false);
                rb.isKinematic = true;
                
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
                lerpedColor = StartingColor;
                gameObject.SetActive(false);
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
