using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signal
{
    public SignalType signalType;
    public bool isRepeated;

    private List<Plant> alreadyReceived = new List<Plant>();
    private Plant origin;

    public Signal(Plant origin, SignalType signalType, bool isRepeated = true)
    {
        this.origin = origin;
        this.signalType = signalType;
        this.isRepeated = isRepeated;
        alreadyReceived.Add(origin);
    }

    public void OnReceived(Plant receiver)
    {
        if(!alreadyReceived.Contains(receiver))
        {
            alreadyReceived.Add(receiver);
        }
    }

    public bool CanBeReceived(Plant receiver)
    {
        return !alreadyReceived.Contains(receiver);
    }

    public enum SignalType
    {
        Fume,
        Electric
    }
}
