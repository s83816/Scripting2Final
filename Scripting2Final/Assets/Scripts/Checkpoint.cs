using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    public Vector3 RespawnPoint;
    public bool isGoal = false;
    Material mat;
    void Start()
    {
        if (isGoal)
        {
            mat = GetComponentInChildren<Renderer>().material;
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.4f);
        }
        else
        {
            mat = GetComponentInChildren<Renderer>().material;
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.1f);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isGoal)
            {
                UIManager.Instance.LevelComplete();
            }
            else
            {
                PlayerControl.Instance.CheckPoint = transform.position;
                ResetAll();
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.2f);
            }
        }      
    }
    void ResetAll()
    {
        Checkpoint[] tempAllCheckPoints = FindObjectsOfType<Checkpoint>();
        foreach(Checkpoint c in tempAllCheckPoints)
        {
            if (!c.isGoal)
            {
                c.Mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.1f);
            }
        }
    }
    public Material Mat
    {
        get
        {
            return mat;
        }
        set
        {
            mat = value;
        }
    }
}