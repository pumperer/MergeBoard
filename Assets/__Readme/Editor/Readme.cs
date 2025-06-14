using UnityEngine;

namespace MergeBoard.Readme
{
    public class Readme : ScriptableObject
    {
        public string Title;

        [Multiline(100)]
        public string Description;
    }
}