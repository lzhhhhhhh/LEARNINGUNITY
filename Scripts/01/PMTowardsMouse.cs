using UnityEngine;

public class PMTowardsMouse : PlayerMovement
{
    protected override void ChangeDirection()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x >= transform.position.x)
        {
            sr.flipX = false;
            //transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            sr.flipX = true;
            //transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }
}
