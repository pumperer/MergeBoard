using alpoLib.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MergeBoard.UI
{
    public class UIProgressComp : CachedUIBehaviour
    {
        [SerializeField] protected Slider progressSlider;
        [SerializeField] protected TMP_Text progressText;
        
        public void SetProgress(float progress)
        {
            if (progressSlider)
                progressSlider.value = progress;
        }

        public void SetText(string text)
        {
            if (progressText)
                progressText.text = text;
        }
    }
}