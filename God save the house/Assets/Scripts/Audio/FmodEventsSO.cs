using FMOD.Studio;
using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "FMODEvents", menuName = "ScriptableObjects/FMODEvents", order = 2)]
public class FmodEventsSO : ScriptableObject
{
    [Header("Musics")]
    public FmodEventRefAndInstance test1;

    [Header("Ambiances")]
    public FmodEventRefAndInstance test2;

    [Header("Gameplay")]
    public EventReference Explosion;

    [Header("Player")]
    public EventReference SpongePressed;
    public EventReference SpongeEmpty;
    public EventReference SpongeRefill;
    public EventReference SpongeFull;
}

[System.Serializable]
public class FmodEventRefAndInstance
{
    public EventReference reference;
    public EventInstance instance;
}