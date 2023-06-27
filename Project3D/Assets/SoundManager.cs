using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * SoundList가 Dictionary일 때 사용
*/
//public struct AudioInfo { }
public class AudioInfo
{
    public AudioClip clip;
    public string key;
}

//[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private static SoundManager Instance = null;

    public static SoundManager GetInstance
    {
        get
        {
            if (!Instance)
            {
                // 싱글톤을 GameObject로 만듦 - 하이어라키에 올려 초기 함수(MonoBehaviour의 Start 함수 등) 사용하려고
                GameObject obj = new GameObject("SoundManager");
                Instance = obj.AddComponent(typeof(SoundManager)) as SoundManager;
            }
            return Instance;
        }
    }


    //private Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();

    // AudioClip은 Dictionary 사용하는 것이 좋다
    private Dictionary<string, List<AudioInfo>> SoundList = new Dictionary<string, List<AudioInfo>>();
    //public List<AudioClip> SoundList = new List<AudioClip>();
    public string[] filePath = { "" };

    private void Start()
    {
        foreach(string path in filePath)
        {
            object[] Objects = Resources.LoadAll("" + path);

            for (int i = 0; i < Objects.Length; ++i)
            {
                AudioInfo obj = new AudioInfo();
                obj.clip = Objects[i] as AudioClip;
                obj.key = Objects[i].ToString();

                List<AudioInfo> temp = new List<AudioInfo>();
                temp.Add(obj);
                SoundList.Add(path, temp);

                // key가 이미 있으면 바로 넣고, 없을 때는 생성해서 넣는 걸로 해줘야 함
            }
        }
    }

    /*
    void PlaySound()  // 필요 X, 임시
    {
        AudioSource source = new AudioSource();
        source.clip = SoundList[0];

        source.Play();
        source.Stop();
    }
    */
}
