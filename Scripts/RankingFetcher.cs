using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class RankingFetcher : MonoBehaviour
{
    public string url = "https://scoreserver.onrender.com/ranking";

    public IEnumerator FetchRanking(System.Action<List<ScoreData>> callback)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;

            // JSON を配列としてパース
            ScoreDataList list = JsonUtility.FromJson<ScoreDataList>("{\"items\":" + json + "}");
            callback(list.items);
        }
        else
        {
            Debug.LogError("ランキング取得失敗: " + request.error);
            callback(null);
        }
    }
}

[System.Serializable]
public class ScoreData
{
    public string player;
    public int score;
    public string time;
}

[System.Serializable]
public class ScoreDataList
{
    public List<ScoreData> items;
}