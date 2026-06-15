using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class RankingUI : MonoBehaviour
{
    public RankingFetcher fetcher;
    public Transform rankingParent; // Vertical Layout Group
    public GameObject rankingItemPrefab; // TextMeshPro の1行テンプレ

    void Start()
    {
        StartCoroutine(fetcher.FetchRanking(OnRankingReceived));
    }

    void OnRankingReceived(List<ScoreData> list)
    {
        if (list == null)
        {
            Debug.LogError("ランキングデータが null");
            return;
        }

        foreach (Transform child in rankingParent)
        {
            Destroy(child.gameObject);
        }

        int rank = 1;
        foreach (var item in list)
        {
            GameObject obj = Instantiate(rankingItemPrefab, rankingParent);
            TextMeshProUGUI text = obj.GetComponent<TextMeshProUGUI>();
            text.text = $"{rank}. {item.player} - {item.score}";
            rank++;
        }
    }
}