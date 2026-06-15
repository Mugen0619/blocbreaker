using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    [Header("ボールの設定")]
    public float speed;                 // 現在のボール速度
    public float maxSpeed;              // ボールの最大速度
    public Vector2 direction;           // ボールの移動方向（正規化ベクトル）
    public Rigidbody2D rig;             // Rigidbody2D（物理挙動用）

    public enum BallState
{
    Clean,      // 清潔
    Infected    // 汚染
}

public BallState currentState = BallState.Clean;  // 初期状態は清潔
public SpriteRenderer sr;                         // 色変更用

public void UpdateBallColor()
{
    if (currentState == BallState.Clean)
        sr.color = Color.cyan;   // 清潔
    else
        sr.color = Color.red;    // 汚染
}

    [Header("参照")]
    public GameManager manager;         // GameManager への参照

    [Header("移動方向フラグ")]
    public bool goingLeft;              // 左方向へ進んでいるか
    public bool goingDown;              // 下方向へ進んでいるか

    void Start()
    {
        // ← GameManager への参照を取得
        manager = FindObjectOfType<GameManager>();
        // ボールを中央に配置
        transform.position = Vector3.zero;

        // 初期方向は下
        direction = Vector2.down;

        // 1秒後に動き出す
        StartCoroutine(ResetBallWaiter());

    }

    void Update()
    {
        // Rigidbody の速度を direction × speed で設定
        // Time.deltaTime を掛ける必要は本来ない（速度は1秒あたりの値だから）
        rig.velocity = direction * speed ;

        // --- 画面端での跳ね返り処理 ---
        // 右端
        if (transform.position.x > manager.wallRight && direction.x > 0)
        {
            direction = new Vector2(-direction.x, direction.y);
        }

        // 左端
        if (transform.position.x < manager.wallLeft && direction.x < 0)
        {
            direction = new Vector2(-direction.x, direction.y);
        }

        // 上端
        if (transform.position.y > manager.wallTop && direction.y > 0)
        {
            direction = new Vector2(direction.x, -direction.y);
        }

        // 下に落ちた（ミス）
        if (transform.position.y < manager.wallBottom)
        {
            manager.LiveLost();   // ← GameManager に通知するだけ

        }
    }

    // パドルやブロックに当たった時に呼ばれる
    // target = 当たったオブジェクトの位置
    public void SetDirection(Vector3 target)
    {
        // 新しい方向ベクトルを計算
        Vector2 dir = (Vector2)(transform.position - target);
        dir.Normalize(); // 長さを1にする

        direction = dir;

        // ボール速度を増加
        speed += manager.ballSpeedIncrement;

        // 最大速度を超えないように制限
        if (speed > maxSpeed)
            speed = maxSpeed;

        // 移動方向フラグを更新
        goingLeft = dir.x < 0;
        goingDown = dir.y < 0;
    }

    // ボールがミスした時に呼ばれる
    public void ResetBall()

    {
        transform.position = Vector3.zero;
        direction = Vector2.down;

        StartCoroutine(ResetBallWaiter());
    }

    // 1秒待ってからボールを動かす
    IEnumerator ResetBallWaiter()
    {
        speed = 0;                      // 一旦停止
        yield return new WaitForSeconds(1.0f);
        speed = 8f;                   // 再スタート時の初期速度
    }
}