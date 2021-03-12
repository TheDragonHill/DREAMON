using DG.Tweening;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Spawn Ghostlights in Scene
/// </summary>
public class GhostLightSpawn : MonoBehaviour
{
    /// <summary>
    /// Ghostlight Objects
    /// </summary>
    [SerializeField]
    List<GameObject> ghostLights;

    /// <summary>
    /// How fast the Ghostlights spawn
    /// </summary>
    [SerializeField]
    float speed = 0.8f;

    /// <summary>
    /// Lights from ghostLights
    /// </summary>
    Light[] lightComponents;

    /// <summary>
    /// Halos from lightComponents
    /// </summary>
    Behaviour[] halos;

    /// <summary>
    /// Position of the current point to search for the nearest light
    /// </summary>
    Vector3 currentPoint;

    bool isTrigger = false;

    void Start()
    {
        InitValues();
    }

    /// <summary>
    /// Init values
    /// </summary>
    void InitValues()
    {
        // Get Light
        lightComponents = transform.GetComponentsInChildren<Light>();
        
        // Get Halos
        halos = new Behaviour[lightComponents.Length];
        for (int i = 0; i < lightComponents.Length; i++)
        {
            halos[i] = (Behaviour)lightComponents[i].GetComponent("Halo");
        }
        
        // Reset all
        ResetLight(0);
        for (int i = 0; i < ghostLights.Count; i++)
        {
            ghostLights[i].SetActive(false);
        }

        // Set currentPoint on default value
        currentPoint = ghostLights[0].transform.position;
        isTrigger = GetComponent<Collider>() ? GetComponent<Collider>().isTrigger : false;
    }

    private void OnEnable()
    {
        if(!isTrigger)
            Light(1);
    }

    private void OnDisable()
    {
        DisableLight(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check Player
        if (other.GetComponent<PlayerController>())
        {
            currentPoint = other.transform.position;
            Light(1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check Player
        if (other.GetComponent<PlayerController>())
        {
            currentPoint = other.transform.position;
            Light(0);
        }
    }

    /// <summary>
    /// Light it up / down
    /// </summary>
    /// <param name="size">Value between 0 - 1 to determine if the lights are going out</param>
    public void Light(int size)
    {
        if(gameObject.activeInHierarchy && ghostLights != null && lightComponents != null && ghostLights.Count > 0 && lightComponents.Length > 0)
        {
            StopAllCoroutines();
            ResetLight(size);
            StartCoroutine(WaitForLight(size));
        }
    }

    /// <summary>
    /// Reset all values of active / inactive lights
    /// </summary>
    /// <param name="size">Value between 0 - 1 to determine if the lights are going out</param>
    private void ResetLight(int size)
    {
        GameObject[] buffer = ghostLights.FindAll(g => g.activeInHierarchy == size < 1).ToArray();

        if (buffer.Length <= 0)
            return;

        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i].transform.localScale = Vector3.one * (size >= 1 ? 0 : 1);
            halos[ghostLights.IndexOf(buffer[i])].enabled = size < 1;
            lightComponents[ghostLights.IndexOf(buffer[i])].intensity = 1 * (size >= 1 ? 0 : 1);
        }
    }

    /// <summary>
    /// Disable / Enable all lights completely
    /// </summary>
    /// <param name="size">Value between 0 - 1 to determine if the lights are going out</param>
    void DisableLight(int size)
    {

        if (gameObject.activeInHierarchy && ghostLights != null && lightComponents != null && ghostLights.Count > 0 && lightComponents.Length > 0)
        {
            for (int i = 0; i < ghostLights.Count; i++)
            {
                ghostLights[i].transform.localScale = Vector3.one * (size >= 1 ? 0 : 1);
                halos[i].enabled = size < 1;
                lightComponents[i].intensity = 1 * (size >= 1 ? 0 : 1);
                ghostLights[i].SetActive(size >= 1);
            }
        }
    }

    /// <summary>
    /// Make Time between turning on / off lights
    /// </summary>
    IEnumerator WaitForLight(int size)
    {

        GameObject currentLight = ClosestLight(size < 1);

        if(currentLight)
        {
            do
            {
                // Set lights active on turning on
                if(size >= 1)
                {
                    currentLight.SetActive(true);
                }

                // Scale light
                currentLight.transform.DOScale(size, speed);

                // Set Intensity slowly
                lightComponents[ghostLights.IndexOf(currentLight)].DOIntensity(size, speed);

                // Wait for light
                yield return new WaitForSeconds(speed);

                // Set lights inactive on turning off
                if (size < 1)
                {
                    currentLight.SetActive(false);
                }

                // Turn Halo on
                halos[ghostLights.IndexOf(currentLight)].enabled = size >= 1;
            
                // Set next light
                currentLight = ClosestLight(size < 1);


            } while (currentLight && currentLight.activeSelf == size < 1);
        }
    }

    /// <summary>
    /// Get the closest active/inactive light
    /// </summary>
    /// <param name="isActive">Are the lights active?</param>
    /// <returns>Closest Light to current Point</returns>
    GameObject ClosestLight(bool isActive)
    {
        // Check for inactive / active lights
        GameObject[] availableLights = ghostLights.FindAll(g => g.activeInHierarchy == isActive).ToArray();

        if (availableLights.Length <= 0)
            return null;


        GameObject closestLight = availableLights[0];

        // Go through all available Lights to get the closest Light
        for (int i = 0; i < availableLights.Length; i++)
        {
            if(
                Vector2.Distance(
                    new Vector2(currentPoint.x, currentPoint.z), 
                    new Vector2(availableLights[i].transform.position.x, availableLights[i].transform.position.z))
                < 
                Vector2.Distance(
                    new Vector2(currentPoint.x, currentPoint.z), 
                    new Vector2(closestLight.transform.position.x, closestLight.transform.position.z)))
            {
                closestLight = availableLights[i];
            }
        }

        // Set the currentPoint on this Light
        currentPoint = closestLight.transform.position;

        return closestLight;
    }
}
