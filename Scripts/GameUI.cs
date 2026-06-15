using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // シーン切り替え用

public class GameUI : MonoBehaviour 
{
    public GameManager manager;          // ゲーム全体を管理する GameManager

    public Text scoreText;               // スコアを表示するテキスト
    public Text livesText;               // ライフを表示するテキスト

    public GameObject gameOverScreen;    // ゲームオーバー画面
    public Text gameOverScoreText;       // ゲームオーバー時にスコアを表示するテキスト

    public GameObject winScreen;         // 勝利画面

    void Update()
    {
        // ゲーム中（ゲームオーバーでも勝利でもない状態）
        if (!manager.gameOver && !manager.wonGame)
        {
            scoreText.text = "<b>SCORE</b>\n" + manager.score; // スコアを表示
            livesText.text = "<b>LIVES</b>: " + manager.lives; // ライフを表示
        }
        else
        {
            // ゲーム終了時はスコア・ライフ表示を消す
            scoreText.text = "";
            livesText.text = "";
        }
    }

    // ゲームオーバー時の処理
    public void SetGameOver()
    {
        gameOverScreen.SetActive(true); // 画面を表示
        gameOverScoreText.text = "<b>YOU ACHIEVED A SCORE OF</b>\n" + manager.score;
    }

    // 勝利時の処理
    public void SetWin()
    {
        winScreen.SetActive(true); // 勝利画面を表示
    }

    // 「TRY AGAIN」ボタンを押したとき
    public void TryAgainButton()
    {
        gameOverScreen.SetActive(false);
        winScreen.SetActive(false);
        manager.StartGame(); // ゲームを再スタート
    }

    // 「MENU」ボタンを押したとき
    public void MenuButton()
    {
        SceneManager.LoadScene(0); // メニューシーンをロード
    }
}