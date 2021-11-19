using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class PlayerAI : MonoBehaviour
{

    [SerializeField]
    private int hitPoint = 25;
    [SerializeField]
    private GameObject headShotFX = null;
    [SerializeField]
    private GameObject bodyShotFX = null;
    [SerializeField]
    private GameObject concrete = null;
    [SerializeField]
    private GameObject road = null;
    [SerializeField]
    private GameObject metal = null;
    [SerializeField]
    private GameObject wood = null;

    [SerializeField]
    private Camera fpsCamera = null;

    private int bulletCount = 30;
    private float timeBetweenBullets = 0.18f;
    private float timeBetweenLastBullet = 0.0f;
    private bool isActive = true;
    private bool once = true;

    private loadingtext loadingText = null;

    public bool IsActive { get => isActive; set => isActive = value; }

    private void Start()
    {
        loadingText = FindObjectOfType<loadingtext>();
        GetComponent<ZombieHealthSystem>().SetMaxHealth(100);
    }

    void Update()
    {
        timeBetweenLastBullet += Time.deltaTime;

        if(isActive)
        {
            if (Input.GetButton("Fire1") && timeBetweenLastBullet > timeBetweenBullets && bulletCount > 0 && !loadingText.GetIsReloading())
            {
                Shoot();
                timeBetweenLastBullet = 0;
            }
            else if (bulletCount == 0)
            {
                if (loadingText != null && !loadingText.GetIsReloading())
                    loadingText.GunReload();
            }
        }
        else if(!isActive && once)
        {
            Die();
        }


    }

    private void Shoot()
    {
        bulletCount--;
        loadingText.bulletCount.text = bulletCount.ToString();

        RaycastHit hit;
        if(Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit))
        {
            transform.GetComponentInChildren<ParticleSystem>().Play(true);

            if (hit.transform.name == "Z_Head")
            {
                hit.transform.parent.GetComponent<ZombieHealthSystem>().Hit(2.5f * hitPoint);

                GameObject fx = Instantiate(headShotFX, hit.point, Quaternion.LookRotation(-fpsCamera.transform.forward), transform) as GameObject;
                fx.GetComponentInChildren<ParticleSystem>().Play(true);
                hit.transform.parent.GetComponent<ZombieAI>().OnHit(fx.GetComponentInChildren<ParticleSystem>().main.duration);
                Destroy(fx, fx.GetComponentInChildren<ParticleSystem>().main.duration);
            }
            else if(hit.transform.name == "Z_Body")
            {
                hit.transform.parent.GetComponent<ZombieHealthSystem>().Hit(hitPoint);

                GameObject fx = Instantiate(bodyShotFX, hit.point, Quaternion.LookRotation(-fpsCamera.transform.forward), transform) as GameObject;
                fx.GetComponent<ParticleSystem>().Play(true);
                hit.transform.parent.GetComponent<ZombieAI>().OnHit(fx.GetComponent<ParticleSystem>().main.duration);
                Destroy(fx, fx.GetComponent<ParticleSystem>().main.duration);
            }
            else if(hit.transform.CompareTag("Wood"))
            {
                GameObject fx = Instantiate(wood, hit.point, Quaternion.LookRotation(-fpsCamera.transform.forward), transform) as GameObject;
                fx.GetComponent<ParticleSystem>().Play(true);
                Destroy(fx, fx.GetComponent<ParticleSystem>().main.duration);
            }
            else if (hit.transform.CompareTag("Concrete") || hit.transform.parent.CompareTag("Concrete"))
            {
                GameObject fx = Instantiate(concrete, hit.point, Quaternion.LookRotation(-fpsCamera.transform.forward), transform) as GameObject;
                fx.GetComponent<ParticleSystem>().Play(true);
                Destroy(fx, fx.GetComponent<ParticleSystem>().main.duration);
            }
            else if (hit.transform.CompareTag("Metal") || hit.transform.parent.CompareTag("Metal"))
            {
                GameObject fx = Instantiate(metal, hit.point, Quaternion.LookRotation(-fpsCamera.transform.forward), transform) as GameObject;
                fx.GetComponent<ParticleSystem>().Play(true);
                Destroy(fx, fx.GetComponent<ParticleSystem>().main.duration);
            }
            else if (hit.transform.CompareTag("Road"))
            {
                GameObject fx = Instantiate(road, hit.point, Quaternion.LookRotation(-fpsCamera.transform.forward), transform) as GameObject;
                fx.GetComponent<ParticleSystem>().Play(true);
                Destroy(fx, fx.GetComponent<ParticleSystem>().main.duration);
            }
        }
    }
    

    public void Die()
    {
        once = false;
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.LoadEndScene();
    }
    

    public void SetBulletCount(int count)
    {
        bulletCount = count;
    }
}
