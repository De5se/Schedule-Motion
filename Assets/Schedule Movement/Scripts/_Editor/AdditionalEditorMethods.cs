using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts._Editor
{
    [ExecuteInEditMode]
    public class AdditionalEditorMethods : MonoBehaviour
    {
        [SerializeField, Range(1f, 10f), EnableIf(nameof(IsPlayMode))]
        private float timeScale;

        private bool IsPlayMode => Application.isPlaying;

        private void OnValidate()
        {
            Time.timeScale = timeScale;
        }
    }
}