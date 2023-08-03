using UnityEngine;

public class SaveAndLoad : MonoBehaviour {
    public int point_S;
    public int numberBalls_S;

    public double percentage2Point_S;
    public double percentage3Point_S;
    public double percentageExtraLong_S;
    private string p2Point_STR;
    private string p3Point_STR;
    private string pExtraLong_STR;

    public int addingPoints_S;
    public float chanceSpeed_S;

    private Player playerScript;
    private Bar barScript;

    public void SaveCharacteristicsAndPoints() { //Сохранение характеристик игрока, забитых очков и мячей
        playerScript = GameObject.Find("Player").GetComponent<Player>();

        percentage2Point_S = playerScript.currentPercentage2Point;
        p2Point_STR = percentage2Point_S.ToString();
        PlayerPrefs.SetString("Percentage2Point", p2Point_STR);

        percentage3Point_S = playerScript.currentPercentage3Points;
        p3Point_STR = percentage3Point_S.ToString();
        PlayerPrefs.SetString("Percentage3Point", p3Point_STR);

        percentageExtraLong_S = playerScript.currentPercentageExtraLong;
        pExtraLong_STR = percentageExtraLong_S.ToString();
        PlayerPrefs.SetString("PercentageExtraLong", pExtraLong_STR);

        point_S = playerScript.point;
        PlayerPrefs.SetInt("Points", point_S);
        numberBalls_S = playerScript.numberBalls;
        PlayerPrefs.SetInt("Ball", numberBalls_S);

        PlayerPrefs.Save();
    }

    public void SaveBarSpeed() { //Сохранение скорости шкалы вероятности
        barScript = GameObject.Find("Bar").GetComponent<Bar>();

        chanceSpeed_S = barScript.currentChanceSpeed;
        PlayerPrefs.SetFloat("chanceSpeed", chanceSpeed_S);

        PlayerPrefs.Save();
    }

    public void SaveAddingPoints() { //Сохранение очков добавления
        playerScript = GameObject.Find("Player").GetComponent<Player>();

        addingPoints_S = playerScript.addingPoints;
        PlayerPrefs.SetInt("addingPoints", addingPoints_S);

        PlayerPrefs.Save();
    }

    public void LoadGame() { //Загрузка всех данных
        point_S = PlayerPrefs.GetInt("Points");
        numberBalls_S = PlayerPrefs.GetInt("Ball");

        p2Point_STR = PlayerPrefs.GetString("Percentage2Point");
        percentage2Point_S = double.Parse(p2Point_STR, System.Globalization.CultureInfo.InvariantCulture);
        p3Point_STR = PlayerPrefs.GetString("Percentage3Point");
        percentage3Point_S = double.Parse(p3Point_STR, System.Globalization.CultureInfo.InvariantCulture);
        pExtraLong_STR = PlayerPrefs.GetString("PercentageExtraLong");
        percentageExtraLong_S = double.Parse(pExtraLong_STR, System.Globalization.CultureInfo.InvariantCulture);

        addingPoints_S = PlayerPrefs.GetInt("addingPoints");
        chanceSpeed_S = PlayerPrefs.GetFloat("chanceSpeed");
    }

    public void ResetData() { //Сброс всех данных
        PlayerPrefs.DeleteAll();
        point_S = 0;
        numberBalls_S = 0;
    }
}