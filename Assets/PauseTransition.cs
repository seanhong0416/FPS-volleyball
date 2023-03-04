using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseTransition : MonoBehaviour
{
    public GameObject Menu;
    public static bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        Menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) && !isPaused)
        {
            Debug.Log("pause key pressed");
            Transition(true);
            Cursor.lockState = CursorLockMode.None;
        } 
        else if(Input.GetKeyDown(KeyCode.P) && isPaused)
        {
            Transition(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void Transition(bool state)
    {
        isPaused = state;
        Menu.SetActive(state);
    }
}
