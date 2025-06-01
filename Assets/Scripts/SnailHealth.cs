using UnityEngine;
using UnityEngine.UI;

public class SnailHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;

    [SerializeField]
    private Text healthTxt;

    private PlayerUI playerUI; // Oyuncunun UI'sine eriþmek için

    void Start()
    {
        health = maxHealth;
        playerUI = GetComponent<PlayerUI>(); // Oyuncunun UI bileþenine eriþ

        if (healthTxt != null)
            healthTxt.text = health.ToString();
        else
            Debug.LogWarning("Health UI metni eksik!");

        // **Baþlangýçta can barýný güncelle**
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

        // **Can barýný UI'da güncelle**
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
