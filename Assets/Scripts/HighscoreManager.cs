using System.IO;
using UnityEngine;

[System.Serializable]
public class HighscoreData
{
    public int highscore;
}


public static class HighscoreManager
{
    private static string filePath = Path.Combine(Application.persistentDataPath, "highscore.json");

    public static void SaveHighscore(int score)
    {
        int currentHighscore = LoadHighscore();

        // only save if new score is higher
        if (score > currentHighscore)
        {
            HighscoreData data = new HighscoreData { highscore = score };
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, json);
        }
    }

    public static int LoadHighscore()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            HighscoreData data = JsonUtility.FromJson<HighscoreData>(json);
            return data.highscore;
        }
        return 0;
    }
}
