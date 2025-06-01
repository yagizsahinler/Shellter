using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingTextAnimator : MonoBehaviour
{
    public Text loadingText; // Legacy UI Text bile�eni
    private string baseText = "LOADING"; // Temel metin
    private int dotCount = 0; // Ka� nokta oldu�unu takip eder

    void Start()
    {
        StartCoroutine(AnimateLoadingText());
    }

    IEnumerator AnimateLoadingText()
    {
        while (true)
        {
            // Nokta say�s�n� art�r (0'dan 3'e kadar)
            dotCount = (dotCount + 1) % 4;

            // Temel metni ve noktalar� birle�tir
            string dots = new string('.', dotCount);
            loadingText.text = baseText + dots;

            // Her g�ncelleme aras�nda 0.5 saniye bekle
            yield return new WaitForSeconds(0.5f);
        }
    }
}
