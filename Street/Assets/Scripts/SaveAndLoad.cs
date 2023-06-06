using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using UnityEngine;

public class SaveAndLoad : MonoBehaviour {
    public int point_S;
    public int numberBalls_S;

    public void SaveGame() {
        GameObject GO = GameObject.Find("Player");
        Player player_point = GO.GetComponent<Player>();
        point_S = player_point.point;

        Player player_balls = GO.GetComponent<Player>();
        numberBalls_S = player_balls.numberBalls;

        PlayerPrefs.SetInt("Points", point_S);
        PlayerPrefs.SetInt("Ball", numberBalls_S);

        PlayerPrefs.Save();

        Debug.Log("Данные игры сохранены! Очки: " + point_S + ", Мячи: " + numberBalls_S);
    }

    public void LoadGame() {
        point_S = PlayerPrefs.GetInt("Points");
        numberBalls_S = PlayerPrefs.GetInt("Ball");

        Debug.Log("Данные игры загружены! Очки: " + point_S + ", Мячи: " + numberBalls_S);
    }

    public void ResetData() {
        PlayerPrefs.DeleteAll();
        point_S = 0;
        numberBalls_S = 0;

        Debug.Log("Весь прогресс удален!");
    }
}