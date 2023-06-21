using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
    public class PanelManager : MonoBehaviour
    {
        [Header("Set in Inspector")]
        public GameObject GamePanel;
        public GameObject LosePanel;
        public GameObject WinPanel;
        public GameObject NewRankPanel;
        public GameObject MenuPanel;
        public GameObject BackPanel;
        public Animator BackAnimator;

        public void LoadGamePanel(GameObject panelDeactivate)
        {
            panelDeactivate.SetActive(false);
            GamePanel.SetActive(true);
        }

        public void LoadLosePanel(GameObject panelDeactivate)
        {
            panelDeactivate.SetActive(false);
            LosePanel.SetActive(true);
        }

        public void LoadLosePanel()
        {
            LosePanel.SetActive(true);
        }

        public void LoadWinPanel(GameObject panelDeactivate)
        {
            panelDeactivate.SetActive(false);
            WinPanel.SetActive(true);
        }

        public void LoadNewRankPanel(GameObject panelDeactivate)
        {
            panelDeactivate.SetActive(false);
            NewRankPanel.SetActive(true);
        }

        public void LoadMenuPanel(GameObject panelDeactivate)
        {
            panelDeactivate.SetActive(false);
            MenuPanel.SetActive(true);
        }

    }
}