using UnityEngine;
using System.Collections;

public class FollowingPlatform : MonoBehaviour
{
    float moveSpeed = 3.2f;
    int layerMask;
    Vector3 targetLoc;
    Vector3 startLoc;
    bool canMove = false;

    void Awake()
    {
        layerMask = 1 << LayerMask.NameToLayer("Floor");
    }
    void Start()
    {
        startLoc = transform.position;
        canMove = true;
        GetComponent<Renderer>().material.color = Color.green;
    }
    void SetCanMove()
    {
        canMove = true;
    }
    void Update()
    {
        if (canMove)
        {
            targetLoc = FindObjectOfType<PlayerControl>().transform.position;
            targetLoc.z = startLoc.z;
            targetLoc.y = startLoc.y;
            Vector3 dir = (targetLoc - transform.position).normalized;
            if (!RayCastSide(dir.x))
            {
                if (Vector3.Distance(transform.position, targetLoc) > 3f)
                {
                    targetLoc.x =  transform.position.x + dir.x * 3f;
                    //Debug.Log(targetLoc.x);
                }
                Vector3 newPos = Vector3.Slerp(transform.position, targetLoc, Time.deltaTime * moveSpeed);
                newPos.z = startLoc.z;
                newPos.y = startLoc.y;
                transform.position = newPos;
            }
        }

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
            checkPosStart.x = transform.position.x + (transform.localScale.x / 2 * leftOrRight);
            checkPosStart.y = transform.position.y - transform.localScale.y / 2;
            for (float i = 0; i < 0.6f; i += 0.2f)
            {
                Physics.Raycast(checkPosStart, rayDir, out hitInfo, 0.2f, layerMask);
                if (hitInfo.collider != null)
                {
                    if (hitInfo.collider.CompareTag("PlatformH"))
                    {
                        return false;
                    }
                    return true;
                }
                checkPosStart.y += 0.1f;
            }
        }
        return false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlatformH"))
        {
            other.GetComponentInParent<PlatformBullets>().Disable();           
        }
    }

}
