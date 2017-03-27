using System;
using System.Collections.Generic;
using UnityEngine;

public enum RockState
{
    Spawn,
    Idle,
    Slip,
    Fall,
}

[Serializable]
public class RockConfig
{
    public float spawnSpeed;
    public float fallSpeed;
    public float spawnHeight;
    public float idleHeight;
    public float deadHeight;
    public float minSpawnTime;
    public float maxSpawnTime;
    public float slipHeight;
    public AnimationCurve slipCurve;
    public float slipTime;
}

public class RockController : MonoBehaviour
{
    private RockConfig config;
    private RockState state;
    private float idleTime = 0;
    private bool isInitialized = false;
    private float slipBaseHeight = 0f;
    private float slipStartTime = 0f;

    public RockState State { get { return state; } }

    private delegate void StateUpdateHandler();
    private delegate void StateStartHandler(RockState oldState);

    private Dictionary<RockState, StateUpdateHandler> onStateUpdate = new Dictionary<RockState, StateUpdateHandler>();
    private Dictionary<RockState, StateStartHandler> onStateStart = new Dictionary<RockState, StateStartHandler>();

    public void Init(RockConfig config, Vector3 position)
    {
        this.config = config;

        addState(RockState.Spawn, onSpawnStart, onSpawnUpdate);
        addState(RockState.Idle, onIdleStart, onIdleUpdate);
        addState(RockState.Slip, onSlipStart, onSlipUpdate);
        addState(RockState.Fall, onFallStart, onFallUpdate);

        transform.position = position;

        isInitialized = true;
        setState(RockState.Spawn);
    }

    public void Respawn()
    {
        setState(RockState.Spawn);
    }

    void Update()
    {
        if (!isInitialized) {
            return;
        }

        if (onStateUpdate.ContainsKey(state)) {
            onStateUpdate[state]();
        }
    }

    private void addState(RockState state, StateStartHandler onStart, StateUpdateHandler onUpdate)
    {
        onStateStart.Add(state, onStart);
        onStateUpdate.Add(state, onUpdate);
    }

    private void setState(RockState state)
    {
        if (!isInitialized) {
            return;
        }

        RockState oldState = this.state;
        this.state = state;
        if (onStateStart.ContainsKey(this.state)) {
            onStateStart[this.state](oldState);
        }
    }

    private void onSpawnUpdate()
    {
        Vector3 position = transform.position;
        position.y -= config.spawnSpeed * Time.deltaTime;
        if (position.y <= config.idleHeight) {
            position.y = config.idleHeight;
            setState(RockState.Idle);
        }
        transform.position = position;
    }

    private void onIdleUpdate()
    {
        if (Time.time >= idleTime) {
            setState(RockState.Slip);
        }
    }

    private void onSlipUpdate()
    {
        float time = Time.time - slipStartTime;
        float curve = config.slipCurve.Evaluate(time / config.slipTime);

        Vector3 position = transform.position;
        position.y = slipBaseHeight - (config.slipHeight * curve);
        transform.position = position;

        if (Time.time >= slipStartTime + config.slipTime) {
            setState(RockState.Fall);
        }
    }

    private void onFallUpdate()
    {
        Vector3 position = transform.position;
        position.y -= config.fallSpeed * Time.deltaTime;
        transform.position = position;
        if (position.y <= config.deadHeight) {
            setState(RockState.Spawn);
        }
    }

    private void onSpawnStart(RockState oldState)
    {
        idleTime = 0f;
        Vector3 position = transform.position;
        position.y = config.spawnHeight;
        transform.position = position;
    }

    private void onIdleStart(RockState oldState)
    {
        idleTime = Time.time + UnityEngine.Random.Range(config.minSpawnTime, config.maxSpawnTime);
    }

    private void onSlipStart(RockState oldState)
    {
        slipBaseHeight = transform.position.y;
        slipStartTime = Time.time;
    }

    private void onFallStart(RockState oldState)
    {

    }
}