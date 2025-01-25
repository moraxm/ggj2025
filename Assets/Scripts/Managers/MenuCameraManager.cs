using System.Collections;
using Unity.Cinemachine;
using UnityEditor.UIElements;
using UnityEngine;

public class MenuCameraManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    public CinemachineCamera Camera;
    public CinemachineSplineDolly Dolly;
    float Speed = 0.5f;
    public AnimationCurve SmoothCurve;

    public void GoToMainMenu()
    {
        StartCoroutine(GoToTargetSplineValue(0.5f));
    }

    public void GoToCredits()
    {
        StartCoroutine(GoToTargetSplineValue(0));
    }

    public void GoToTutorial()
    {
        StartCoroutine(GoToTargetSplineValue(1));
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
    }
}
