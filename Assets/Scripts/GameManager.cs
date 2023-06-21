using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts;
using System;
using System.Threading;

[System.Serializable]
public class Question
{
    public string question;
    public Sprite QuestSprite;
    public string[] answers = new string[4];
}

[System.Serializable]
public class Category
{
    public string NameCategory;
    public Question[] Questions;
}

public enum GameMode
{
    playing,
    menu
}

public class GameManager : MonoBehaviour
{


    [Header("Set in Inspector")]

    private Saver _saver;
    public Category[] Categories = new Category[6];
    public Demon[] Demons;
    public Button[] AnswerBtns = new Button[4];
    public Button[] LevelBtns = new Button[6];
    public Image DemonDisplay;
    public Image NewRankDemonDisplay;
    public Image QImage;
    public TextMeshProUGUI DemonNameTxt;
    public TextMeshProUGUI NewRankDemonNameTxt;
    public TextMeshProUGUI[] AnswersTxt;
    public TextMeshProUGUI[] LevelStatusTxt;
    public TextMeshProUGUI[] QuestGuessedTxt;
    public TextMeshProUGUI QTxt;
    public TextMeshProUGUI PointsTxt;
    public Color DefaultBtnColor;
    public TextMeshProUGUI uitTimer;
    public TextMeshProUGUI uitScoreSpeed;

    [Header("Set in Dinamically")]

    //public TextMeshProUGUI QuestionsCountTxt;
    private SoundManager _soundManager;
    private Question currentQ;
    private int _qRand;
    public int CategoryNum;
    private int _questNumber = 0;
    private HealhSystem _healhSystem;
    public int[] QuestionsCount;
    private int _questGuessed;
    public int totalPoints;
    private int _numberRunk = 0;
    private int _newNumberRunk;
    private bool _isNewRank = false;
    private string _demonName;
    public int[] points;
    public float[] SpeedPoints; 
    private PanelManager panelMananager;
    private int _rateSpeedPoints;
    private bool _onTimer;
    private float _timer;
    private GameMode _gameMode;
   
    List<object> qList;

    private void Awake() {
        //Application.targetFrameRate = 30;
    }

    private void FixedUpdate()
    {
        if (_onTimer)
        {
            _timer += Time.deltaTime;
            uitTimer.text = _timer.ToString("F0");
        }
        print(_timer);
    }
    private void Start() {
        panelMananager = GetComponent<PanelManager>();
        _rateSpeedPoints = 20000;
        DefaultBtnColor = AnswerBtns[0].gameObject.GetComponent<Image>().color;
        _saver = GetComponent<Saver>();
        QuestionsCount = new int[Categories.Length];
        points = new int[Categories.Length];
        SpeedPoints = new float[Categories.Length];
        _healhSystem = GetComponent<HealhSystem>();
        _soundManager = GetComponent<SoundManager>();
        _gameMode = GameMode.menu;

        for(int i = 0; i < Categories.Length; i++)
        {
            QuestionsCount[i] = Categories[i].Questions.Length;
            totalPoints += Categories[i].Questions.Length;
        }

        PlayerEventManager.OnSetedTotalPoint(totalPoints);
    }

    public void OnClickPlay()
    {
        qList = new List<object>(Categories[CategoryNum].Questions);
        QuestGenerate();
        _gameMode = GameMode.playing;
        _onTimer = true;
    }

    public void QuestGenerate()
    {
        _onTimer = true;
        if (qList.Count > 0)
        {
            _qRand = UnityEngine.Random.Range(0, qList.Count);
            currentQ = qList[_qRand] as Question;
            QTxt.text = currentQ.question;
            QImage.sprite = currentQ.QuestSprite;

            ++_questNumber;
            //QuestionsCountTxt.text = _questNumber.ToString() + "/" + QuestionsCount[CategoryNum].ToString();
            QTxt.gameObject.GetComponent<Animator>().SetTrigger("In");
            List<string> answers = new List<string>(currentQ.answers);
            for (int i = 0; i <currentQ.answers.Length; i++)
            {
                int rand = UnityEngine.Random.Range(0, answers.Count);
                AnswersTxt[i].text = answers[rand];
                answers.RemoveAt(rand);
            }

            StartCoroutine(AnimBtns());
        }

        else
        {
            panelMananager.LoadMenuPanel(panelMananager.GamePanel);
            Result();
            StopAllCoroutines();
        }   

    }

