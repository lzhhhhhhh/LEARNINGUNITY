using System.Collections;
using UnityEngine;

public class TrackingGun : MonoBehaviour
{
    private SpriteRenderer sr;
    private Vector2 mousePos;
    public Transform playerTransform;
    public int bulletNum;
    public GameObject bulletPrefab;
    public float firingInterval;
    public int weaponType;


    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        TowardsMouse();
        if (Input.GetMouseButtonDown(0))
        {
            Fire(weaponType);
        }
    }

    private void TowardsMouse()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePos.x >= playerTransform.position.x)
        {
            sr.flipY = false;
        }
        else
        {
            sr.flipY = true;
        }
        transform.right = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
    }

    private void Fire(int weaponType=1)
    {
        switch(weaponType)
        {
            case 1:
                StartCoroutine(FireOneBulletCo());
                break;

            case 2:
                for (int i = 0; i < bulletNum; i++)
                {
                    GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
                    bullet.transform.position = transform.Find("Muzzle").position;
                    bullet.GetComponent<TrailRenderer>().Clear();
                    bullet.GetComponent<TrackingMissle>().timer = bullet.GetComponent<TrackingMissle>().lifeTime;
                    bullet.transform.right = transform.right;
                    bullet.GetComponent<TrackingMissle>().target = bullet.GetComponent<TrackingMissle>().FindNearestEnemy();
                    bullet.GetComponent<TrackingMissle>().GetRandomAngle();
                }
                break;

        }
      
    }

    IEnumerator FireOneBulletCo()
    {
        for (int i = 0; i < bulletNum; i++)
        {
            GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
            bullet.transform.position = transform.Find("Muzzle").position;
            bullet.GetComponent<TrailRenderer>().Clear();
            bullet.GetComponent<TrackingMissle>().timer = bullet.GetComponent<TrackingMissle>().lifeTime;
            bullet.transform.right = transform.right;
            bullet.GetComponent<TrackingMissle>().target = bullet.GetComponent<TrackingMissle>().FindNearestEnemy();
            bullet.GetComponent<TrackingMissle>().GetRandomAngle();
            yield return new WaitForSeconds(firingInterval);
        }
    }
}
