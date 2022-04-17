using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : IgnoreCollision
{
    PlayerTiedEffectsManager effects;
    public float dimRate = 5f;
    public float effectIncreaseRate = 5f;
    public Transform playerLoc;
    public bool playerInRange = false;
    public Material fogMaterial;
    private List<ParticleSystem> fogParticles = new List<ParticleSystem>();
    private float initialFogOpacity = 255f;
    private float initialFogEffectOpactiy =255f;

    // Start is called before the first frame update
    void Start()
    {
        playerLoc = GameObject.Find("Player").transform;
        effects = GameObject.Find("Effects").GetComponent<PlayerTiedEffectsManager>();
        fogMaterial = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        StartCoroutine(HandleFogOpacity());
    }

    private IEnumerator HandleFogOpacity()
    {
        WaitForSeconds wait = new WaitForSeconds(.2f);

        //Color fogColor = fogMaterial.GetColor("Color");
        //Debug.Log(fogColor);
        //fogColor.a = (1 / (Vector3.Distance(playerLoc.position, transform.position))) * dimRate;
        //fogMaterial.color = fogColor; // probably not necessary
        if (playerInRange)
        {
            //Color fogColor = fogMaterial.color;
            //fogColor.a = (1 / (Vector3.Distance(playerLoc.position, transform.position))) * dimRate;
            //fogMaterial.color = fogColor; // probably not necessary

            foreach (ParticleSystem particles in fogParticles)
            {
                Color startColor = particles.main.startColor.color;
                startColor.a = (1 / (Vector3.Distance(playerLoc.position, transform.position))) * effectIncreaseRate;
            }
        }
        else
        {
            //Color fogColor = fogMaterial.shader.;
            //fogColor.a = initialFogOpacity;
            //foreach (ParticleSystem particles in fogParticles)
            //{
            //    Color startColor = particles.main.startColor.color;
            //    startColor.a = initialFogEffectOpactiy;
            //}
        }

        yield return wait;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("entering fog");

        playerInRange = true;
        if (other.name == "Player")
        {
            GameObject child = effects.ToggleEffect("Fog", true);
            foreach (ParticleSystem particles in child.GetComponentsInChildren<ParticleSystem>())
            {
                Color color = particles.main.startColor.color;
                color.a = 20f;


                ParticleSystem.MinMaxGradient ugh = particles.main.startColor;
                ugh.color = color;
                fogParticles.Add(particles);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        playerInRange = false;

        if (other.name == "Player")
        {
            effects.ToggleEffect("Fog", false);
        }
        fogParticles.Clear();
    }
}
