using System.Collections;
using UnityEngine;

public class TrackingMissle : MonoBehaviour
{
    public GameObject target;
    [SerializeField] private float moveSpeed;
    public float Damage;
    public float randomAngle;
    public float standardAngle;
    private float rotationAngle;
    private Vector3 direction;
    public float timer;
    public float lifeTime;
    private Transform weaponTransform;
    public GameObject slashEffect;
    public int trackType;
    private SpriteRenderer sr;

    private void Start()
    {
        timer = lifeTime;
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        weaponTransform = GameObject.FindGameObjectWithTag("Weapon").transform.Find("Muzzle").transform;

        if (target != null)
        {
            direction = target.transform.position - transform.position;
        }

        if (Vector2.Distance(transform.position, weaponTransform.position) <= 0.01f)
        {
            timer = lifeTime;
        }

        if (target != null)
        {
            Track(trackType);

            if (Vector3.Magnitude(direction) <= 1.5f)
            {
                ObjectPool.Instance.PushObject(gameObject);
                target.GetComponentInChildren<HealthBar>().hp -= Damage;
                EventSystem.instance.CameraShakeEvent(0.1f);
                Instantiate(slashEffect, transform.position, Quaternion.identity);
            }
        }
        else
        {
            Shoot();
        }

        if (timer <= 0)
        {
            ObjectPool.Instance.PushObject(gameObject);
            Instantiate(slashEffect, transform.position, Quaternion.identity);
        }
    }

    public GameObject FindNearestEnemy()
    {
        GameObject nearestEnemy = null;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float minDistance = Mathf.Infinity;
        foreach (var enemy in enemies)
        {
            if (Vector2.Distance(enemy.transform.position, mousePos) < minDistance)
            {
                nearestEnemy = enemy;
                minDistance = Vector3.Magnitude(new Vector2(enemy.transform.position.x, enemy.transform.position.y) - mousePos);
            }
        }
        return nearestEnemy;
    }

    public void GetRandomAngle()
    {
        rotationAngle = Random.Range(standardAngle - randomAngle, standardAngle + randomAngle);
    }

    private void Track(int trackType = 1)
    {
        switch (trackType)
        {
            case 1:
                EATrack();
                break;

            case 2:
                SlerpTrack();
                break;

            case 3:
                LerpTrack();
                break;
            
            case 4:
                StartCoroutine(TrackType4Co());
                break;
            case 5:
                StartCoroutine(TrackType5Co());
                
                break;


        }

    }

    private void EATrack()//基本追踪类型01：欧拉角追踪
    {
        float angle = 360 - Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
        transform.rotation *= Quaternion.Euler(0, 0, rotationAngle);
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

    private void SlerpTrack()//基本追踪类型02：球形插值追踪
    {
        transform.right = Vector3.Slerp(transform.right,
                                        direction,
                                        0.5f / Vector2.Distance(transform.position, target.transform.position));
        transform.position += transform.right * moveSpeed * Time.deltaTime;
    }

    private void LerpTrack()//基本追踪类型03：线性插值追踪
    {
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
    }

    private void Shoot()
    {
        if (Vector2.Distance(transform.position, weaponTransform.position) <= 0.1f)
        {
            transform.right = weaponTransform.right;
            transform.rotation *= Quaternion.Euler(0, 0, Random.Range(-30,30));
        }
        transform.position += transform.right * moveSpeed * Time.deltaTime;
    }

    IEnumerator TrackType4Co()//这段程序需要修改
    {
        Shoot();
        yield return new WaitForSeconds(0.3f);
        Track(1);
        if (Vector3.Magnitude(direction) <= 1.5f)
        {
            ObjectPool.Instance.PushObject(gameObject);
            target.GetComponentInChildren<HealthBar>().hp -= Damage;
            EventSystem.instance.CameraShakeEvent(0.1f);
            Instantiate(slashEffect, transform.position, Quaternion.identity);
        }
    }

    IEnumerator TrackType5Co()//这段程序需要修改
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Random.Range(0f,0.2f));
        yield return new WaitForSeconds(Random.Range(0f,5f));
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Random.Range(0.8f,1f));
        Track(1);

    }
}
