using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour 
{
    public GameManager manager;     // ゲーム全体を管理する GameManager
    public int durability ;      // 耐久値（例：3回当たると壊れる）
    public Color[] damageColors;    // 耐久に応じた色（任意）

    private SpriteRenderer sr;      //ブロックの見た目を制御するコンポーネント
    public GameObject breakEffect;   // パーティクル
    public AudioClip breakSound;     // 破壊音
    public AudioSource audioSource;  // 再生用
    public bool isInfectedClone = false;  // 増殖ブロックかどうかのフラグ

    private bool hasCloned = false; // 増殖済みかどうかのフラグ

    public float maxY = 4.5f; // 画面上限（必要に応じて調整）


    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        UpdateColor(); // 初期色を設定
        manager = FindObjectOfType<GameManager>();

    }

    void OnTriggerEnter2D (Collider2D col)      // ボールが衝突したときに呼ばれる
    {
        if (manager == null)
        manager = FindObjectOfType<GameManager>();

        if(col.gameObject.tag == "Ball")
        {
             Ball ball = col.gameObject.GetComponent<Ball>();

                // ★ 汚染ボールならブロックを増殖
                if (ball.currentState == Ball.BallState.Infected)
                {
                    SpawnClone();
                }

            ball.SetDirection(transform.position);  // ボールの方向を設定

                 // 咳SEを鳴らす
                GameObject.Find("SoundManager").GetComponent<SoundManager>().PlayCough(durability);
            durability--;
                // 呼吸速度を更新
                GameObject.Find("BreathSound").GetComponent<BreathManager>().AdjustBreath(durability);


            if (durability <= 0)    // 耐久地が０以下ならブロックを破壊
            {
                // 破壊エフェクトを生成
                if (breakEffect != null)
                {
                    Instantiate(breakEffect, transform.position, Quaternion.identity);
                }
                manager.score++;// スコアを1点加算（必要ならここを += 100 などに変更可能）
                manager.bricks.Remove(gameObject); // bricksリストから削除

                if(manager.bricks.Count == 0)
                    manager.WinGame();// すべてのブロックが壊れたら勝利処理


                Destroy(gameObject);// ブロックを消す
            }
            else
            {
                UpdateColor(); // 耐久値が残ってる場合耐久地に応じて色を変える
            }
        }
    }

    void SpawnClone() // ブロックを増殖させるメソッド
    {
        if (hasCloned) return;// すでに増殖済みなら何もしない
        
        // 上方向にだけ増殖
        Vector3 offset = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(0.5f, 1.5f),
            0
        );

        Vector3 spawnPos = transform.position + offset;

        // ★ 上限チェック：画面上限を超えるなら生成しない
        if (spawnPos.y > maxY - 0.3f)

        {
            hasCloned = true; // 生成しなかったけど「増殖済み扱い」にする
            return;
        }

        hasCloned = true; // 増殖済みフラグを立てる

        GameObject clone = Instantiate(gameObject, spawnPos, Quaternion.identity);

        if (manager == null)
            manager = FindObjectOfType<GameManager>();

        manager.bricks.Add(clone);
        // ★ 追加：増殖ブロックは「増殖ブロック」であることを設定
        clone.GetComponent<Brick>().isInfectedClone = true;

            // ★ 追加：増殖ブロックは耐久1
        clone.GetComponent<Brick>().durability = 1;

        // ★ 追加：増殖ブロックの色を変える（感染色）
        clone.GetComponent<SpriteRenderer>().color = new Color(1f, 0.3f, 0.3f);

    }

    void UpdateColor()// 耐久値に応じて色を変える
    {
        if (isInfectedClone) return; // ★ 感染ブロックは色変更しない

        // 耐久値に応じた色に変更
        if (damageColors.Length >= durability && durability > 0)
        {
            sr.color = damageColors[durability - 1];
        }
    }
}