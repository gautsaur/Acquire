
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeOutToMenu : MonoBehaviour
{
    public void changeScene()
    {
        SceneManager.LoadScene("PreMenu");
    }
}
