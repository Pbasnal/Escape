using UnityEngine;

public interface IMove
{
    float MaxVelocity { get; }
    float TimeToMaxVelocitySec { get; }
    float TimeToZeroVelocitySec { get; }
    Vector2 PreviousVelocity { get; set; }
    Rigidbody2D Rigidbody { get; }
    Transform Transform { get; }
    MovementState CurrentState { get; set; }
}

