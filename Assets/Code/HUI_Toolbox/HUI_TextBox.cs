

using UnityEngine;
using UnityEngine.UI;

namespace H_UI
{
    /// <summary>
    /// A scrollable box containing text
    /// </summary>
    public class HUI_TextBox
    {

        RectTransform root;
        Scrollbar scrollbar;
        Text text;
        RectTransform textRect;

        /// <summary>
        /// Constructor build textbox imidiatly
        /// </summary>
        public HUI_TextBox(RectTransform source, Vector2 min, Vector2 max, Font font)
        {
            //this.source = source;

            root = HUI.buildUIObject("TextBox", source, min, max, new Vector2(.5f, .5f));
            HUI.addImage(root, new Color(.3f, .3f, .3f, .3f));

            RectTransform viewPort = HUI.buildUIObject("ViewPort", root);
            scrollbar = HUI.addVerticalScrollbarChild(root);

            text = HUI.addTextChild(viewPort, Color.white, font, new Vector2(.02f, .02f), new Vector2(.98f, .98f), "", TextAnchor.UpperLeft);
            textRect = text.gameObject.GetComponent<RectTransform>();
            textRect.pivot = new Vector2(1f, 0f);
            HUI.addContentSizeFitter(textRect);

            HUI.addScrollRect(root, textRect, viewPort, scrollbar, false, true);
            root.gameObject.AddComponent<Mask>();
        }

        /// <summary>
        /// Sets text in textbox
        /// </summary>
        public void setText(string text)
        {
            this.text.text = text;
        }

        /// <summary>
        /// Appends text in textbox
        /// </summary>
        public void append(string text)
        {
            this.text.text += "\n" + text;
            //textRect.anchoredPosition = new Vector2(0f, 0f);
        }

        /// <summary>
        /// Destroy this obejct
        /// </summary>
        public void destroy()
        {
            GameObject.Destroy(root.gameObject);
        }
    }
}
