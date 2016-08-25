using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    GameObject target;
    float yFalloff = 2f;
    float mSpeed = 4f;
    float maxX = 5f;
    void Start()
    {
        target = FindObjectOfType<PlayerControl>().gameObject;
    }

    void LateUpdate()
    {
        Vector3 targetPos = target.transform.position;
        targetPos.z = transform.position.z;
        targetPos.y += yFalloff;
        if (targetPos.x > 0)
        {
            targetPos.x = Mathf.Min(targetPos.x, maxX);
        }
        else if (targetPos.x < 0)
        {
            targetPos.x = Mathf.Max(targetPos.x, -maxX);
        }
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * mSpeed);
    }
}
