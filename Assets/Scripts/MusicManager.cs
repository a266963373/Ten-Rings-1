using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager I;

    [SerializeField] AudioSource audioSource;

    // 场景名到音乐名的映射
    private Dictionary<string, string> sceneMusicMap = new()
    {
        { "TitleScene", "TitleMusic" },
        { "SaveScene", "TitleMusic" },
        { "MainScene", "MainMusic" },
        { "LevelScene", "Level" },
        //{ "BattleScene", "Battle" },
        // 可继续添加
    };

    void Awake()
    {
        if (I == null)
        {
            I = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (sceneMusicMap.TryGetValue(scene.name, out var musicName))
        {
            PlayMusic(musicName);
        }
    }

    public void PlayMusic(AudioClip newClip)
    {
        if (audioSource.clip == newClip) return; // 相同就不切换
        audioSource.clip = newClip;
        audioSource.Play();
    }

    public void PlayMusic(string musicName)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Music/{musicName}");
        if (clip != null)
        {
            PlayMusic(clip);
        }
        else
        {
            if (musicName == "Battle")
            {
                if (BattleSession.IsLastEncounter)
                {
                    // 如果是 Battle 音乐，see if last encounter
                    musicName = "Boss";
                }

                // 新增：尝试随机播放以 musicName 为前缀的音乐
                AudioClip[] clips = Resources.LoadAll<AudioClip>("Music");
                var matched = clips.Where(c => c.name.StartsWith(musicName)).ToArray();
                if (matched.Length > 0)
                {
                    var randomClip = matched[Random.Range(0, matched.Length)];
                    PlayMusic(randomClip);
                }
                else
                {
                    Debug.LogWarning($"未找到音乐资源: {musicName}");
                }
            }
            else if (musicName == "Level")
            {
                if (GameSystem.I.Run.Level == null)
                {
                    PlayMusic("MainMusic");
                } else
                {
                    PlayMusic(GameSystem.I.Run.Level.Name);
                }
            }
            else
            {
                Debug.LogWarning($"未找到音乐资源: {musicName}");
            }
        }
    }
}
