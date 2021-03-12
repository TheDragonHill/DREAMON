using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Managing Gamesave
/// </summary>
public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    public AutoSave currentAutoSave;
    public bool isNewGame = true;

    public delegate void LoadSave();
    public event LoadSave OnLoadSave;


    BinaryFormatter formatter;
    string autoPath;
    string settingsPath;
    SaveData data;
    SettingsData settingsData;


    private void Awake()
    {
        InitValues();
    }


    /// <summary>
    /// Load the saved Game
    /// </summary>
    public void LoadSavedGame()
    {
        if (SaveExists())
            SetDataValues();
    }

    public void LoadSaveInScene()
    {
        if(!isNewGame && SaveExists())
        {
            if(data.currentScene == SceneManager.GetActiveScene().buildIndex)
            {
                OnLoadSave();
            }
        }
    }

    #region Save Methods

    /// <summary>
    /// Save Interaction with Object
    /// </summary>
    /// <param name="interactIndex">"0" is default value</param>
    public void Save(string interactIndex)
    {
        if (!string.IsNullOrEmpty(interactIndex))
        {
            data.AddInteract(interactIndex);
            SaveGame();
        }
    }

    /// <summary>
    /// Save result of interaction
    /// </summary>
    /// <param name="resultIndex">"0" is default value</param>
    /// <param name="resultValue">the result</param>
    public void Save(string index, bool resultValue)
    {
        if (!string.IsNullOrEmpty(index))
        {
            data.AddInteract(index);
            data.AddResult(index, resultValue);
            SaveGame();
        }
    }

    /// <summary>
    /// Save current position of player
    /// </summary>
    public void Save(Vector3 pos, string animationState, int footstepIndex)
    {
        data.ChangePosition(pos);
        data.animationStateName = animationState;
        data.footstepIndex = footstepIndex;
        SaveGame();
    }

    public void SaveSettings(string index, float value)
    {
        settingsData.Add(index, value);
        SaveGameSettings();
    }

    public void SaveSettings(string index, int value)
    {
        settingsData.Add(index, value);
        SaveGameSettings();
    }

    #endregion

    #region Get Methods

    /// <summary>
    /// Check if player interacted with this object
    /// </summary>
    /// <param name="index">"0" is default value</param>
    /// <returns>Returns true if player interacted with object</returns>
    public bool HasInteracted(string index)
    {
        if (!string.IsNullOrEmpty(index))
            return data.GetInteraction(index);
            
        return false;
    }

    /// <summary>
    /// Check result of player actions
    /// </summary>
    /// <param name="index">index of resultObject</param>
    /// <returns>Returns "null" if player didn't interacted with it</returns>
    public bool? HasResult(string index)
    {
        if (!string.IsNullOrEmpty(index))
            return data.GetResult(index);

        return null;
    }

    /// <summary>
    /// Get the saved Playerposition
    /// </summary>
    /// <returns>Returns the Playerposition</returns>
    public Vector3 GetPlayerPosition()
    {
        return new Vector3(data.position[0], data.position[1], data.position[2]);
    }

    /// <summary>
    /// Get Animationstate name
    /// </summary>
    /// <returns>Animationstate as string</returns>
    public string GetAnimationState()
    {
        return data.animationStateName;
    }

    public int GetFootstepIndex()
    {
        return data.footstepIndex;
    }

    public float? GetSettingsFloat(string index)
    {
        return settingsData.GetFloat(index);
    }

    public int? GetSettingsInt(string index)
    {
        return settingsData.GetInt(index);
    }

    #endregion

    /// <summary>
    /// Create a new Game
    /// </summary>
    public void NewGame()
    {
        if (SaveExists())
        {
            File.Delete(autoPath);
            data = new SaveData();
            isNewGame = true;
        }
    }

    /// <summary>
    /// Check for SaveFile
    /// </summary>
    /// <returns></returns>
    public bool SaveExists()
    {
        return File.Exists(autoPath);
    }

    public bool SettingsExists()
    {
        return File.Exists(settingsPath);
    }

    void SaveGame()
    {
        data.currentScene = SceneManager.GetActiveScene().buildIndex;

        using (FileStream stream = File.Create(autoPath))
        {
            formatter.Serialize(stream, data);
        }
    }

    void SaveGameSettings()
    {
        using (FileStream stream = File.Create(settingsPath))
        {
            formatter.Serialize(stream, settingsData);
        }
    }

    void InitValues()
    {
        if (instance != null)
            Destroy(this.gameObject);

        instance = this;

        formatter = new BinaryFormatter();
        autoPath = Path.Combine(Application.streamingAssetsPath, "auto.dream");
        settingsPath = Path.Combine(Application.streamingAssetsPath, "settings.dream");
        data = LoadData();
        settingsData = LoadSettings();
        
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Loads the saved Player into his Scene
    /// </summary>
    void SetDataValues()
    {
        if (SceneManager.GetActiveScene().buildIndex != (int)data.currentScene)
        {
            SceneManager.LoadScene((int)data.currentScene);
        }
    }

    SaveData LoadData()
    {
        if (SaveExists())
        {
            using (FileStream stream = File.OpenRead(autoPath))
            {
                return formatter.Deserialize(stream) as SaveData;
            }
        }
        else
        {
            return new SaveData();
        }
    }

    SettingsData LoadSettings()
    {
        if(SettingsExists())
        {
            using (FileStream stream = File.OpenRead(settingsPath))
            {
                return formatter.Deserialize(stream) as SettingsData;
            }
        }
        else
        {
            return new SettingsData();
        }
    }

    public void IntegrateSettingsData()
    {
        if (settingsData.IsNew())
            return;


        float? valuef;
        int? valuei;

        for (int i = 0; i < AudioManager.Instance.volumeStrings.Length; i++)
        {
            // Get Value
            valuef = (float)settingsData.GetFloat(AudioManager.Instance.volumeStrings[i]);

            // Check for value was saved
            if (valuef != null)
            {
                AudioManager.Instance.SetCurrentVolume(i, (float)valuef);
            }
        }

        if((valuei = settingsData.GetInt("graphics")) != null)
        {
            QualitySettings.SetQualityLevel((int)valuei, true);
        }

        if((valuei = settingsData.GetInt("fullscreen")) != null)
        {
            Screen.fullScreen = (int)valuei >= 1;
        }

        if ((valuei = settingsData.GetInt("resolution")) != null)
        {
            Screen.SetResolution(Screen.resolutions[(int)valuei].width, Screen.resolutions[(int)valuei].height, Screen.fullScreen);
        }
    }
}
