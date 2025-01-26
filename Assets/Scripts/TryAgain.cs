using UnityEngine;
using UnityEngine.Playables;

public class TryAgain : MonoBehaviour
{
    public void ActivateRestart()
    {
        InputManager.Instance.RegisterOnBackPerformed(OnBack);
    }

    private void OnDestroy()
    {
        InputManager.Instance.UnregisterOnBackPerformed(OnBack);
    }

    private void OnBack()
    {
        PlayableDirector Director = GetComponent<PlayableDirector>();
        if (Director != null && Director.state != PlayState.Playing)
        {
            GameManager.Instance.GoBackToMainMenu();
            InputManager.Instance.UnregisterOnBackPerformed(OnBack);
        }
    }
}
