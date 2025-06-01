using UnityEngine;
using Photon.Pun;
using UnityEngine.UI; // UI öðelerini eklemek için

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField usernameInput;
    public InputField createInput;
    public InputField joinInput;
    public UnityEngine.UI.Text warningText; // Text sýnýfýný UI'dan açýkça belirtiyoruz

    private void Start()
    {
        warningText.text = "";
    }

    public void SetUsername()
    {
        if (string.IsNullOrEmpty(usernameInput.text))
        {
            warningText.text = "Please enter a username!";
            return;
        }

        PhotonNetwork.NickName = usernameInput.text;
        warningText.text = "Username saved!";
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(PhotonNetwork.NickName))
        {
            warningText.text = "Set your username first!";
            return;
        }

        if (string.IsNullOrEmpty(createInput.text))
        {
            warningText.text = "Enter the room name!";
            return;
        }

        PhotonNetwork.CreateRoom(createInput.text);
    }

    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(PhotonNetwork.NickName))
        {
            warningText.text = "Set your username first!";
            return;
        }

        if (string.IsNullOrEmpty(joinInput.text))
        {
            warningText.text = "Enter a room name to join!";
            return;
        }

        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }
}
