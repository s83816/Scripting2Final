using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour
{
    bool canDoDmg = true;
    Rigidbody rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        GetComponent<Renderer>().material.color = Color.red;
    }
    void OnDisable()
    {
        if (rigid == null)
            rigid = GetComponent<Rigidbody>();
        canDoDmg = true;
        rigid.velocity = Vector3.zero;
        CancelInvoke();
    }
    void OnEnable()
    {
        Invoke("Disable", 2f);
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            if (canDoDmg)
            {
                other.collider.GetComponent<PlayerControl>().DoDmg();
                canDoDmg = false;
                Disable();
            }
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            Disable();
        }
    }
    void Disable()
    {
        gameObject.SetActive(false);
    }
    public void IsHit()
    {
        Disable();
    }
}
