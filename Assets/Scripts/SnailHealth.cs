using UnityEngine;
using UnityEngine.UI;

public class SnailHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;

    [SerializeField]
    private Text healthTxt;

    private PlayerUI playerUI; // Oyuncunun UI'sine eri�mek i�in

    void Start()
    {
        health = maxHealth;
        playerUI = GetComponent<PlayerUI>(); // Oyuncunun UI bile�enine eri�

        if (healthTxt != null)
            healthTxt.text = health.ToString();
        else
            Debug.LogWarning("Health UI metni eksik!");

        // **Ba�lang��ta can bar�n� g�ncelle**
        if (playerUI != null)
        {
            playerUI.UpdateHealth(health, maxHealth);
        }
    }

    public void ChangeHealth(int change)
    {
        health += change;

        if (health > maxHealth)
            health = maxHealth;
        else if (health <= 0)
        {
            health = 0;
            Die();
        }

        if (healthTxt != null)
            healthTxt.text = health.ToString();

        // **Can bar�n� UI'da g�ncelle**
        if (playerUI != null)
        {
            playerUI.UpdateHealth(health, maxHealth);
        }
    }

    private void Die()
    {
        SnailManager.instance.RemoveSnail(gameObject);
        Destroy(gameObject);
    }
}
