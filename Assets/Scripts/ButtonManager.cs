using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void LoadScene1()
    {
        SceneManager.LoadScene("Loading");
    }
    public void GoToMainMenu()
    {
        // Photon baðlantýsýný kopar
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }

        // Ana menü sahnesine dön
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }
}
