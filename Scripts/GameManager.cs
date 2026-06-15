using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // --- ゲーム設定（ここにまとめるとわかりやすい） ---
    [Header("壁の範囲設定")]
    public float wallLeft = -7f;
    public float wallRight = 7f;
    public float wallTop = 3f;
    public float wallBottom = -5f;


    // --- ゲームの基本情報 ---
    public int score;                  // プレイヤーのスコア
    public int lives;                  // 残りライフ数
    public int ballSpeedIncrement;     // ボールがブロックに当たるたびに増加する速度
    public bool gameOver;              // ゲームオーバー状態かどうか
    public bool wonGame;               // ゲームクリア状態かどうか

    // --- ゲームオブジェクト ---
    public GameObject paddle;          // パドル
    public GameObject ball;            // ボール
    public GameObject ballPrefab;
    private GameObject currentBall;
    public GameUI gameUI;              // UI管理クラス

    // --- プレハブとブロック管理 ---
    public GameObject brickPrefab;     // ブロックのプレハブ
    public List<GameObject> bricks = new List<GameObject>(); // 現在画面にあるブロックのリスト
    public int brickCountX;            // 横方向のブロック数（偶数より奇数推奨）
    public int brickCountY;            // 縦方向のブロック数
    public Color[] colors;             // ブロックの色パターン

         void Start()
    {
        StartGame();
    }

    // --- ゲーム開始時の初期化 ---
    public void StartGame()
    {
    score = 0;
    lives = 3;
    gameOver = false;
    wonGame = false;

    paddle.SetActive(true);
    if (currentBall != null)
        Destroy(currentBall);
    currentBall = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);
    currentBall.GetComponent<Ball>().ResetBall();


    paddle.GetComponent<Paddle>().ResetPaddle();

    ClearBricks();        // ← 前のステージのブロックを消す（重要）
    CreateBrickArray();   // ← 新しいブロック生成

    }

    // --- ブロックを生成して配置 ---
    public void CreateBrickArray()
    {
        int colorId = 0; // 現在の色インデックス

        for (int y = 0; y < brickCountY; y++)
        {
            for (int x = -(brickCountX / 2); x < (brickCountX / 2); x++)
            {
                Vector3 pos = new Vector3(0.8f + (x * 1.6f), 1 + (y * 0.4f), 0);
                GameObject brick = Instantiate(brickPrefab, pos, Quaternion.identity);

                // Brick.cs に GameManager を渡す
                brick.GetComponent<Brick>().manager = this;

                // 色を設定
                brick.GetComponent<SpriteRenderer>().color = colors[colorId];

                // 耐久値をランダムに設定（1〜3）
                brick.GetComponent<Brick>().durability = Random.Range(1, 4);

                bricks.Add(brick);
            }

            colorId++;
            if (colorId == colors.Length) colorId = 0;
        }


    }

    // --- 勝利処理 ---
    public void WinGame()
    {
        wonGame = true;
        paddle.SetActive(false);
        currentBall.SetActive(false);
        gameUI.SetWin();

        Debug.Log("ステージクリア！");

        // 呼吸音をフェードアウト
        GameObject.Find("BreathSound").GetComponent<BreathManager>().FadeOutBreath(2.0f);

        // ★スコア送信
        FindObjectOfType<SendScore>().Send("Yumeto", score);
        

    }

    // ---ボール・パドルをリセット ---
    public void ResetGame()
    {
        paddle.GetComponent<Paddle>().ResetPaddle();
        currentBall.SetActive(true);
        currentBall.GetComponent<Ball>().ResetBall();
    }

// --- 画面上のすべてのブロックを削除 ---
public void ClearBricks()
{
    foreach (var brick in bricks)
    {
        Destroy(brick);
    }
    bricks.Clear();
}

    // --- ライフを失ったときの処理 ---
            public void LiveLost()
            {
                lives--;

            if (lives <= 0)
        {
            gameOver = true;
            paddle.SetActive(false);
            Destroy(currentBall);
            gameUI.SetGameOver();

            ClearBricks();
        }
        else{
        // ライフが残っている場合はパドルとボールをリセット
        ResetGame();
        }
    }
}
