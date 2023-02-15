using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public static partial class GFunc
{
    //! ������ �����ϴ� �Լ�
    public static void QuitThisGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }       // QuitThisGame()

    //! �ٸ� ���� �ε��ϴ� �Լ�
    public static void LoadScene(string sceneName_)
    {
        SceneManager.LoadScene(sceneName_);
    }       // LoadScene()
    public static void Pause()
    {
        Time.timeScale = 0;
    }   // Pause()
    public static void ReleasePause()
    {
        Time.timeScale = 1;
    }   // ReleasePause()
}
