﻿namespace GleyLocalization
{
    using GleyPlugins;
    using UnityEditor;
    using UnityEngine;

    public class GoogleTranslation
    {
        private EditorFileLoaded fileLoader;
        private TranslatedWord translatedWord;
        private SupportedLanguages toLanguage;
        string url;

        /// <summary>
        /// Make a request to Google Translate to translate a word
        /// </summary>
        /// <param name="from">word to translate</param>
        /// <param name="fromLanguage">original language</param>
        /// <param name="translatedWord">translated word</param>
        /// <param name="toLanguage">language to translate in</param>
        public GoogleTranslation(string from, SupportedLanguages fromLanguage, TranslatedWord translatedWord, SupportedLanguages toLanguage)
        {
            this.translatedWord = translatedWord;
            fileLoader = new EditorFileLoaded();
            this.toLanguage = toLanguage;
            EditorApplication.update = MyUpdate;
            url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl="
              + LanguageCode.GetLanguageCode((SystemLanguage)fromLanguage) + "&tl=" + LanguageCode.GetLanguageCode((SystemLanguage)toLanguage) + "&dt=t&q=" + WWW.EscapeURL(from);
            fileLoader.LoadFile(url);
        }


        /// <summary>
        /// Editor update method
        /// </summary>
        private void MyUpdate()
        {
            if (fileLoader.IsDone())
            {
              
                EditorApplication.update = null;
                string result = fileLoader.GetResult();
               // Debug.Log("RESULT " + url);
                var N = JSONNode.Parse(result);
                string translatedText = N[0][0][0];
                translatedWord.SetWord(translatedText, toLanguage);
            }
        }
    }
}
