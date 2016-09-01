using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    public Vector3 RespawnPoint;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            PlayerControl myPlayer = (PlayerControl)GameObject.Find("Player").GetComponent("PlayerControl");
            myPlayer.checkpoint = other.transform.position;
        }
    }
}