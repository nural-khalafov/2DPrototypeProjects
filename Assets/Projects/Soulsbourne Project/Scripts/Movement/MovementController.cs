using UnityEngine;

public abstract class MovementController : MonoBehaviour
{
    public abstract Vector2 GetVelocity();
    public abstract bool IsGrounded();
}
