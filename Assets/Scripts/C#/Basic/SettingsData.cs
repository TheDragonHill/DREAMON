using System.Collections.Generic;
using System;

[Serializable]
public class SettingsData
{

    public Dictionary<string, float> floatHistory;
    public Dictionary<string, int> intHistory;

    public SettingsData()
    {
        floatHistory = new Dictionary<string, float>();
        intHistory = new Dictionary<string, int>();
    }


    public void Add(string index, float value)
    {
        if (floatHistory.ContainsKey(index))
        {
            floatHistory[index] = value;
        }
        else
        {
            floatHistory.Add(index, value);
        }
    }

    public void Add(string index, int value)
    {
        if(intHistory.ContainsKey(index))
        {
            intHistory[index] = value;
        }
        else
        {
            intHistory.Add(index, value);
        }
    }

    public float? GetFloat(string index)
    {
        float value;

        if (floatHistory.TryGetValue(index, out value))
            return value;
        else
            return null;
    }

    public int? GetInt(string index)
    {
        int value;

        if (intHistory.TryGetValue(index, out value))
            return value;
        else
            return null;
    }

    public bool IsNew()
    {
        return floatHistory.Count == 0 && intHistory.Count == 0;
    }

}
