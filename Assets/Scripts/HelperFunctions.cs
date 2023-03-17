using UnityEngine;

public static class HelperFunctions {
    public static Color GetColorFromHex(string hex)
    {
        ColorUtility.TryParseHtmlString("#dedede", out Color hoverColor);
        return hoverColor;
    }
}
