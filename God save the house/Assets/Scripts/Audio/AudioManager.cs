using FMOD.Studio;
using FMODUnity;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class AudioManager : Singleton<AudioManager>
{

    public static FmodEventsSO fmodEvents;
    [SerializeField] private FmodEventsSO _fmodEvents;
    [SerializeField] private bool needDebug = false;

    private void Awake()
    {
        fmodEvents = _fmodEvents;
    }

    public void SetGlobalParameter(string parameterName, int value)
    {
        RuntimeManager.StudioSystem.setParameterByName(parameterName, value);
    }
    public void SetGlobalParameter(string parameterName, float value)
    {
        RuntimeManager.StudioSystem.setParameterByName(parameterName, value);
    }

    public void SetEventInstanceParameter(EventInstance eventInstance, string parameterName, int value)
    {
        eventInstance.setParameterByName(parameterName, value);
    }

    public void PlayOnShotEvent(EventReference eventReference)
    {
        RuntimeManager.PlayOneShot(eventReference);
        if(needDebug)
            Debug.Log("[Audio] " + $"Playing one shot event {eventReference}.");
    }

    public EventInstance PlayEvent(EventReference eventReference)
    {
        EventInstance eventInstance;
        eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstance.start();
        eventInstance.release();
        if(needDebug)
            Debug.Log("[Audio] " + $"Playing event {GetEventPath(eventInstance)}.");
        return eventInstance;
    }

    public void PlayEvent(EventInstance eventInstance)
    {
        eventInstance.start();
        eventInstance.release();
        if(needDebug)
            Debug.Log("[Audio] " + $"Playing event {GetEventPath(eventInstance)}.");
    }

    public void PlayEvent(FmodEventRefAndInstance refAndInst)
    {
        refAndInst.instance = RuntimeManager.CreateInstance(refAndInst.reference);
        refAndInst.instance.start();
        refAndInst.instance.release();
        Debug.Log("[Audio] " + $"Playing event {GetEventPath(refAndInst.instance)}.");
    }

    public void StopEvent(EventInstance eventInstance)
    {
        if (eventInstance.isValid())
        {
            if (IsEventInstancePlaying(eventInstance))
            {
                eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                eventInstance.release();
                if(needDebug)
                    Debug.Log("[Audio] " + $"Event instance {GetEventPath(eventInstance)} has been stopped successfully!");
            }
            else
            {
                if(needDebug)
                    Debug.Log("[Audio] " + $"Event instance {GetEventPath(eventInstance)} is already stopped.");
            }
        }
        else
        {
            if(needDebug)
                Debug.Log("[Audio] " + $"Event instance {GetEventPath(eventInstance)} was not valid!");
        }
    }

    public float GetEventLengthInSeconds(EventReference eventReference)
    {
        EventDescription eventDescription = RuntimeManager.GetEventDescription(eventReference);
        int lenght;
        eventDescription.getLength(out lenght);
        return lenght / 1000f;
    }

    public string GetEventPath(EventInstance eventInstance)
    {
        EventDescription description;
        eventInstance.getDescription(out description);
        string path;
        description.getPath(out path);
        return path;
    }

    public EventInstance CreateAndGetEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance;
        eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }

    public bool IsEventInstanceValid(EventInstance eventInstance)
    {
        EventDescription eventDescription;
        eventInstance.getDescription(out eventDescription);

        return eventDescription.isValid();
    }

    public bool IsEventInstancePlaying(EventInstance instance)
    {
        PLAYBACK_STATE state;
        instance.getPlaybackState(out state);
        return state != PLAYBACK_STATE.STOPPED;
    }
}
