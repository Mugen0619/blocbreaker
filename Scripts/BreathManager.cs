using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathManager : MonoBehaviour
{
    private AudioSource audioSource;
    public int maxDurability = 3; // ブロックの最大耐久値を設定


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.volume = 0.4f; // 初期音量
        audioSource.pitch = 1.5f; // 最初は速めの呼吸
        audioSource.Play();
    }
    
        // 耐久値に応じて呼吸速度を調整
    public void AdjustBreath(int durability)
    {
        // 耐久値が減るほど pitch を下げる（呼吸が遅くなる）
        float ratio = (float)durability / maxDurability; // 0〜1の割合
        audioSource.pitch = Mathf.Lerp(0.7f, 1.5f, ratio);
        // ratio=1 → pitch=1.5（速い呼吸）
        // ratio=0 → pitch=0.7（遅い呼吸）
    }

    // フェードアウト開始
    public void FadeOutBreath(float duration = 2.0f)
    {
        StartCoroutine(FadeOutCoroutine(duration));
    }

    private IEnumerator FadeOutCoroutine(float duration)
    {
        float startVolume = audioSource.volume;

        // duration 秒かけて音量を 0 にする
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }

        audioSource.volume = 0;
        audioSource.Stop(); // 完全に停止
    }
}

