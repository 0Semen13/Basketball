using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using UnityEngine;

public class SaveAndLoad : MonoBehaviour {
    public int point_S;
    public int numberBalls_S;

    public void SaveGame() {
        point_S = GameObject.Find("Player").GetComponent<Player>().point;
        numberBalls_S = GameObject.Find("Player").GetComponent<Player>().numberBalls;

        PlayerPrefs.SetInt("Points", point_S);
        PlayerPrefs.SetInt("Ball", numberBalls_S);

        PlayerPrefs.Save();
    }

    public void LoadGame() {
        point_S = PlayerPrefs.GetInt("Points");
        numberBalls_S = PlayerPrefs.GetInt("Ball");
    }

    public void ResetData() {
        PlayerPrefs.DeleteAll();
        point_S = 0;
        numberBalls_S = 0;
    }
}