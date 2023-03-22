using UnityEngine.SceneManagement;
public enum Scene
{
    MainMenu,
    Level1,
    Level2,
    Level3,
    Defeat,
    Victory,
}

public static class Loader
{
    public static void Load(Scene scene)
    {
        SceneManager.LoadSceneAsync(scene.ToString());
    }
}
