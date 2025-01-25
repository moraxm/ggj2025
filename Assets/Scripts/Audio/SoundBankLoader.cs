using UnityEngine;
using System.Collections.Generic;
using FMODUnity;

public class SoundBankLoader : MonoBehaviour
{
    public enum SoundBankLoadingType
    {
        AWAKE = 0,
        START = 1,
        ONENABLE = 2
    }

    [SerializeField]
    private SoundBankLoadingType _loadType = SoundBankLoadingType.AWAKE;
    [SerializeField]
    [BankRef]
    private List<string> _banks = null;
    [SerializeField]
    private bool _preloadSamples = true;

	private void Awake()
	{
		if (_loadType == SoundBankLoadingType.AWAKE)
        {
            Load();
        }
	}

	private void OnEnable()
	{
		if (_loadType == SoundBankLoadingType.ONENABLE)
		{
			Load();
		}
	}

	private void Start()
    {
		if (_loadType == SoundBankLoadingType.START)
		{
			Load();
		}
	}

	private void OnDisable()
	{
		if (_loadType == SoundBankLoadingType.ONENABLE)
		{
			Unload();
		}
	}

	private void OnDestroy()
    {
		if (_loadType == SoundBankLoadingType.AWAKE || _loadType == SoundBankLoadingType.START)
		{
			Unload();
		}
	}

    public void Load()
    {
        foreach (var bankRef in _banks)
        {
            AudioManager.Instance.LoadBank(bankRef, _preloadSamples);
        }
    }

    public void Unload()
    {
        foreach (var bankRef in _banks)
        {
            AudioManager.Instance.UnloadBank(bankRef);
        }
    }
}
