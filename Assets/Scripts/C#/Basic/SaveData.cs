using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data to save for the Game
/// </summary>
[Serializable]
public class SaveData
{
    public float[] position;
    public int currentScene;
    public Dictionary<string, bool> resultHistory;
    public Dictionary<string, bool> interactHistory;
    public string animationStateName;
    public int footstepIndex;

    /// <summary>
    /// Initialise values
    /// </summary>
    public SaveData()
    {
        position = new float[3];
        resultHistory = new Dictionary<string, bool>();
        interactHistory = new Dictionary<string, bool>();
        animationStateName = string.Empty;
    }

    /// <summary>
    /// Change saved position
    /// </summary>
    /// <param name="pos">New position</param>
    public void ChangePosition(Vector3 pos)
    {
        position = new float[3] { pos.x, pos.y, pos.z };
    }


    /// <summary>
    /// Add result to dictionary
    /// </summary>
    /// <param name="index">unique index of value</param>
    /// <param name="value">new value</param>
    public void AddResult(string index, bool value)
    {
        // Check if dictionary already contains the index
        if (resultHistory.ContainsKey(index))
        {
            resultHistory[index] = value;
        }
        else
        {
            resultHistory.Add(index, value);
        }
    }

    /// <summary>
    /// Add interact to dictionary
    /// </summary>
    /// <param name="index">unique index of value</param>
    public void AddInteract(string index)
    {
        //Always change to true
        interactHistory.Add(index, true);
    }

    /// <summary>
    /// Return result for index
    /// </summary>
    /// <param name="index">unique index</param>
    /// <returns>result value or null for no value found</returns>
    public bool? GetResult(string index)
    {
        bool value;

        if (resultHistory.TryGetValue(index, out value))
            return value;
        else
            return null;
    }

    /// <summary>
    /// Is there a object, which was interacted with?
    /// </summary>
    /// <param name="index">unique index</param>
    /// <returns>true for interacted, false not interacted or value not found</returns>
    public bool GetInteraction(string index)
    {
        return interactHistory.ContainsKey(index);
    }
}
