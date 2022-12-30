using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    // references
    [SerializeField] private CinemachineImpulseSource impulseSource;
    public static CameraShake Instance; 

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void ShakeCamera(float shakeTime, float shakeAmplitude, float shakeFrequency)
    {
        impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = shakeTime;
        impulseSource.m_ImpulseDefinition.m_AmplitudeGain = shakeAmplitude;
        impulseSource.m_ImpulseDefinition.m_FrequencyGain = shakeFrequency;
        impulseSource.GenerateImpulse();

        Vibrator.Vibrate(5000);
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(.05f);
        Vibrator.Cancel();
    }
   

}
