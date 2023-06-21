using UnityEngine;
using UnityEngine.UI;
using YG;

public class RewardedAd : MonoBehaviour
{
    [SerializeField] int AdID;
    private HealhSystem _healhSystem;

    void Awake()
    {
        _healhSystem = GetComponent<HealhSystem>();
    }

    private void OnEnable() => YandexGame.RewardVideoEvent += Rewarded;
    private void OnDisable() => YandexGame.RewardVideoEvent -= Rewarded;

    void Rewarded(int id)
    {
        if (id == AdID)
            AdRestoreHealh();
    }

    void AdRestoreHealh()
    {
        _healhSystem.RestoreFullHp();
        _healhSystem.HealhUpdate();
    }
}