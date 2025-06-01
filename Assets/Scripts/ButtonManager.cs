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
        // Photon ba�lant�s�n� kopar
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }

        // Ana men� sahnesine d�n
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }
}
