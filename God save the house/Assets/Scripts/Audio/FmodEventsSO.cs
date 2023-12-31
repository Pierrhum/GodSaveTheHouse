using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "FMODEvents", menuName = "ScriptableObjects/FMODEvents", order = 2)]
public class FmodEventsSO : ScriptableObject
{
    [Header("UI")]
    public EventReference UIButton;
    
    [Header("Musics")]
    public FmodEventRefAndInstance Victory;
    public FmodEventRefAndInstance Defeat;

    [Header("Ambiances")]
    public EventReference Ambiance;

    [Header("Gameplay")]
    public EventReference Explosion;
    public List<EventReference> FireLevel;
    public List<EventReference> HouseBurning;
    public List<EventReference> HouseSelected;
    public EventReference HouseSafe;
    public List<EventReference> RainOnFire;
    public List<EventReference> RainOnWater;

    [Header("Sponge")]
    public EventReference SpongePressed;
    public EventReference SpongeRaining;
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