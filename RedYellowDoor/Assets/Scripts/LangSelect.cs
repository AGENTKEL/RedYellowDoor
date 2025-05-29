using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LangSelect : MonoBehaviour
{
    [SerializeField] private List<Button> languageButtons;
    private bool active = false;
    private void Start()
    {
        int ID = PlayerPrefs.GetInt("LocaleKey");
        ChangeLocale(ID);
    }

    public void ChangeLocale(int localeID)
    {
        if (active == true)
            return;
        StartCoroutine(SetLocale(localeID));
    }
    

    private IEnumerator SetLocale(int _localeID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
        PlayerPrefs.SetInt("LocaleKey", _localeID);
        UpdateButtonIndicators(_localeID);
        active = false;
    }
    
    public void ChangeLocaleLoop()
    {
        if (active == true)
            return;
        StartCoroutine(SetLocaleLoop());
    }
    
    private IEnumerator SetLocaleLoop()
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;

        var locales = LocalizationSettings.AvailableLocales.Locales;
        int currentID = PlayerPrefs.GetInt("LocaleKey", 0);
        int nextID = (currentID + 1) % locales.Count;

        LocalizationSettings.SelectedLocale = locales[nextID];
        PlayerPrefs.SetInt("LocaleKey", nextID);
        
        UpdateButtonIndicators(nextID);
        active = false;
    }
    
    private void UpdateButtonIndicators(int selectedID)
    {
        for (int i = 0; i < languageButtons.Count; i++)
        {
            // Assumes the image you want to toggle is the first child
            Transform childImage = languageButtons[i].transform.GetChild(0);
            childImage.gameObject.SetActive(i == selectedID);
        }
    }
}
