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

    public int firstStart_S;

    public int addingPoints_S;
    public float chanceSpeed_S;

    public void SaveGame() {
        point_S = GameObject.Find("Player").GetComponent<Player>().point;
        numberBalls_S = GameObject.Find("Player").GetComponent<Player>().numberBalls;

        percentage2Point_S = GameObject.Find("Player").GetComponent<Player>().currentPercentage2Point;
        p2Point_STR = percentage2Point_S.ToString();
        percentage3Point_S = GameObject.Find("Player").GetComponent<Player>().currentPercentage3Points;
        p3Point_STR = percentage3Point_S.ToString();
        percentageExtraLong_S = GameObject.Find("Player").GetComponent<Player>().currentPercentageExtraLong;
        pExtraLong_STR = percentageExtraLong_S.ToString();

        firstStart_S = GameObject.Find("Player").GetComponent<Player>().firstStart;

        addingPoints_S = GameObject.Find("Player").GetComponent<Player>().addingPoints;
        chanceSpeed_S = GameObject.Find("Player").GetComponent<Player>().chanceSpeedForSave;

        PlayerPrefs.SetInt("Points", point_S);
        PlayerPrefs.SetInt("Ball", numberBalls_S);

        PlayerPrefs.SetString("Percentage2Point", p2Point_STR);
        PlayerPrefs.SetString("Percentage3Point", p3Point_STR);
        PlayerPrefs.SetString("PercentageExtraLong", pExtraLong_STR);

        PlayerPrefs.SetInt("firstStart", firstStart_S);

        PlayerPrefs.SetInt("addingPoints", addingPoints_S);
        PlayerPrefs.SetFloat("chanceSpeed", chanceSpeed_S);

        PlayerPrefs.Save();
    }

    public void LoadGame() {
        point_S = PlayerPrefs.GetInt("Points");
        numberBalls_S = PlayerPrefs.GetInt("Ball");

        p2Point_STR = PlayerPrefs.GetString("Percentage2Point");
        percentage2Point_S = double.Parse(p2Point_STR, System.Globalization.CultureInfo.InvariantCulture);
        //percentage2Point_S = Convert.ToDouble(p2Point_STR);
        p3Point_STR = PlayerPrefs.GetString("Percentage3Point");
        //percentage3Point_S = Convert.ToDouble(p3Point_STR);
        percentage3Point_S = double.Parse(p3Point_STR, System.Globalization.CultureInfo.InvariantCulture);
        pExtraLong_STR = PlayerPrefs.GetString("PercentageExtraLong");
        //percentageExtraLong_S = Convert.ToDouble(pExtraLong_STR);
        percentageExtraLong_S = double.Parse(pExtraLong_STR, System.Globalization.CultureInfo.InvariantCulture);

        addingPoints_S = PlayerPrefs.GetInt("addingPoints");
        chanceSpeed_S = PlayerPrefs.GetFloat("chanceSpeed");
    }

    public void ResetData() {
        PlayerPrefs.DeleteAll();
        point_S = 0;
        numberBalls_S = 0;
    }
}