using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    public Vector3 checkpoint;
    public Vector3 Startpoint;
    Rigidbody rigid;
    public int startinghp = 5;
    private int hp;
    bool mL = false;
    bool mR = false;
    bool canJump = false;
    bool isJumping = false;

    bool canTakeDmg = true;
    float maxVelocityX = 5f;
    float jumpForce = 10f;
    float jumpTimer = 0.5f;
    float invTime = 1f;
    public float jumpCount = 0;
    int layerMask;

    public GameObject[] Healthbar;
    void Start()
    {
        
        Healthbar = new GameObject[5];
        for(int j = 0; j < Healthbar.Length; j++)
        { 
            Healthbar[j] = GameObject.Find("heart"+j.ToString());
        }

        rigid = GetComponent<Rigidbody>();
        layerMask = 1 << LayerMask.NameToLayer("Floor");
        Startpoint = transform.position;
    }

    void Update()
    {
        //respawn test 
        if (Input.GetKeyDown(KeyCode.R))
        {
            rigid.velocity = Vector3.zero;
            if (checkpoint != Vector3.zero)
                transform.position = checkpoint;
            else
                transform.position = Startpoint;
        }

        if (hp <= 0)
            Respawn();
        CheckInput();
        //Debug.Log(rigid.velocity);
    }
    void FixedUpdate()
    {
        Movement();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor")|| other.CompareTag("MovingPlatform"))
        {
            canJump = true;
            isJumping = false;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if ((other.CompareTag("Floor")|| other.CompareTag("MovingPlatform")) && !isJumping)
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
            for(float i = 0; i < 1f; i += 0.05f)
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
        }
    }
    void ResetCanTakeDmg()
    {
        canTakeDmg = true;
    }

    public void Respawn()
    {
        //set hp to 5
        hp = startinghp;
        //reactivate health images
        for (int j = 0; j < hp; j++)
            Healthbar[j].SetActive(true);

        //stop velocity to stop bouncing when respawning
        rigid.velocity = Vector3.zero;

        //if checkpint is set spawn to checkpoint
            if (checkpoint != Vector3.zero)
                transform.position = checkpoint;
            else
                transform.position = Startpoint;
    }

}
