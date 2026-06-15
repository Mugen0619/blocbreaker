using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject gameCanvas;//メインUIマネージャー
    public GameObject rankingCanvas;//ランキングUIマネージャー

    public void ShowRanking()//ランキング画面表示
    {
        gameCanvas.SetActive(false);
        rankingCanvas.SetActive(true);

    }
    public void OnClickShowRanking()
    {
        FindObjectOfType<UIManager>().ShowRanking();//ボタン押したらランキング画面表示
    }
}
