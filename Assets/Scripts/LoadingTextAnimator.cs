using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingTextAnimator : MonoBehaviour
{
    public Text loadingText; // Legacy UI Text bileþeni
    private string baseText = "LOADING"; // Temel metin
    private int dotCount = 0; // Kaç nokta olduðunu takip eder

    void Start()
    {
        StartCoroutine(AnimateLoadingText());
    }

    IEnumerator AnimateLoadingText()
    {
        while (true)
        {
            // Nokta sayýsýný artýr (0'dan 3'e kadar)
            dotCount = (dotCount + 1) % 4;

            // Temel metni ve noktalarý birleþtir
            string dots = new string('.', dotCount);
            loadingText.text = baseText + dots;

            // Her güncelleme arasýnda 0.5 saniye bekle
            yield return new WaitForSeconds(0.5f);
        }
    }
}
