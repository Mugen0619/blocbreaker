using UnityEngine;

public class Paddle : MonoBehaviour
{
    [Header("パドル設定")]
    public float speed = 8f;            // パドルの移動速度（1秒あたりの移動量）
    public float minX = -4.5f;          // 左端の移動制限
    public float maxX = 4.5f;           // 右端の移動制限
    public bool canMove = true;         // パドルが動けるかどうか

    public enum PaddleState
{
    Clean,
    Infected
}

    public PaddleState currentState = PaddleState.Clean;
    public SpriteRenderer sr; // パドルの色変更用

    public void UpdatePaddleColor()
{
    if (currentState == PaddleState.Clean)
        sr.color = Color.white;   // 清潔
    else
        sr.color = Color.red;     // 汚染
}

    [Header("参照")]
    public Rigidbody2D rig;             // Rigidbody2D への参照
		public GameManager manager;				 // GameManager への参照

    void Update()
    {
        if (!canMove)
        {
            rig.velocity = Vector2.zero;
            return;
        }

        // --- 入力処理 ---
        float move = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            move = -1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            move = 1f;
        }

        // Rigidbody2D の velocity は「1秒あたりの速度」なので deltaTime は不要
        rig.velocity = new Vector2(move * speed, 0);

        // --- 画面端の制限 ---
				float clampedX = Mathf.Clamp(transform.position.x, manager.wallLeft, manager.wallRight);
				transform.position = new Vector3(clampedX, transform.position.y, 0);
		}

    // パドルにボールが当たった時に呼ばれる
    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Ball")) return;

        Ball ball = col.GetComponent<Ball>();
        if (ball == null) return;
        // パドルの状態をボールに合わせる
    ball.currentState = (currentState == PaddleState.Infected)
        ? Ball.BallState.Infected
        : Ball.BallState.Clean;

    ball.UpdateBallColor();


        // 反射処理
        ball.SetDirection(transform.position);

    }

    // パドルを中央にリセット
    public void ResetPaddle()
    {
        transform.position = new Vector3(0, transform.position.y, 0);
    }
}