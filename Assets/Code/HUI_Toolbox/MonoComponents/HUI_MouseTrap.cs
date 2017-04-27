

using UnityEngine.EventSystems;

namespace H_UI
{
    /// <summary>
    /// A mouse click blocking script with no other functionallity
    /// </summary>
    public class HUI_MouseTrap : UnityEngine.MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            return;
        }
    }
}