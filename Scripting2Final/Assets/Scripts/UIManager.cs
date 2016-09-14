using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
    private static UIManager myInstance;
    public GameObject endScreen;
    public static UIManager Instance
    {
        get
        {
            if (myInstance == null)
            {
                myInstance = FindObjectOfType<UIManager>();
            }
            return myInstance;
        }
    }
    void Start()
    {
        if (endScreen != null)
        {
            endScreen.SetActive(false);
        }
    }
    public void LevelComplete()
    {
        if (endScreen != null)
        {
            endScreen.SetActive(true);
        }
        PlayerControl.Instance.CanMove = false;
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void PlayAgain()
    {
        Application.LoadLevel(("level01"));
        
    }
}
