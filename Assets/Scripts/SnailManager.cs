using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnailManager : MonoBehaviour
{
    
    public static SnailManager instance;

    private Snail[] snails;

    private Transform snailCamera;

    private int currentSnail;

    private Vector2 minBounds = new Vector2(-0.1f, -0.02f); // Sol-alt köþe
    private Vector2 maxBounds = new Vector2(0.1f, 0.03f);  // Sað-üst köþe

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);  // Yanlýþ instance'ý yok et
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Geçerli instance kalýcý olsun
        }
    }

    private void Start()
    {
        StartCoroutine(InitializeSnails());
    }

    void LateUpdate()
    {
        if (snails != null && snails.Length > 0 && snails[currentSnail] != null)
        {
            Vector3 targetPosition = snails[currentSnail].transform.position;

            // Kamera pozisyonunu sýnýrlayalým (isteðe baðlý)
            targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);
            targetPosition.z = -10f; // Kamera Z ekseni sabit kalýr

            // Kamera pozisyonunu pürüzsüz bir þekilde hareket ettir
            if (snailCamera != null)
            {
                snailCamera.position = Vector3.Lerp(snailCamera.position, targetPosition, Time.deltaTime * 5f);
            }
        }
    }



    private IEnumerator InitializeSnails()
    {
        yield return new WaitForSeconds(1f); // Oyuncularýn spawn olmasý için bekle

        snails = GameObject.FindObjectsByType<Snail>(FindObjectsSortMode.None);

        if (snails.Length == 0)
        {
            Debug.LogError("Snail nesneleri bulunamadý! Spawn iþleminin tamamlandýðýndan emin olun.");
            yield break;
        }

        snailCamera = Camera.main?.transform;
        if (snailCamera == null)
        {
            Debug.LogError("Ana kamera bulunamadý!");
            yield break;
        }

        for (int i = 0; i < snails.Length; i++)
        {
            snails[i].snailID = i;
        }

        NextSnail();
    }


    public bool IsMyTurn(int i)
    {
        return i == currentSnail;
    }

    public void NextSnail()
    {
        StartCoroutine(_NextSnail());
    }

    public IEnumerator _NextSnail()
    {
        yield return new WaitForSeconds(2f);

        int previousSnail = currentSnail;
        currentSnail = (currentSnail + 1) % snails.Length;

        // Önceki oyuncunun çerçevesini eski haline getir
        if (snails[previousSnail] != null)
        {
            snails[previousSnail].GetComponent<Snail>()?.SetTurnIndicator(false);
        }

        // Yeni oyuncunun çerçevesini parlak yap
        if (snails[currentSnail] != null)
        {
            snails[currentSnail].GetComponent<Snail>()?.SetTurnIndicator(true);
        }
    }


    public void RemoveSnail(GameObject snail)
    {
        List<Snail> tempSnailList = new List<Snail>(snails);
        tempSnailList.Remove(snail.GetComponent<Snail>());

        snails = tempSnailList.ToArray();

        if (snails.Length == 1)
        {
            SceneManager.LoadScene("Game Over");
            return;
        }

        if (snails.Length == 0)
        {
            SetDefaultCameraPosition();
            SceneManager.LoadScene("GameOver");
            return;
        }

        currentSnail = 0;
        if (snails[currentSnail] != null && snails[currentSnail].gameObject.activeSelf)
        {
            snailCamera.SetParent(snails[currentSnail].transform);
            snailCamera.localPosition = Vector3.zero + Vector3.back * 10f;
        }
    }


    private void SetDefaultCameraPosition()
    {
        // Kamera baðlantýsýný kaldýr ve varsayýlan bir pozisyona yerleþtir
        snailCamera.SetParent(null);
        snailCamera.position = new Vector3(0, 0, -10); // Kamerayý sahnenin ortasýna yerleþtir
    }


}
