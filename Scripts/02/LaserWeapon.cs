using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeapon : MonoBehaviour
{
    private SpriteRenderer sr;
    private Vector2 mousePos;
    public Transform playerTransform;
    public GameObject slashEffect;

    public Transform firePoint;
    [SerializeField] private float maxDist;
    public LayerMask mask;

    [Header("Laser")]
    private LineRenderer lineRenderer;
    [SerializeField] private Gradient color;


    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
        lineRenderer.useWorldSpace = true;
    }

    private void Update()
    {
        TowardsMouse();

        if (Input.GetMouseButtonDown(0))
        {
            EventSystem.instance.CameraShakeEvent(0.1f);
        }

        if (Input.GetMouseButton(0))
        {

            lineRenderer.enabled = true;
            LaunchLaser();
        }
        else
        {
            lineRenderer.enabled = false;
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

    private void LaunchLaser()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, transform.right, maxDist, mask);
        lineRenderer.SetPosition(0, firePoint.position);
        if (hitInfo.collider != null)
        {
            lineRenderer.SetPosition(1, hitInfo.point);

            lineRenderer.colorGradient = color;
            if (hitInfo.collider.tag == "Enemy")
            {
                EventSystem.instance.CameraShakeEvent(0.05f);
                hitInfo.collider.gameObject.GetComponentInChildren<HealthBar>().hp -= 0.3f;
                Instantiate(slashEffect, hitInfo.point, Quaternion.identity);
            }
        }
    }
}
