using UnityEngine;
using System.Collections;

public class Trail : MonoBehaviour
{

    void OnEnable()
    {
        Invoke("Disable", 0.5f);
    }
    void OnDisable()
    {
        CancelInvoke();
    }
    void Disable()
    {
        gameObject.SetActive(false);
    }
}
