using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField]
    private float speed = 5f;

    void Update()
    {
        if (!IsOwner) return;

        Plane plane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float point))
        {
            Vector3 targetPosition = ray.GetPoint(point);
            Vector3 direction = targetPosition - transform.position;
            direction.y = 0; // Ensure the movement is strictly horizontal on the plane

            if (direction.magnitude > 1)
            {
                direction = direction.normalized;
            }

            transform.position += direction * speed * Time.deltaTime;
        }
    }
}
