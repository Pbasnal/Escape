using UnityEngine;

public interface IMove
{
    float MaxVelocity { get; }
    float TimeToMaxVelocitySec { get; }
    float TimeToZeroVelocitySec { get; }
    Vector3 PreviousVelocity { get; set; }
    Rigidbody2D Rigidbody2d { get; }
    Transform Transform { get; }
    MovementState CurrentState { get; set; }
}

