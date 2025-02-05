using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab; // Oyuncu prefab'i
    public Transform[] spawnPoints; // Sabit spawn noktalar�

    private void Start()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("HATA: playerPrefab atanmam��!");
            return;
        }

        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        playerIndex = playerIndex % spawnPoints.Length;

        Vector3 spawnPosition = spawnPoints[playerIndex].position;
        GameObject spawnedPlayer = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);

        if (spawnedPlayer.GetComponent<Snail>() == null)
        {
            Debug.LogError("HATA: Spawlanan prefab 'Snail' bile�eni i�ermiyor!");
        }
    }

}