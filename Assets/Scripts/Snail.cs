using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Snail : MonoBehaviour
{
    [SerializeField] private Rigidbody2D bulletPrefab;
    [SerializeField] private Transform currentGun;

    private PlayerUI playerUI;
    private SnailHealth snailHealth;
    private PhotonView view;
    private Camera mainCam;
    private Vector3 diff;
    private Animator animator;

    public int snailID;
    public float walkSpeed = 1f;
    public float missileForce = 5f;

    void Start()
    {
        playerUI = GetComponent<PlayerUI>();
        snailHealth = GetComponent<SnailHealth>();
        view = GetComponent<PhotonView>();
        mainCam = Camera.main;
        animator = GetComponent<Animator>(); // Animator referansı alınıyor

        if (playerUI != null)
        {
            playerUI.UpdateHealth(snailHealth.health, snailHealth.maxHealth);
        }
    }

    void Update()
    {
        if (!IsTurn) return;

        if (view.IsMine)
        {
            RotateGun();
            MovementAndShooting();
        }

        if (transform.position.y < -6)
        {
            Die();
        }
    }

    public bool IsTurn => SnailManager.instance.IsMyTurn(snailID);

    private void Die()
    {
        snailHealth.ChangeHealth(-snailHealth.maxHealth);
        SnailManager.instance.RemoveSnail(gameObject);
    }

    void RotateGun()
    {
        diff = mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();
        float rot_Z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        currentGun.rotation = Quaternion.Euler(0, 0, rot_Z + 180f);
    }

    void MovementAndShooting()
    {
        float hor = Input.GetAxis("Horizontal");

        if (hor != 0)
        {
            transform.position += Vector3.right * hor * Time.deltaTime * walkSpeed;
            view.RPC("SetWalkingAnimation", RpcTarget.All, true); // Tüm istemcilerde yürüme animasyonunu başlat
        }
        else
        {
            view.RPC("SetWalkingAnimation", RpcTarget.All, false); // Durma animasyonunu başlat
        }

        if (Input.GetKeyDown(KeyCode.Q)) // Ateş Etme
        {
            Rigidbody2D bullet = Instantiate(bulletPrefab, currentGun.position - currentGun.right, currentGun.rotation);
            bullet.AddForce(-currentGun.right * missileForce, ForceMode2D.Impulse);

            if (IsTurn)
            {
                SnailManager.instance.NextSnail();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            snailHealth.ChangeHealth(-10);

            if (playerUI != null)
            {
                playerUI.UpdateHealth(snailHealth.health, snailHealth.maxHealth);
            }

            if (IsTurn)
            {
                SnailManager.instance.NextSnail();
            }
        }
    }

    // 🟢 **RPC ile Tüm Oyunculara Animasyon Senkronizasyonu**
    [PunRPC]
    void SetWalkingAnimation(bool isWalking)
    {
        animator.SetBool("isWalking", isWalking);
    }

    // 🔄 **Sırası Gelen Oyuncunun Çerçevesini Parlatma**
    public void SetTurnIndicator(bool isActive)
    {
        if (playerUI != null)
        {
            playerUI.HighlightTurn(isActive);
        }
    }
}
