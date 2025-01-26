using UnityEngine;
using UnityEngine.UI;
using static PowerUpManager;

public class PowerUpUI : MonoBehaviour
{
    public string _powerUpName;

    public Image _loadingBar;

    public void OnClick()
    {
        PowerUpManager.Instance.StartPowerUp(_powerUpName);
    }


    public void Update()
    {
        if(!PowerUpManager.Instance.LevelStarted)
        {
            _loadingBar.fillAmount = 1;
            return;
        }

        PowerUp p = PowerUpManager.Instance.GetPowerUpData(_powerUpName);
        float timeRemaining = p.TimeRemaining;

        if(p.InUse)
        {
            //in use
            float progress = timeRemaining / p._timeUse;
            _loadingBar.fillAmount = (1 - progress);
        }

        if(!p.CanBeUsed)
        {
            //recharge
            float progress = timeRemaining / p._timeRecharge;
            _loadingBar.fillAmount = progress;
        }
        else
        {
            if(!p.InUse)
            {
                _loadingBar.fillAmount = 0;
            }
        }
    }
}
