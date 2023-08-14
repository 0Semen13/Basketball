using System;
using UnityEngine;
using UnityEngine.UI;

public class TextLocaliserUI : MonoBehaviour {
    private Text textField;
    public string key;

    private UIManager UIManagerScript;

    private void Start() {
        UIManagerScript = GameObject.Find("UIManager").GetComponent<UIManager>();
        UIManagerScript.OnLanguageChange += LanguageUpdatesEvents;

        LanguageUpdates();
    }

    private void LanguageUpdatesEvents(object sender, EventArgs e) {
        LanguageUpdates();
    }

    private void LanguageUpdates() {
        textField = GetComponent<Text>();
        string value = LocalisationSystem.GetLocalisedValue(key);
        textField.text = value;
        if (LocalisationSystem.IsLanguageEN() == true) textField.text = textField.text.Remove(textField.text.Length - 2);
    }
}