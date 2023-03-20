using UnityEngine;

public static class HelperFunctions {
    public static Color GetColorFromHex(string hex)
    {
        ColorUtility.TryParseHtmlString("#dedede", out Color hoverColor);
        return hoverColor;
    }

    public static Scene GetNextScene(int level)
    {
        return level switch
        {
            1 => Scene.Level1,
            2 => Scene.Level2,
            3 => Scene.Level3,
            _ => Scene.Level1,
        };
    }
}
