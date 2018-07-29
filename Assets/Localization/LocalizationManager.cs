using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    protected LocalizationManager() { }

    private static LocalizationManager _instance;

    private static Dictionary<string, LocalizationLanguage> localizationLists;

    public LocalizationManager GetInstance
    {
        get
        {
            SetInstance();

            return _instance;
        }

        set
        {
            _instance = value;
        }
    }

    private void SetInstance()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        else if (_instance != gameObject)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(_instance);
    }

    private void SetLocalization()
    {
        Debug.LogError("SET LOCALIZATION");

        localizationLists = new Dictionary<string, LocalizationLanguage>();


    }

    private void Awake()
    {
        SetInstance();

        SetLocalization();
    }
}

public class LocalizationLanguage : ScriptableObject
{
    private Dictionary<string, string> _localTranslation;

    public Dictionary<string, string> LocalTranslation
    {
        get
        {
            return _localTranslation;
        }

        private set
        {
            _localTranslation = value;
        }
    }
}