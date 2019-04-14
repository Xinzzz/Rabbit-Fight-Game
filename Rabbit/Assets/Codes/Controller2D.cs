using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour
{
    const float skinWidth = .015f;
    BoxCollider2D collider;
    RaycastOrigins raycastOrigins;
    public CollisionInfo collsions;
    public LayerMask collsionMask;

    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    ComboSystem attack;

    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        attack = GetComponent<ComboSystem>();
        CaculateRaySpacing();
    }


    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public void ResetCollision()
        {
            above = below = false;
            left = right = false;
        }
    }

    public void Move(Vector3 velocity)
    {
        

        UpdateRaycastOrigins();
        collsions.ResetCollision();
        if(velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }
        if(velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }
        
        transform.Translate(velocity);

    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collsionMask);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

            if(hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                collsions.above = (directionY == 1);
                collsions.below = (directionY == -1);
            }
        }
    }

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collsionMask);
            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.blue);
            
            if (hit)
            {
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                collsions.right = (directionX == 1);
                collsions.left = (directionX == -1);
            }
        }
    }

    void UpdateRaycastOrigins()
    {
        Bounds bound = collider.bounds;
        bound.Expand(skinWidth * -2);

        raycastOrigins.topLeft = new Vector2(bound.min.x, bound.max.y);
        raycastOrigins.topRight = new Vector2(bound.max.x, bound.max.y);
        raycastOrigins.bottomLeft = new Vector2(bound.min.x, bound.min.y);
        raycastOrigins.bottomRight = new Vector2(bound.max.x, bound.min.y);
    }

    void CaculateRaySpacing()
    {
        Bounds bound = collider.bounds;
        bound.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bound.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bound.size.x / (verticalRayCount - 1);
    }
}
