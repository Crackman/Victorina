using UnityEngine;
using UnityEngine.UI;
using TMPro;
using YG;

public class Saver : MonoBehaviour
{
    private GameManager _gameManager;
    private SoundManager _soundManager;

    private void OnEnable() => YandexGame.GetDataEvent += GetLoad;
    private void OnDisable() => YandexGame.GetDataEvent -= GetLoad;

    private void Start() {
        _gameManager =  GetComponent<GameManager>();
        _soundManager = GetComponent<SoundManager>();
    }

    private void Awake()
    {
        if (YandexGame.SDKEnabled)
            GetLoad();
    }

    public void Save()
    {
        for(int i = 0; i < _gameManager.points.Length; i++)
        {
            YandexGame.savesData.points[i] = _gameManager.points[i];
            YandexGame.savesData.SpeedPoints[i] = _gameManager.SpeedPoints[i];
        }

        YandexGame.savesData.MusicOn = _soundManager.MusicOn;

        YandexGame.SaveProgress();
    }

    public void Load() => YandexGame.LoadProgress();

    public void GetLoad()
    {
        for (int i = 0; i < _gameManager.points.Length; i++)
        {
            _gameManager.points[i] = YandexGame.savesData.points[i];
            _gameManager.SpeedPoints[i] = YandexGame.savesData.SpeedPoints[i];
        }

        for(int i = 0; i < _gameManager.Categories.Length; i++)
        {
            _gameManager.QuestGuessedTxt[i].text = YandexGame.savesData.points[i].ToString() + "/" + _gameManager.QuestionsCount[i].ToString();
        }
        _gameManager.CheckRank();
        _gameManager.LevelManager();
        _gameManager.SetPoitsTxt();
        _gameManager.SetScoreSpeedTxt();
        PlayerEventManager.OnUpdatedPoints?.Invoke(_gameManager.TotalPoints(_gameManager.points));

        _soundManager.MusicOn = YandexGame.savesData.MusicOn;

        if(_soundManager.MusicOn)
        {
           _soundManager.MusicButton.GetComponent<Image>().sprite = _soundManager.onMusic;
        }
        else if (!_soundManager.MusicOn)
        {
            _soundManager.MusicButton.GetComponent<Image>().sprite = _soundManager.offMusic;
        }
    }
}