    IEnumerator AnimBtns()
    {
        yield return new WaitForSeconds(0.5f);

        for(int i = 0; i < AnswerBtns.Length; i++)
        {
            AnswerBtns[i].interactable = false;
        }

        int a = 0;
        while(a < AnswerBtns.Length)
        {
            if (!AnswerBtns[a].gameObject.activeSelf) 
            {
                AnswerBtns[a].gameObject.SetActive(true);
            }
            else 
            {
                AnswerBtns[a].gameObject.GetComponent<Animator>().SetTrigger("In");
                a++;
                yield return new WaitForSeconds(0.3f);
            }
        }

        for(int i = 0; i < AnswerBtns.Length; i++)
        {
            AnswerBtns[i].interactable = true;
        }

        yield break;
    }

    IEnumerator TrueOrFalse(bool check, int buttonIndex)
    {
        for(int i = 0; i < AnswerBtns.Length; i++)
        {
            AnswerBtns[i].interactable = false;
        }
        yield return new WaitForSeconds(1);

        if (check)
        {
            _questGuessed++;
            SubtractTimer(5f);
            AnswerBtns[buttonIndex].gameObject.GetComponent<Animator>().SetTrigger("TrueAnswer");
            _soundManager.GoodSound();
        
            yield return new WaitForSeconds(1);

            BtnsAnimOut(AnswerBtns);

            QTxt.gameObject.GetComponent<Animator>().SetTrigger("Out");
            yield return new WaitForSeconds(0.8f);

            qList.RemoveAt(_qRand);
            QuestGenerate();
            yield break;
        }
        else
        {
            AnswerBtns[buttonIndex].gameObject.GetComponent<Animator>().SetTrigger("FalseAnswer");
            _soundManager.ErrorSound();
            AddTimer(10f);

            yield return new WaitForSeconds(1);

            BtnsAnimOut(AnswerBtns);

            QTxt.gameObject.GetComponent<Animator>().SetTrigger("Out");
            yield return new WaitForSeconds(0.8f);

            qList.RemoveAt(_qRand);

            _healhSystem.Healh--;
            if (_healhSystem.Healh <= 0)
            {
                panelMananager.LoadLosePanel();
                _onTimer = false;
                _soundManager.PauseMusic();
                _soundManager.LoseSound();
            }
            else 
            {
                _healhSystem.HealhUpdate();
                QuestGenerate();
            }
            yield break;
        }
    }

    public void AnswersBtn(int index)
    {
        if (AnswersTxt[index].text == currentQ.answers[0]) 
        { 
            StartCoroutine(TrueOrFalse(true, index));
        }
        else 
        {
            StartCoroutine(TrueOrFalse(false, index));
        }
    }

    public void SetCategory(int index)
    {
        CategoryNum = index;
    }

    public void SetPoitsTxt()
    {
        PointsTxt.text = TotalPoints(points).ToString() + " из " + totalPoints.ToString();
    }

    void BtnsAnimOut(Button[] btns)
    {
        for (int i = 0; i < btns.Length; i++)
            {
                btns[i].gameObject.GetComponent<Animator>().SetTrigger("Out");
                btns[i].gameObject.GetComponent<Image>().color = DefaultBtnColor;
                btns[i].gameObject.SetActive(false);
            }  
    }

    public int TotalPoints(int[] array)
    {
        int total = 0;
        for (int i = 0; i < array.Length; i++) total += array[i];

        return total;
    }

