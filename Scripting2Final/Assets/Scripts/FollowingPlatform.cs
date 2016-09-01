using UnityEngine;
using System.Collections;

public class FollowingPlatform : MonoBehaviour
{
    float moveSpeed = 5f;
    int layerMask;
    Vector3 playerLoc;
    void Awake()
    {
        layerMask = 1 << LayerMask.NameToLayer("Floor");
    }
    void Start()
    {

    }

    void Update()
    {
        
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
            for (float i = 0; i < 0.6f; i += 0.1f)
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
