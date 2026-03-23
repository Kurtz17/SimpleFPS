using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 60;
    public float bulletPrefabLifetime = 3f;
    public float laserDuration = 0.05f; 
    public float laserRange = 100f;
    public Color laserColor = Color.red; 

    private LineRenderer laserLine;

    public GameObject muzzleEffect;

    void Start()
    {
        laserLine = gameObject.AddComponent<LineRenderer>();
        laserLine.startWidth = 0.05f;
        laserLine.endWidth = 0.05f;
        laserLine.material = new Material(Shader.Find("Sprites/Default"));
        laserLine.material.color = laserColor;
        laserLine.startColor = laserColor;
        laserLine.endColor = laserColor;
        laserLine.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            FireWeapon();
        }
    }

    private void FireWeapon()
{
    if (muzzleEffect != null)
    {
        ParticleSystem ps = muzzleEffect.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ps.Play();
        }
    }
    else
    {
        Debug.LogWarning("muzzleEffect belum di-assign di Inspector!");
    }

    SoundManager.Instance.shootingSoundDeagle.Play();
    
    GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

    Rigidbody rb = bullet.GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.AddForce(bulletSpawn.forward.normalized * bulletVelocity, ForceMode.Impulse);
    }
    else
    {
        Debug.LogWarning("Bullet prefab tidak punya Rigidbody!");
    }

    StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifetime));

    Ray ray = new Ray(bulletSpawn.position, bulletSpawn.forward);
    RaycastHit hit;

    Vector3 endPoint;
    if (Physics.Raycast(ray, out hit, laserRange))
    {
        endPoint = hit.point;
    }
    else
    {
        endPoint = bulletSpawn.position + bulletSpawn.forward * laserRange;
    }

        laserLine.SetPosition(0, bulletSpawn.position);
        laserLine.SetPosition(1, endPoint);

        StartCoroutine(ShowLaser());
    }

    private IEnumerator ShowLaser()
    {
        laserLine.enabled = true;          
        yield return new WaitForSeconds(laserDuration);
        laserLine.enabled = false;         
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(bullet);
    }
}