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

    public int addingPoints_S;
    public float chanceSpeed_S;

    private Player PlayerScript;

    public void SaveGame() {
        PlayerScript = GameObject.Find("Player").GetComponent<Player>();

        point_S = PlayerScript.point;
        PlayerPrefs.SetInt("Points", point_S);
        numberBalls_S = PlayerScript.numberBalls;
        PlayerPrefs.SetInt("Ball", numberBalls_S);

        percentage2Point_S = PlayerScript.currentPercentage2Point;
        p2Point_STR = percentage2Point_S.ToString();
        PlayerPrefs.SetString("Percentage2Point", p2Point_STR);

        percentage3Point_S = PlayerScript.currentPercentage3Points;
        p3Point_STR = percentage3Point_S.ToString();
        PlayerPrefs.SetString("Percentage3Point", p3Point_STR);

        percentageExtraLong_S = PlayerScript.currentPercentageExtraLong;
        pExtraLong_STR = percentageExtraLong_S.ToString();
        PlayerPrefs.SetString("PercentageExtraLong", pExtraLong_STR);

        addingPoints_S = PlayerScript.addingPoints;
        PlayerPrefs.SetInt("addingPoints", addingPoints_S);
        chanceSpeed_S = PlayerScript.chanceSpeedForSave;
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