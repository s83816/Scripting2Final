using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public enum ProjectileType
{
    Horizontal = 0,
    Verticle = 1
}
public class PlayerControl : MonoBehaviour
{
    private static PlayerControl myPlayerControl;
    public Vector3 checkpoint;
    public Vector3 Startpoint;
    Rigidbody rigid;
    public GameObject barrel;

    private ProjectileType projectileType = ProjectileType.Horizontal;
    public int hp = 5;
    public int startinghp = 5;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    public Image damageImage;

    bool mL = false;
    bool mR = false;
    bool canJump = false;
    bool isJumping = false;

    bool canTakeDmg = true;
    float maxVelocityX = 5f;
    float jumpForce = 10f;
    float invTime = 1f;
    public float jumpCount = 0;
    int layerMask;
    private bool canMove = true;
    private Renderer gunRend;

    public GameObject[] Healthbar;
    public static PlayerControl Instance
    {
        get
        {
            if (myPlayerControl == null)
            {
                myPlayerControl = FindObjectOfType<PlayerControl>();
            }
            return myPlayerControl;
        }
    }
    public ProjectileType ProjType
    {
        get
        {
            return projectileType;
        }
    }
    void Start()
    {
        damageImage = GameObject.Find("redflash").GetComponent<Image>();
        gunRend = barrel.GetComponent<Renderer>();
        gunRend.material.color = Color.blue;
        rigid = GetComponent<Rigidbody>();
        layerMask = 1 << LayerMask.NameToLayer("Floor");
        Startpoint = transform.position;
                Healthbar = new GameObject[5];
        for(int j = 0; j < Healthbar.Length; j++)
        { 
            Healthbar[j] = GameObject.Find("heart"+j.ToString());
        }
    }

    void Update()
    {
        //respawn test 
        if (canMove)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                ReturnToCheckPoint();
            }

            if (hp <= 0)
                Respawn();
            CheckInput();
        }
        ResetScreenColor();
    }
    void ReturnToCheckPoint()
    {
        rigid.velocity = Vector3.zero;
        if( checkpoint != Vector3.zero )
            transform.position = checkpoint;
        else
            transform.position = Startpoint;
    }
    void FixedUpdate()
    {
        Movement();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor") || other.gameObject.layer == LayerMask.NameToLayer("Turret"))
        {
            canJump = true;
            isJumping = false;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if ((other.gameObject.layer == LayerMask.NameToLayer("Floor")|| other.gameObject.layer == LayerMask.NameToLayer("Turret")) && !isJumping)
        {
            OnTriggerEnter(other);
        }
    }
    void CheckInput()
    {
        mL = false;
        mR = false;
        if (Input.GetButton("MoveLeft"))
        {
            mL = true;
        }
        else if (Input.GetButton("MoveRight"))
        {
            mR = true;
        }
        if (canJump && !isJumping && Input.GetButton("Jump"))
        {
            isJumping = true;
            //jumpCount = jumpTimer;
            rigid.AddRelativeForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SwitchProj();
        }
        else
        {
            /*
            if (jumpCount > 0)
            {
                jumpCount -= Time.deltaTime;
            }
            else if (jumpCount < 0 && jumpCount != 0)
            {
                jumpCount = 0;
            }
            */
        }

    }
    void Movement()
    {
        Vector3 newVel = Vector3.zero;
        newVel.y = rigid.velocity.y;
        if (mL && !RayCastSide(-1))
        {
            newVel.x = -maxVelocityX;
        }
        else if (mR && !RayCastSide(1))
        {
            newVel.x = maxVelocityX;
        }
        rigid.velocity = newVel;
    }
    public bool RayCastSide(float leftOrRight)
    {
        // right = 1    left = -1
        if (leftOrRight > 0.0f || leftOrRight < 0.0f)
        {
            Vector3 rayDir;
            if (leftOrRight > 0.0f)
            {
                rayDir = Vector3.right;
            }
            else
            {
                rayDir = Vector3.left;
            }

            RaycastHit hitInfo;
            Vector3 checkPosStart = Vector3.zero;
            checkPosStart.x = transform.position.x + 0.5f * leftOrRight;
            checkPosStart.y = transform.position.y - 0.99f;
            for (float i = 0; i < 1f; i += 0.05f)
            {
                Physics.Raycast(checkPosStart, rayDir, out hitInfo, 0.1f, layerMask);
                if (hitInfo.collider != null)
                {
                    return true;
                }
                checkPosStart.y += 0.1f;
            }

            //Debug.Log(hitInfo.collider);

        }
        return false;
    }
    public void DoDmg()
    {
        if (canTakeDmg)
        {
            
            canTakeDmg = false;
            hp--;
            Healthbar[hp].SetActive(false);
            Invoke("ResetCanTakeDmg", invTime);
            Invoke("ResetScreenColor", 0);
            damageImage.color = flashColour;
        }
    }
    
    void ResetCanTakeDmg()
    {
        canTakeDmg = true;
    }
    public Vector3 CheckPoint
    {
        set
        {
            checkpoint = value + Vector3.up * 2f;
        }
    }
    void ResetScreenColor()
    {
        var timer =0f;

        timer += Time.deltaTime*0.7f;
        if (timer < invTime)
        {      
        damageImage.color = Color.Lerp(damageImage.color, Color.clear, Mathf.MoveTowards(0, 1, timer));
        }

        
    }
            


    void CheckHealth()
    {

    }

    public void Respawn()
    {
        //set hp to 5
        hp = startinghp;
        //reactivate health images
        for (int j = 0; j < hp; j++)
        {
            Healthbar[j].SetActive( true );
        }
        ReturnToCheckPoint();
    }
    private void SwitchProj()
    {
        switch (projectileType)
        {
            case ProjectileType.Horizontal:
                gunRend.material.color = Color.green;
                projectileType = ProjectileType.Verticle;
                break;
            case ProjectileType.Verticle:
                gunRend.material.color = Color.blue;
                projectileType = ProjectileType.Horizontal;
                break;
        }
    }
    public bool CanMove
    {
        get
        {
            return canMove;
        }
        set
        {
            canMove = value;
        }
    }
}
