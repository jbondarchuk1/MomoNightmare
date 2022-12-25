 using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TraumaInducer : MonoBehaviour 
{
    public static TraumaInducer Instance { get; private set; }
    
    [Header("Instance Shake Values")]
    [Tooltip("Seconds to wait before trigerring the explosion particles and the trauma effect")]
    public float Delay = 0f;
    [Tooltip("Maximum stress the effect can inflict upon objects")][Range(0f,1f)]
    public float stressMultiplier = 0.6f;
    [Tooltip("Maximum distance in which objects are affected by this TraumaInducer")]
    public float Range = 45;
    
    [Space]
    [Header("Instance Settings")]
    public bool playOnStart = false;
    public bool continuousShake = false;

    [Space]
    [Header("Singleton Values")]
    public List<StressReceiver> receivers;
    [SerializeField] private bool singletonInstanceOverride = false;

    private bool isShaking = false;


    private IEnumerator Start()
    {
        if (Instance == null) Instance = this;
        else if (singletonInstanceOverride) Instance = this;

        if (playOnStart)
            yield return InduceStress(Range, stressMultiplier);
    }

    private void Update()
    {
        if (continuousShake && !isShaking)
            StartCoroutine(InduceStress(Range, stressMultiplier));
    }

    /// <summary>
    /// Instance method that automatically applies stress to all registered stress receivers
    /// </summary>
    public IEnumerator InduceStress(float range, float stressMultiplier)
    {
        isShaking = true;
        yield return new WaitForSeconds(Delay);
        foreach (StressReceiver r in receivers)
            InduceStress(range, stressMultiplier, r);
        isShaking = false;
    }

    /// <summary>
    /// Static method to shake an object
    /// </summary>
    public static void InduceStress(float range, float multiplier, StressReceiver r)
    {
        float distance = Vector3.Distance(r.transform.position, r.transform.position);
        float distance01 = Mathf.Clamp01(distance / range);
        float stress = (1-distance01) * multiplier;
        Debug.Log("stress: " + stress);
        r.InduceStress(stress);
    }


}