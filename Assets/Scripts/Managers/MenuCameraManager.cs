using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class MenuCameraManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    public CinemachineCamera MenuCamera;
    public CinemachineCamera GameplayCamera;
    public CinemachineSplineDolly Dolly;
    public float Speed = 0.5f;
    public AnimationCurve SmoothCurve;
    const int MaxPriority = 100;

    public void GoToPlay()
    {
        MenuCamera.Priority.Value = 0;
        GetComponent<PlayableDirector>().Play();
    }

    public void GoToMainMenu()
    {
        MenuCamera.Priority.Value = MaxPriority;
        GameplayCamera.Priority.Value = 0;
        StartCoroutine(GoToTargetSplineValue(0.5f));
    }

    public void GoToCredits()
    {
        StartCoroutine(GoToTargetSplineValue(1));
    }

    public void GoToTutorial()
    {
        StartCoroutine(GoToTargetSplineValue(0));
    }

    IEnumerator GoToTargetSplineValue(float TargetValue)
    {
        float CurrentValue = Dolly.CameraPosition;
        float f = 0;

        while (Dolly.CameraPosition != TargetValue)
        {
            Dolly.CameraPosition = Mathf.Lerp(CurrentValue, TargetValue, SmoothCurve.Evaluate(f));
            f += Time.deltaTime * Speed;
            yield return null;
        }
        
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            GoToCredits();
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            GoToTutorial();
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            GoToMainMenu();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            GoToPlay();
        }
    }
}
