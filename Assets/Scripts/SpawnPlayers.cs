using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab; // Oyuncu prefab'i
    public Transform[] spawnPoints; // Sabit spawn noktalarý

    private void Start()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("HATA: playerPrefab atanmamýþ!");
            return;
        }

        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        playerIndex = playerIndex % spawnPoints.Length;

        Vector3 spawnPosition = spawnPoints[playerIndex].position;
        GameObject spawnedPlayer = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);

        if (spawnedPlayer.GetComponent<Snail>() == null)
        {
            Debug.LogError("HATA: Spawlanan prefab 'Snail' bileþeni içermiyor!");
        }
    }

}