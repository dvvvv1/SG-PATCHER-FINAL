using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TestButton : MonoBehaviour
{
    public Text text;
    public AudioSource audioSource;

    public void GetAllMusics()
    {
        var json = SGResources.Load<TextAsset>("MusicJson");
        var list = JsonUtility.FromJson<MusicList>(json.text);
        var allMusic = list.Items.ToList();

        foreach (var item in allMusic)
        {
            Debug.Log(item.MusicName);
        }
    }

    public void Text1()
    {
        var lagu = SGResources.Load<AudioClip>("Untuk apa");
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

