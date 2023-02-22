using UnityEngine;

namespace HomaGames.Internal.BaseTemplate.Examples.Basic
{
    /// <summary>
    /// Just a basic example of a UI Manager... to show how can it be connected to a GameState change event
    /// </summary>
    public class UIBehaviour : MonoBehaviour
    {
        private CanvasGroup m_canvasGroup;

        private void Awake()
        {
            m_canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void Show()
        {
            m_canvasGroup.alpha = 1;
            m_canvasGroup.blocksRaycasts = true;
        }

        public virtual void Hide()
        {
            m_canvasGroup.alpha = 0;
            m_canvasGroup.blocksRaycasts = false;
        }
    }
}