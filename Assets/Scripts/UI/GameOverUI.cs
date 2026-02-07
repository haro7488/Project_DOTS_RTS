using System;
using UnityEngine;

namespace DotsRts.UI
{
    public class GameOverUI : MonoBehaviour
    {
        private void Start()
        {
            DOTSEventsManager.Instance.OnHQDead += DOTSEventsManager_OnHQDead;
            Hide();
        }

        private void DOTSEventsManager_OnHQDead(object sender, EventArgs e)
        {
            Show();
            Time.timeScale = 0;
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}