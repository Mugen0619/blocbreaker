using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class SendScore : MonoBehaviour
{
    // ★ あなたの Render の URL に置き換える
    private string apiUrl = "https://scoreserver.onrender.com/score";

    public void Send(string player, int score)
    {
        StartCoroutine(SendScoreRoutine(player, score));
    }

    private IEnumerator SendScoreRoutine(string player, int score)
    {
        // JSON データを作成
        string json = JsonUtility.ToJson(new ScoreData(player, score));

        // POST リクエスト作成
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // 送信
        yield return request.SendWebRequest();

        // 結果確認
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("送信成功: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("送信失敗: " + request.error);
        }
    }

    // JSON 化用のクラス
    [System.Serializable]
    public class ScoreData
    {
        public string player;
        public int score;

        public ScoreData(string player, int score)
        {
            this.player = player;
            this.score = score;
        }
    }
}