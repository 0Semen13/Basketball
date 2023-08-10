using System.Collections.Generic;

public class LocalisationSystem
{
    public enum Language {
        Russia,
        English
    }

    public static Language language;

    private static Dictionary<string, string> localisedRU;
    private static Dictionary<string, string> localisedEN;

    public static bool isInit;

    public static void SetLanguage(bool ToggleL) {
        if (ToggleL) {
            language = Language.Russia;
        }
        else {
            language = Language.English;
        }
    }

    public static bool IsLanguageEN() {
        if (language == Language.English) return true;
        else return false;
    }

    public static void Init() {
        CSVLoader csvLoader = new CSVLoader();
        csvLoader.LoadCSV();

        localisedRU = csvLoader.GetDictionaryValues("ru");
        localisedEN = csvLoader.GetDictionaryValues("en");

        isInit = true;
    }

    public static string GetLocalisedValue(string key) {
        if (!isInit) { Init(); }
        string value = key;

        switch (language) {
            case Language.Russia:
                localisedRU.TryGetValue(key, out value);
                break;

            case Language.English:
                localisedEN.TryGetValue(key, out value);
                break;
        }

        return value;   
    }
}