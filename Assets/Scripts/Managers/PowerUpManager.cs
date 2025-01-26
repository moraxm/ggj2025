using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    private static PowerUpManager _instance = null;
    public static PowerUpManager Instance => _instance;

    [System.Serializable]
    public struct PowerUp
    {
        public string _name;
        public float _timeUse;
        public float _timeRecharge;

        private float _timeRemaining;
        private bool _inUse;
        private bool _canBeUsed;

        public bool UpdateTimeRemaining(float t)
        {
            _timeRemaining -= t;
            return _timeRemaining <= 0;
        }

        public void SetTimeRemaining(float t)
        {
            _timeRemaining = t;
        }

        public float TimeRemaining => _timeRemaining;

        public bool InUse
        {
            get => _inUse;
            set => _inUse = value;
        }
        public bool CanBeUsed
        {
            get => _canBeUsed;
            set => _canBeUsed = value;
        }
    }

    [SerializeField] PowerUp[] _powerups;

    private bool _levelStarted = false;

    public bool LevelStarted => _levelStarted;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartLevel()
    {
        _levelStarted = true;
        ResetAll();
    }

    public void EndLevel()
    {
        _levelStarted = false;
    }

    private void Update()
    {
        if (!_levelStarted) { return; }

        UpdateTimeRecharge();
        UpdatePowerUpsInUse();
    }

    void ResetAll()
    {
        for (int i = 0; i < _powerups.Length; ++i)
        {
            ResetPowerUp(i);
        }
    }

    void ResetPowerUp(int index)
    {
        _powerups[index].InUse = false;
        _powerups[index].SetTimeRemaining(_powerups[index]._timeRecharge);
        _powerups[index].CanBeUsed = false;
    }

    void UpdateTimeRecharge()
    {
        float t = Time.deltaTime;
        for(int i  = 0; i < _powerups.Length; ++i)
        {
            if (_powerups[i].CanBeUsed) { continue; }
            bool changeStatus = _powerups[i].UpdateTimeRemaining(t);
            if(changeStatus)
            {
                _powerups[i].CanBeUsed = true;
            }
        }
    }

    void UpdatePowerUpsInUse()
    {
        float t = Time.deltaTime;
        for (int i = 0; i < _powerups.Length; ++i)
        {
            if (!_powerups[i].InUse) { continue; }
            bool changeStatus = _powerups[i].UpdateTimeRemaining(t);
            if (changeStatus)
            {
                ResetPowerUp(i);
            }
        }
    }

    public List<string> GetActivePowerUps()
    {
        List<string> toReturn = new List<string>();

        for (int i = 0; i < _powerups.Length; ++i)
        {
            if (_powerups[i].InUse)
            {
                toReturn.Add(_powerups[i]._name);
            }
        }
        return toReturn;
    }

    public PowerUp GetPowerUpData(string name)
    {
        for (int i = 0; i < _powerups.Length; ++i)
        {
            if (_powerups[i]._name == name)
            {
                return _powerups[i];
            }
        }
        return _powerups[0];
    }

    public bool StartPowerUp(string name)
    {
        for (int i = 0; i < _powerups.Length; ++i)
        {
            if (_powerups[i]._name == name)
            {
                if (_powerups[i].CanBeUsed)
                {
                    _powerups[i].InUse = true;
                    _powerups[i].SetTimeRemaining(_powerups[i]._timeUse);
                    return true;
                }
                return false;
            }
        }
        return false;
    }

    public bool IsPowerUpInUse(string name)
    {
        for (int i = 0; i < _powerups.Length; ++i)
        {
            if (_powerups[i]._name == name)
            {
                return _powerups[i].InUse;
            }
        }

        return false;
    }

}
