using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseTransition : MonoBehaviour
{
    public GameObject Menu;
    public GameObject SettingMenu;
    public static bool isPaused;
    bool isP;
    bool isO;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        Menu.SetActive(false);
        isP = false;
        isO = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.P) && !isPaused)
        {
            Pause();
        }
        else if (Input.GetKeyDown(KeyCode.P) && isPaused && isP)
        {
            UnPause();
        }
        

        if(Input.GetKeyDown(KeyCode.O) && !isPaused)
        {
            Setting();
        }
        else if(Input.GetKeyDown(KeyCode.O) && isPaused && isO)
        {
            UnSetting();
        }
    }




    void Transition(bool state)
    {
        isPaused = state;
        Menu.SetActive(state);
        isP = state;
    }

    public void Pause()
    {
        Debug.Log("pause key pressed");
        Transition(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void UnPause()
    {
        Transition(false);
        Cursor.lockState = CursorLockMode.Locked;
    }




    void SettingTransition(bool state)
    {
        isPaused = state;
        SettingMenu.SetActive(state);
        isO = state;
    }

    public void Setting()
    {
        Debug.Log("setting key pressed");
        SettingTransition(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void UnSetting()
    {
        SettingTransition(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
