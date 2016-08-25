using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    Rigidbody rigid;
    bool mL = false;
    bool mR = false;
    bool canJump = false;
    bool isJumping = false;
    float maxVelocityX = 5f;
    float jumpForce = 5f;
    int layerMask;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        layerMask = 1 << LayerMask.NameToLayer("Floor");
    }

    void Update()
    {
        CheckInput();
        //Debug.Log(rigid.velocity);
    }
    void FixedUpdate()
    {
        Movement();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            canJump = true;
            isJumping = false;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Floor"))
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
            rigid.AddRelativeForce(Vector3.up * jumpForce, ForceMode.Impulse);
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
            for(float i = 0; i < 1f; i += 0.1f)
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

}
