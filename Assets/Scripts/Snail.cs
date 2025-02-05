using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Snail : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D bulletPrefab;

    [SerializeField]
    private Transform currentGun;

    public float walkSpeed = 1f;
    public float maxRealtiveVelocity = 6f;
    public float misileForce = 5f;

    public int snailID;

    private SpriteRenderer spriteRenderer;

    private Camera mainCam;

    private Vector3 diff;

    public bool IsTurn { get { return SnailManager.instance.IsMyTurn(snailID); } }
    private SnailHealth snailHealth;

    private Animator animator;

    PhotonView view;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        //can scripti
        snailHealth = GetComponent<SnailHealth>();

        mainCam = Camera.main;

        animator = GetComponent<Animator>();

        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
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
            Die(); // Y Ekseni sýnýrýný aþýnca ölümü tetikler
        }

    }

    private void Die()
    {
        snailHealth.ChangeHealth(-snailHealth.maxHealth); // Caný sýfýra indirir
        SnailManager.instance.RemoveSnail(gameObject); // Yöneticiye bildir
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
            // Yürüme animasyonunu baþlat
            animator.SetBool("isWalking", true);
            transform.position += Vector3.right * hor * Time.deltaTime * walkSpeed;
            spriteRenderer.flipX = hor < 0;
        }
        else
        {
            // Karakter hareketsizse yürüme animasyonunu durdur
            animator.SetBool("isWalking", false);
        }

        // Ateþ Etme Kontrolü
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Rigidbody2D bullet = Instantiate(bulletPrefab, currentGun.position - currentGun.right, currentGun.rotation);
            bullet.AddForce(-currentGun.right * misileForce, ForceMode2D.Impulse);

            animator.SetBool("isShooting", true);
            Invoke("StopShooting", 0.5f);

            if (IsTurn)
            {
                SnailManager.instance.NextSnail();
            }
        }
    }

    void StopShooting()
    {
        animator.SetBool("isShooting", false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            snailHealth.ChangeHealth(-10);

            if (IsTurn)
                SnailManager.instance.NextSnail();
        }
    }

}
