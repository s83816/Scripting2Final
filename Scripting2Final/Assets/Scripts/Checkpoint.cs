using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    public Vector3 RespawnPoint;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControl.Instance.CheckPoint = transform.position;
        }
    }
}