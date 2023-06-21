using UnityEngine;
using UnityEngine.UI;
using YG;

public class NewResultLeaderboard : MonoBehaviour
{
    [SerializeField] LeaderboardYG leaderboardYG;
    [SerializeField] InputField scoreLbInputField;

    [Header("Set in dinamically")]
    private GameManager _gameManager;

    public void NewScore()
    {
        // Статический метод добавление нового рекорда
        string record = _gameManager.uitScoreSpeed.text;
        YandexGame.NewLeaderboardScores(leaderboardYG.nameLB, int.Parse(record));

        // Метод добавление нового рекорда обращением к компоненту LeaderboardYG
        // leaderboardYG.NewScore(int.Parse(scoreLbInputField.text));
    }

    public void NewScoreTimeConvert()
    {
        // Статический метод добавление нового рекорда конвертированного в time тип
        YandexGame.NewLBScoreTimeConvert(leaderboardYG.nameLB, float.Parse(scoreLbInputField.text));

        // Метод добавление нового рекорда обращением к компоненту LeaderboardYG
        // leaderboardYG.NewScoreTimeConvert(float.Parse(scoreLbInputField.text));
    }
}
