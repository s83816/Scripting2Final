using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PreAimBullet : MonoBehaviour
{
    
    Vector3 Endposition;
    Vector3 startPosition;
    float moveImpulse = 80f;
    float distanceTravel = 0;
    float totalDis = 0;
    float spawnTrailDistance = 0.5f;
    private List<GameObject> spawnedTrails = new List<GameObject>();
    void OnEnable()
    {
        GetComponent<Rigidbody>().drag = 0.2f;
        GetComponent<Rigidbody>().isKinematic = false;
        startPosition = transform.position;
        startPosition.z = 0f;
        Endposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Endposition.z = 0f;
        Vector3 dir = ((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position)).normalized;
        dir.z = 0f;
        GetComponent<Rigidbody>().AddForce(dir * moveImpulse, ForceMode.Impulse);
    }
    void OnDisable()
    {
        distanceTravel = 0;
        totalDis = 0;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        CancelInvoke();
    }
    void Update()
    {
        if (Vector3.Distance(transform.position, Endposition) < 0.3f)
        {
            Disable();
        }
        else if (GetComponent<Rigidbody>().velocity.magnitude < 2f)
        {
            Disable();
        }
        else if (Vector3.Distance(transform.position, Gun.Instance.transform.position) >= 15f)
        {
            Disable();
        }
        else if (totalDis > 15f)
        {
            Disable();
        }
    }
    void Disable()
    {
        gameObject.SetActive(false);
        Gun.Instance.PreAim();
    }
    void FixedUpdate()
    {
        totalDis += GetComponent<Rigidbody>().velocity.magnitude * Time.fixedDeltaTime;
        if (distanceTravel >= spawnTrailDistance)
        {
            distanceTravel = 0;
            SpawnTrail();
        }
        else
        {
            distanceTravel += GetComponent<Rigidbody>().velocity.magnitude * Time.fixedDeltaTime;
        }
    }
    void SpawnTrail()
    {
        GameObject tempTrail = Gun.Instance.GetTrail;
        if (tempTrail != null)
        {
            tempTrail.transform.position = transform.position;
            tempTrail.SetActive(true);
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Floor"))
        {
            GetComponent<Rigidbody>().drag = 1f;
        }
        if (other.collider.CompareTag("MovingPlatform"))
        {
            Disable();
        }
        if (other.collider.CompareTag("EnemyBullet"))
        {
            other.collider.GetComponent<EnemyBullet>().IsHit();
        }
    }
}
