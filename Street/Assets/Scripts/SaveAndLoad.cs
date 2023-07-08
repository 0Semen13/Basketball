using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using UnityEngine;

public class SaveAndLoad : MonoBehaviour {
    public int point_S;
    public int numberBalls_S;

    public double percentage2Point_S;
    private string p2Point_STR;
    public double percentage3Point_S;
    private string p3Point_STR;
    public double percentageExtraLong_S;
    private string pExtraLong_STR;

    public void SaveGame() {
        point_S = GameObject.Find("Player").GetComponent<Player>().point;
        numberBalls_S = GameObject.Find("Player").GetComponent<Player>().numberBalls;

        percentage2Point_S = GameObject.Find("Player").GetComponent<Player>().currentPercentage2Point;
        p2Point_STR = percentage2Point_S.ToString();
        percentage3Point_S = GameObject.Find("Player").GetComponent<Player>().currentPercentage3Points;
        p3Point_STR = percentage3Point_S.ToString();
        percentageExtraLong_S = GameObject.Find("Player").GetComponent<Player>().currentPercentageExtraLong;
        pExtraLong_STR = percentageExtraLong_S.ToString();

        PlayerPrefs.SetInt("Points", point_S);
        PlayerPrefs.SetInt("Ball", numberBalls_S);

        PlayerPrefs.SetString("Percentage2Point", p2Point_STR);
        PlayerPrefs.SetString("Percentage3Point", p3Point_STR);
        PlayerPrefs.SetString("PercentageExtraLong", pExtraLong_STR);

        PlayerPrefs.Save();
    }

    public void LoadGame() {
        point_S = PlayerPrefs.GetInt("Points");
        numberBalls_S = PlayerPrefs.GetInt("Ball");

        p2Point_STR = PlayerPrefs.GetString("Percentage2Point");
        percentage2Point_S = Convert.ToDouble(p2Point_STR);
        p3Point_STR = PlayerPrefs.GetString("Percentage3Point");
        percentage3Point_S = Convert.ToDouble(p3Point_STR);
        pExtraLong_STR = PlayerPrefs.GetString("PercentageExtraLong");
        percentageExtraLong_S = Convert.ToDouble(pExtraLong_STR);
    }

    public void ResetData() {
        PlayerPrefs.DeleteAll();
        point_S = 0;
        numberBalls_S = 0;
    }
}