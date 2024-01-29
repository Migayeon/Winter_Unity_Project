using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static bool canMove = true;
    private Vector2 curPoint;
    private float dragSpeed = 0.01f;
    private Vector2 prevPoint;

    void Update()
    {
        if (canMove)
        {
            if (Input.GetMouseButton(1))
            {
                curPoint = Input.mousePosition;

                if (Input.GetMouseButtonDown(1))
                {
                    prevPoint = Input.mousePosition;
                }
                Vector3 move = (curPoint - prevPoint) * (-1) * dragSpeed;
                transform.Translate(move);
                if (transform.position.y > -4)
                    transform.position = new Vector3(transform.position.x, -4, -10);
                if (transform.position.y < -24)
                    transform.position = new Vector3(transform.position.x, -24, -10);
                if (transform.position.x < 0)
                    transform.position = new Vector3(0, transform.position.y, -10);
                if (transform.position.x > 28)
                    transform.position = new Vector3(28, transform.position.y, -10);
                prevPoint = Input.mousePosition;
            }
        }
    }

    private void LateUpdate()
    {
        
    }
}
