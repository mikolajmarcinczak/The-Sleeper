using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float fireRate = 0;
    public int Damage = 10;

    [System.Serializable]
    public class WeaponAmmo
    {
        public int maxAmmo = 12;

        private int _curAmmo = 12;
        public int curAmmo
        {
            get { return _curAmmo; }
            set { _curAmmo = Mathf.Clamp(value, 0, maxAmmo); }
        }

        public void Init()
        {
            curAmmo = maxAmmo;
        }
    }

    public WeaponAmmo ammo = new WeaponAmmo();

    public LayerMask whatToHit;

    public Transform BulletTrailPrefab;
    public Transform HitPrefab;
    public Transform MuzzleFlashPrefab;
    float timeToSpawnEffect = 0;
    public float effectSpawnRate = 10;

    public float camShakeAmount = 0.05f;
    public float camShakeLength = 0.1f;
    CameraShake camShake;

    float timeToFire = 0; //place in time for the next shot
    Transform firePoint;

    void Awake()
    {
        firePoint = transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.LogError("No firepoint.");
        }
    }

    void Start()
    {
        camShake = GameMaster.gm.GetComponent<CameraShake>();
    }

    void Update()
    {
        if (ammo.curAmmo > 0)
        {
            if (fireRate == 0)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Shoot();
                    ammo.curAmmo -= 1;
                }
            }
            else
            {
                if (Input.GetButton("Fire1") && Time.time > timeToFire)
                {
                    timeToFire = Time.time + 1 / fireRate;
                    Shoot();
                    ammo.curAmmo -= 1;
                }
            }
        }
        else Debug.Log("Out of ammo");
    }

    void Shoot()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 100, whatToHit);
        
        Debug.DrawLine(firePointPosition, (mousePosition-firePointPosition)*100, Color.cyan);
        AudioManager.instance.Play("Shot");
        if(hit.collider != null)
        {
            Debug.DrawLine(firePointPosition, hit.point, Color.red);
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.DamageEnemy(Damage);
                Debug.Log("You hit " + hit.collider.name + " and did " + Damage + " damage.");
            }
        }

        if (Time.time >= timeToSpawnEffect)
        {
            Vector3 hitPos;
            Vector3 hitNormal;

            if(hit.collider == null)
            {
                hitPos = (mousePosition - firePointPosition) * 30;
                hitNormal = new Vector3(9999, 9999, 9999);
            }
            else
            {
                hitPos = hit.point;
                hitNormal = hit.normal;
            }

            Effect(hitPos, hitNormal);
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
    }

    void Effect(Vector3 hitPos, Vector3 hitNormal)
    {
        Transform trail = (Transform)Instantiate(BulletTrailPrefab, firePoint.position, firePoint.rotation);
        LineRenderer lr = trail.GetComponent<LineRenderer>();

        if(lr != null)
        {
            lr.SetPosition(0, firePoint.position);
            lr.SetPosition(1, hitPos);
        }

        Destroy(trail.gameObject, 0.05f);

        if(hitNormal != new Vector3(9999,9999,9999))
        {
            Transform hitParticle = (Transform)Instantiate(HitPrefab, hitPos, Quaternion.FromToRotation(Vector3.right, hitNormal));
            Destroy(hitParticle.gameObject, 1f);
        }

        Transform muzz = (Transform)Instantiate(MuzzleFlashPrefab, firePoint.position, firePoint.rotation);
        muzz.parent = firePoint;
        float size = Random.Range(0.6f, 0.9f);
        muzz.localScale = new Vector3(size, size, size);
        Destroy(muzz.gameObject, 0.02f);

        camShake.Shake(camShakeAmount, camShakeLength);
    }
}
