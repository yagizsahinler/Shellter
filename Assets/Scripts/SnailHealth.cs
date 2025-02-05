using UnityEngine;
using UnityEngine.UI;

public class SnailHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;

    [SerializeField]
    private Text healthTxt;

    void Start()
    {
        health = maxHealth;

        if (healthTxt != null)
            healthTxt.text = health.ToString();
        else
            Debug.LogWarning("Health UI metni eksik!");
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
    }

    private void Die()
    {
        SnailManager.instance.RemoveSnail(gameObject);
        Destroy(gameObject);
    }
}