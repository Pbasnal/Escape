using UnityEngine;

public interface IJump
{
    int MaxExtraJumps { get; }
    float JumpHeight { get; }
    //float JumpPower { get; }
    float InitialVelocity { get; }
    //float TimeToMaxHeight { get; }
    float FallGravityMultiplier { get; }
    Rigidbody2D Rigidbody { get; }

    int ExtraJumps { get; set; }
}

