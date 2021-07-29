using UnityEngine;
using System.Collections;

public class InfraredSight : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform firePoint;
    [SerializeField] private float maxDist;
    public LayerMask mask;
    [SerializeField] private Gradient redColor, greenColor;
    private bool isDetecting;

    private void Start()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
        lineRenderer.useWorldSpace = true;
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isDetecting)
        {
            lineRenderer.enabled = true;
            isDetecting = true;
        }
        if (isDetecting)
        {
            Detect();
            StartCoroutine(StopDetectingCo());
        }
    }

    IEnumerator StopDetectingCo()
    {

        yield return null;
        if (Input.GetMouseButtonDown(1))
        {
            isDetecting = false;
            lineRenderer.enabled = false;
        }
    }

    private void Detect()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, transform.right, maxDist, mask);
        lineRenderer.SetPosition(0, firePoint.position);
        if (hitInfo.collider != null)
        {
            lineRenderer.SetPosition(1, hitInfo.point);
            if (hitInfo.collider.tag == "Enemy")
            {
                lineRenderer.colorGradient = redColor;
            }
            else
            {
                lineRenderer.colorGradient = greenColor;
            }
        }
        else
        {
            lineRenderer.SetPosition(1, transform.position + transform.right * maxDist);
            lineRenderer.colorGradient = greenColor;
        }
    }
}
