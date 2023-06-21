using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ExpTrace : MonoBehaviour
    {
        [Header("Set in dinamically")]
        private Slider _slider;
        private GameManager _gameManaManager;

        private void OnEnable()
        {
            PlayerEventManager.OnUpdatedPoints += ExpBarUpdate;
            PlayerEventManager.OnSetedTotalPoint += SetMaxValue;
        }
        private void OnDisable()
        {
            PlayerEventManager.OnUpdatedPoints -= ExpBarUpdate;
            PlayerEventManager.OnSetedTotalPoint -= SetMaxValue;
        }

        // Use this for initialization

        private void Awake()
        {
            _gameManaManager = Camera.main.GetComponent<GameManager>();
            _slider = GetComponent<Slider>();
        }

        void ExpBarUpdate(int totalPoints)
        {
            //image.fillAmount = totalPoints / 100f;
            _slider.value = totalPoints;
        }

        void SetMaxValue(int value)
        {
            _slider.maxValue = value;
        }
    }
}