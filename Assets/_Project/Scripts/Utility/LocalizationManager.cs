using System;
using alpoLib.Util;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace MergeBoard.Utility
{
    public sealed class LocalizationManager : SingletonMonoBehaviour<LocalizationManager>
    {
        private class CurrentLanguageHolder : PersistentGameState
        {
            public SystemLanguage Language;
        }
        
        private StringTable _selectedStringTable;
        private Locale _selectedLocale;
        private string _tableCollectionName;
        private CurrentLanguageHolder _currentLanguageHolder;
        
        public SystemLanguage CurrentLanguage => _currentLanguageHolder?.Language ?? CheckLanguage();

        public void Initialize(string tableCollectionName)
        {
            CheckLanguage();

            _tableCollectionName = tableCollectionName;
            ChangeLanguage(_currentLanguageHolder.Language);
        }
        
        private SystemLanguage CheckLanguage()
        {
            _currentLanguageHolder = GameStateManager.Instance.GetState<CurrentLanguageHolder>(false);
            if (_currentLanguageHolder != null)
                return _currentLanguageHolder.Language;
            
            // 첫 실행!
            _currentLanguageHolder = GameStateManager.Instance.GetState<CurrentLanguageHolder>();
            _currentLanguageHolder.Language = GetFallbackLanguage(Application.systemLanguage).language;
            GameStateManager.Instance.Save();
            return _currentLanguageHolder.Language;
        }
        
        private (Locale locale, SystemLanguage language) GetFallbackLanguage(SystemLanguage language)
        {
            var newLocale = LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(language));
            if (newLocale)
                return (newLocale, language);
            
            Debug.LogWarning($"{language} 언어가 세팅되어 있지 않아서 English 로 fallback 합니다.");
            newLocale = LocalizationSettings.AvailableLocales.GetLocale(
                new LocaleIdentifier(SystemLanguage.English));
            return (newLocale, SystemLanguage.English);
        }
        
        public void ChangeLanguage(SystemLanguage language)
        {
            var localeList = LocalizationSettings.AvailableLocales.Locales;
            var (newLocale, newLanguage)= GetFallbackLanguage(language);
            if (_currentLanguageHolder.Language != newLanguage)
            {
                _currentLanguageHolder.Language = newLanguage;
                GameStateManager.Instance.Save();
            }

            if (_selectedLocale && _selectedLocale == newLocale)
                return;
            
            if (_selectedLocale)
                LocalizationSettings.StringDatabase.ReleaseTable(_tableCollectionName, _selectedLocale);
            _selectedLocale = newLocale;
            LocalizationSettings.SelectedLocale = _selectedLocale;
            var h = LocalizationSettings.StringDatabase.PreloadTables(_tableCollectionName, _selectedLocale);
            if (h.WaitForCompletion() is LocalizedStringDatabase lsd)
                _selectedStringTable = lsd.GetTable(_tableCollectionName, _selectedLocale);
        }
        
        public string GetString(string key, params object[] args)
        {
            var entry = _selectedStringTable.GetEntry(key);
            if (entry != null)
                return entry.GetLocalizedString(args);
            
            Debug.LogWarning($"{key} 는 string table 에 없습니다.");
            return $"#{key}";
        }

        public void ChangeLocalizeStringEvent(LocalizeStringEvent lse, string key)
        {
            if (!lse)
                return;
            ChangeLocalizeStringReference(lse.StringReference, key);
        }
        
        public void ChangeLocalizeStringReference(LocalizedString ls, string key)
        {
            ls?.SetReference(_tableCollectionName, key);
        }

        public static string MakeTimeSpanString(TimeSpan span)
        {
            var dayString = Instance.GetString("UI_Label_Day");
            var hourString = Instance.GetString("UI_Label_Hour");
            var minuteString = Instance.GetString("UI_Label_Minute");
            var secondString = Instance.GetString("UI_Label_Second");
            string text;
            if (span.Days > 0)
            {
                text = $"{span.Days}{dayString}";
                if (span.Hours > 0)
                    text = $"{text} {span.Hours}{hourString}";
            }
            else if (span.Hours > 0)
            {
                text = $"{span.Hours}{hourString}";
                if (span.Minutes > 0)
                    text = $"{text} {span.Minutes}{minuteString}";
            }
            else if (span.Minutes > 0)
            {
                text = $"{span.Minutes}{minuteString}";
                if (span.Seconds > 0)
                    text = $"{text} {span.Seconds}{secondString}";
            }
            else
            {
                text = $"{span.Seconds}{secondString}";
            }

            return text;
        }
    }
}