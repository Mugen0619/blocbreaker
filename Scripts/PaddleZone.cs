using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleZone : MonoBehaviour
{
    public bool isCleanZone; // true = 清潔, false = 汚染

private void OnTriggerEnter2D(Collider2D col)
{
    // ボール
    Ball ball = col.GetComponent<Ball>();
    if (ball != null)
    {
        ball.currentState = isCleanZone ? Ball.BallState.Clean : Ball.BallState.Infected;
        ball.UpdateBallColor();
    }

    // パドル
    Paddle paddle = col.GetComponentInParent<Paddle>();
    if (paddle != null)
    {
        if (isCleanZone)
            paddle.currentState = Paddle.PaddleState.Clean;
        else
            paddle.currentState = Paddle.PaddleState.Infected;

        paddle.UpdatePaddleColor();
    }
}
}
