using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("hit");
            //PlayerControl myPlayer = (PlayerControl)GameObject.Find("Player").GetComponent("PlayerControl");
            PlayerControl.Instance.CheckPoint = transform.position;
            //myPlayer.checkpoint = other.transform.position;
        }
    }
}