using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Text playerNameText;  // TMP_Text (Eğer eski Text kullanıyorsan: public Text playerNameText;)
    public Slider healthSlider;
    public Image playerFrame;

    private Color originalFrameColor;
    private Color activeTurnColor = Color.yellow; // Sırası gelen oyuncunun çerçevesi sarı olacak

    private void Start()
    {
        if (playerFrame != null)
        {
            originalFrameColor = playerFrame.color;
        }

        // UI öğeleri atanmış mı kontrol edelim
        if (playerNameText == null) Debug.LogError("Player Name Text atanmamış!");
        if (healthSlider == null) Debug.LogError("Health Slider atanmamış!");

        // Sağlık barı başlangıç değeri
        if (healthSlider != null)
        {
            healthSlider.maxValue = 100; // Varsayılan max can
            healthSlider.value = 100;
        }
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        else
        {
            Debug.LogError("Health slider eksik!");
        }
    }

    public void SetPlayerName(string playerName)
    {
        if (playerNameText != null)
        {
            playerNameText.text = playerName;
        }
    }

    public void HighlightTurn(bool isActive)
    {
        if (playerFrame != null)
        {
            playerFrame.color = isActive ? activeTurnColor : originalFrameColor;
        }
    }
}
