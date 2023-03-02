using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance_ = null;
    public static UIManager Instance
    {
        get
        {
            if(instance_ == null)
            {
                instance_ = new UIManager();
            }
            return instance_;
        }
    }

    public PopupUI pausePopup_;
    public KeyCode pauseKey_ = KeyCode.Escape;

    public delegate void EventHandler();
    public EventHandler onClosePopup;
    public static bool GameIsPaused = false;
    private UIManager()
    {

    }
    void Awake()
    {
        Init();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Init()
    {
        GameObject UiObjs = GFunc.GetRootObj("UiObjs");
        ClosePopup(pausePopup_);
        onClosePopup = new EventHandler(ClosePauseUI);
    }

    // Update is called once per frame
    void Update()
    {
        ToggleKeyDownAction(pauseKey_, pausePopup_);
    }
    private void ToggleKeyDownAction(in KeyCode key, PopupUI popup)
    {
        if(Input.GetKeyDown(key))
            ToggleOpenClosePopup(popup);
    }
    private void ToggleOpenClosePopup(PopupUI popup)
    {
        if(!popup.gameObject.activeSelf)
        {
            OpenPopup(popup);
        }
        else
        {
            ClosePopup(popup);
        }
    }
    private void OpenPopup(PopupUI popup)
    {
        popup.gameObject.SetActive(true);
        Pause();
    }
    private void ClosePopup(PopupUI popup)
    {
        popup.gameObject.SetActive(false);
        Resume();
    }
    public void ClosePauseUI()
    {
        ClosePopup(pausePopup_);
    }
    public void Pause()
    {
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
}
