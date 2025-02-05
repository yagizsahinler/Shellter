using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void LoadScene1()
    {
        SceneManager.LoadScene("Loading");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainScene"); // Ana menüye dön
    }
}
