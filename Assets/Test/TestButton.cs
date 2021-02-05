using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using SIDGIN.Patcher.SceneManagment;
using UnityEngine.SceneManagement;

public class TestButton : MonoBehaviour
{
    public Text text;
    public AudioSource audioSource;

    public List<Music> GetAllMusic()
    {
        var json = SGResources.Load<TextAsset>("MusicJson");
        var list = JsonUtility.FromJson<MusicList>(json.text);
        var allMusic = list.Items.ToList();

        

        return allMusic;
    }

    public void ShowAllMusicInConsole()
    {
        var json = SGResources.Load<TextAsset>("MusicJson");
        var list = JsonUtility.FromJson<MusicList>(json.text);
        var allMusic = list.Items.ToList();
        foreach (var item in allMusic)
        {
            Debug.Log(item.MusicName);
        }
    }

    public Music GetRandomMusic()
    {
        var allMusic = GetAllMusic();
       return allMusic.Shuffle();
    }

    public void Text1()
    {
        //Play Random
        var music = GetRandomMusic();
        Debug.Log(music.MusicName);
        var lagu = SGResources.Load<AudioClip>(music.MusicName);

        audioSource.clip = lagu;
        audioSource.Play();
        text.text = "Mantul 1";
    }

    public void Text2()
    {
        var lagu = SGResources.Load<AudioClip>("Undangan palsu");
        audioSource.clip = lagu;
        audioSource.Play();

        text.text = "Mantul 2";
    }

    public void Text3()
    {
        var lagu = SGResources.Load<AudioClip>("Usai disini");
        audioSource.clip = lagu;
        audioSource.Play();


        text.text = "Mantul 433242";
    }


    public void LoadScene(string sceneName)
    {
        SGSceneManager.LoadScene(sceneName);
    }

    public void LoadSceneNormal(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

[System.Serializable]
public class Music
{
    public string MusicName;
    public string FilePath;

    public string Answer1;
    public string Answer2;
    public string Answer3;
    public string Answer4;

    public int ToLevel;
    public string Genre;
    public string Penyanyi;
    public int TahunRelease;

    public string[] Items
    {
        get
        {
            return new string[] { Answer1, Answer2, Answer3, Answer4 };
        }
    }
}

[System.Serializable]
public class MusicList
{
    public List<Music> Items;
}



public static class Helpers
{
    public static T  Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;

            int k = Random.Range(0, list.Count);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

        return list.FirstOrDefault();
    }
}