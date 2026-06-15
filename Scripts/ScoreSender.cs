using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class ScoreSender : MonoBehaviour
{
    public IEnumerator SendScore(string playerName, int score)
    {
        string url = "http://localhost:3000/score";

        // JSONデータを作成
        ScoreData data = new ScoreData(playerName, score);
        string json = JsonUtility.ToJson(data);
        byte[] body = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // 送信
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("スコア送信成功: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("スコア送信失敗: " + request.error);
        }
    }

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