    public void CheckRank()
    {
        _isNewRank = false;
        for (int i = 0; i < Demons.Length; i++)
        {
            if (Demons[i].ScoreNeed <= TotalPoints(points))
            {
                _newNumberRunk = i;
                DemonDisplay.sprite = Demons[i].DemonSprite;
                _demonName = Demons[i].DemonName;
                DemonNameTxt.text = "Ранг: " + Demons[i].DemonName;
            }
        }

        if(_newNumberRunk > _numberRunk)
        {
            _isNewRank = true;
            _numberRunk = _newNumberRunk;
        }
    }

    public void LevelManager()
    {
        for (int i = 0; i < LevelBtns.Length; i++)
        {
            int levelBlock = (i * QuestionsCount[i]) - (4 * i);

            if (TotalPoints(points) < levelBlock )
            {
                LevelBtns[i].interactable = false;
                QuestGuessedTxt[i].text = "Баллов до открытия: " + (levelBlock - TotalPoints(points)).ToString();
                LevelStatusTxt[i].text = "закрыто";
            }
            else if (TotalPoints(points) >= levelBlock)
            {
                LevelBtns[i].interactable = true;
                QuestGuessedTxt[i].text = points[i].ToString() + "/" + QuestionsCount[i].ToString();
                LevelStatusTxt[i].text = "";
            }
        }
    }

    public void Result()
    {
        _onTimer = false;
        if (points[CategoryNum] < _questGuessed)
        {
            points[CategoryNum] = _questGuessed;
        }

        _questNumber = 0;
        _questGuessed = 0;
        _healhSystem.RestoreFullHp();
        _healhSystem.HealhUpdate();

        
        _onTimer = false;
        SetPoitsTxt();

        if (_gameMode == GameMode.playing) SetSpeedPoints();
        _saver.Save();
        _gameMode = GameMode.menu;
        SetScoreSpeedTxt();
        CheckRank();
        LevelManager();
        PlayerEventManager.OnUpdatedPoints?.Invoke(TotalPoints(points));
        _timer = 0;

        if (TotalPoints(points) == totalPoints)
        {
            panelMananager.LoadWinPanel(panelMananager.MenuPanel);
            _soundManager.StopMusic();
            _soundManager.WinSound();
        }
        else if(_isNewRank)
        {
            panelMananager.LoadNewRankPanel(panelMananager.MenuPanel);
            _soundManager.WinSound();
            NewRankDemonDisplay.sprite = DemonDisplay.sprite;
            NewRankDemonNameTxt.text = "Твой новый ранг: " + _demonName;
        }
    }

    public void SetScoreSpeedTxt()
    {
        uitScoreSpeed.text = TotalSpeedPoints(SpeedPoints).ToString();
    }

    public float TotalSpeedPoints(float[] array)
    {
        float total = 0;

        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] != 0) total += _rateSpeedPoints / array[i];
        }

        return MathF.Round(total);
    }

    void SetSpeedPoints()
    {
        int penaltyTime = (QuestionsCount[CategoryNum] - points[CategoryNum]) * 10;
        AddTimer(penaltyTime);

        if (SpeedPoints[CategoryNum] == 0)
        {
            SpeedPoints[CategoryNum] = Mathf.Floor(_timer);
        }
        else
        {
            if (SpeedPoints[CategoryNum] > _timer)
            {
                SpeedPoints[CategoryNum] = Mathf.Floor(_timer);
            }
        }
    }

    public void ClearSpeedPoints()
    {
        for (int i = 0; i < SpeedPoints.Length; i++)
        {
            SpeedPoints[i] = 0;
        }
    }

    private void SubtractTimer(float value)
    {
        if (_timer - value <= 0) _timer = 0;
        else _timer -= value;
    }

    private void AddTimer(float value)
    {
        _timer += value;
    }

}
