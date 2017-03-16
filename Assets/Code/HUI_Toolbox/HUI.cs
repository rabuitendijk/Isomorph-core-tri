
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Help fucntions for generating HUI
/// </summary>
static class HUI {

    /// <summary>
    /// Build a ui object and set it to full scaling size
    /// </summary>
    public static RectTransform buildUIObject(string ob_name, Transform parent)
    {
        GameObject ob = new GameObject() { name = ob_name };
        RectTransform rect = ob.AddComponent<RectTransform>() as RectTransform;
        rect.SetParent(parent);
        setRectFull(rect);
        return rect;
    }

    public static RectTransform buildUIObject(string ob_name, Transform parent, Vector2 min, Vector2 max, Vector2 pivot)
    {
        GameObject ob = new GameObject() { name = ob_name };
        RectTransform rect = ob.AddComponent<RectTransform>() as RectTransform;
        rect.SetParent(parent);
        setRectFull(rect, min, max, pivot);
        return rect;
    }

    /// <summary>
    /// Set UI object to full scaling size
    /// </summary>
    public static void setRectFull(RectTransform rect)
    {
        rect.anchorMin = new Vector2(0f, 0f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(.5f, .5f);
        rect.sizeDelta = new Vector2(0f, 0f);
        rect.position = new Vector3(0f, 0f, 0f);
        rect.offsetMax = new Vector2(0f, 0f);
        rect.offsetMin = new Vector2(0f, 0f);
    }


    /// <summary>
    /// Set UI object to full scaling size
    /// </summary>
    public static void setRectFull(RectTransform rect, Vector2 min, Vector2 max, Vector2 pivot)
    {
        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.pivot = pivot;
        rect.sizeDelta = new Vector2(0f, 0f);
        rect.position = new Vector3(0f, 0f, 0f);
        rect.offsetMax = new Vector2(0f, 0f);
        rect.offsetMin = new Vector2(0f, 0f);
    }

    public static Image addImage(RectTransform rect, Color color, Sprite sprite = null, bool sliced=true, bool filled=true, bool racastTarget=true)
    {
        Image i = rect.gameObject.AddComponent<Image>();
        i.color = color;


        if (sprite != null)
        {
            i.sprite = sprite;
            if (sliced)
                i.type = Image.Type.Sliced;
            else
                i.type = Image.Type.Simple;

            i.fillCenter = filled;
        }

        i.raycastTarget = racastTarget;

        return i;
    }


    public static ScrollRect addScrollRect(RectTransform rect, RectTransform target, bool horizontal = true, bool vertical = true, float scrollSensitivity = 15f)
    {
        ScrollRect sr = rect.gameObject.AddComponent<ScrollRect>() as ScrollRect;
        sr.horizontal = horizontal;
        sr.vertical = vertical;
        sr.content = target;
        sr.scrollSensitivity = scrollSensitivity;

        return sr;
    }

    public static ScrollRect addScrollRect(RectTransform rect, RectTransform target, RectTransform viewport, Scrollbar verticalScrollbar, bool horizontal = true, bool vertical = true, float scrollSensitivity = 15f)
    {
        ScrollRect sr = rect.gameObject.AddComponent<ScrollRect>() as ScrollRect;
        sr.horizontal = horizontal;
        sr.vertical = vertical;
        sr.content = target;
        sr.scrollSensitivity = scrollSensitivity;
        sr.viewport = viewport;
        sr.verticalScrollbar = verticalScrollbar;

        return sr;
    }

    public static Text addTextChild(RectTransform rect, Color textColor, Font font, string text = "", TextAnchor align = TextAnchor.MiddleLeft, bool raycastTarget = false)
    {
        RectTransform childRect = HUI.buildUIObject("TextChild", rect);

        return addText(childRect, textColor, font, text, align, raycastTarget);
    }

    public static Text addTextChild(RectTransform rect, Color textColor, Font font, Vector2 min, Vector2 max, string text = "", TextAnchor align = TextAnchor.MiddleLeft, bool raycastTarget=false)
    {
        RectTransform childRect = HUI.buildUIObject("TextChild", rect, min, max, new Vector2(.5f, .5f));

        return addText(childRect, textColor, font, text, align, raycastTarget);
    }

    public static Text addTextChild(RectTransform rect, Color textColor, Font font, int fontSize, Vector2 min, Vector2 max, string text = "", bool resizeTextForBestFit = false, TextAnchor align = TextAnchor.MiddleLeft, bool raycastTarget = false)
    {
        RectTransform childRect = HUI.buildUIObject("TextChild", rect, min, max, new Vector2(.5f, .5f));

        return addText(childRect, textColor, font, fontSize, text, resizeTextForBestFit, align, raycastTarget);
    }

    public static Text addText(RectTransform rect, Color textColor, Font font, string txt = "", TextAnchor align = TextAnchor.MiddleLeft, bool raycastTarget = false)
    {
        Text text = rect.gameObject.AddComponent<Text>();
        text.font = font;
        text.alignment = align;
        text.text = txt;
        text.color = textColor;
        text.raycastTarget = raycastTarget;
        return text;
    }

    public static Text addText(RectTransform rect, Color textColor, Font font, int fontSize, string txt = "", bool resizeTextForBestFit = false, TextAnchor align = TextAnchor.MiddleLeft, bool raycastTarget = false)
    {
        Text text = rect.gameObject.AddComponent<Text>();
        text.font = font;
        text.alignment = align;
        text.text = txt;
        text.color = textColor;
        text.raycastTarget = raycastTarget;
        text.fontSize = fontSize;
        text.resizeTextForBestFit = resizeTextForBestFit;
        return text;
    }


    public static VerticalLayoutGroup addVerticalLayoutGroup(RectTransform rect, RectOffset padding, float spacing, TextAnchor align = TextAnchor.UpperLeft, bool ControlHeight = false, bool ControlWidth = true, bool ForceExpandHeight = false, bool ForceExpandWidth = true)
    {
        VerticalLayoutGroup vlg = rect.gameObject.AddComponent<VerticalLayoutGroup>();
        vlg.padding = padding;
        vlg.childControlHeight = ControlHeight;
        vlg.childControlWidth = ControlWidth;
        vlg.childForceExpandHeight = ForceExpandHeight;
        vlg.childForceExpandWidth = ForceExpandWidth;
        vlg.childAlignment = align;
        vlg.spacing = spacing;

        return vlg;
    }

    public static ContentSizeFitter addContentSizeFitter(RectTransform rect, ContentSizeFitter.FitMode horzontal = ContentSizeFitter.FitMode.Unconstrained, ContentSizeFitter.FitMode vertical = ContentSizeFitter.FitMode.PreferredSize)
    {
        ContentSizeFitter csf = rect.gameObject.AddComponent<ContentSizeFitter>();
        csf.horizontalFit = horzontal;
        csf.verticalFit = vertical;

        return csf;
    }

    public static InputField addInputField(RectTransform rect, Color textColor, Font font, string initialText = "", bool bestFit=false)
    {
        HUI.addImage(rect, new Color(.5f, .5f, .5f));
        Text target = HUI.addTextChild(rect, textColor, font, 32, new Vector2(.02f, 0f), new Vector2(1f, 1f), initialText, bestFit);

        InputField i = rect.gameObject.AddComponent<InputField>();
        i.textComponent = target;
        i.text = initialText;

        return i;
    }


    public static Scrollbar addVerticalScrollbarChild(RectTransform rect)
    {
        RectTransform child = buildUIObject("Scrollbar", rect);
        child.anchorMin = new Vector2(1f, 0f);
        child.sizeDelta = new Vector2(20f, 0f);
        return addVerticalScrollbar(child);
    }

    public static Scrollbar addVerticalScrollbar(RectTransform rect)
    {
        addImage(rect, Color.gray);

        RectTransform slidingArea = buildUIObject("Sliding Area", rect);
        RectTransform handle = buildUIObject("Handle", slidingArea);
        addImage(handle, Color.white);

        Scrollbar sb = rect.gameObject.AddComponent<Scrollbar>();
        sb.direction = Scrollbar.Direction.BottomToTop;
        sb.handleRect = handle;

        return sb;
    }
}
