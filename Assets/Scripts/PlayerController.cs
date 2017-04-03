using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public struct PlayerConfig
{
    public float maxSpeed;
    public float acceleration;
    public float movementDecay;
    public float minPositionX;
    public float maxPositionX;
    public float deathBounceSpeed;
    public Transform inventoryRoot;
    public int inventorySlotCount;
    public float inventorySlotSpacing;
}

public class PlayerController : MonoBehaviour, IPickupHolder
{
    public PlayerConfig config;

    private float acceleration = 0f;
    private Vector2 velocity = Vector2.zero;
    private Animator animator;
    private bool facingLeft;
    private Transform[] itemTransforms;
    private HealthProperty health;

    public bool IsGrabbing { get { return pickupReceiver.IsGrabbing; } }

    private PickupReceiver pickupReceiver;
    private Inventory inventory;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        pickupReceiver = GetComponent<PickupReceiver>();
        pickupReceiver.onPickupReceived += onPickupReceived;
        pickupReceiver.canPickupCallback += canPickup;

        health = GetComponent<HealthProperty>();
        health.onDeath += onDeath;

        itemTransforms = new Transform[config.inventorySlotCount];

        for (int i = 0; i < config.inventorySlotCount; ++i) {
            GameObject go = new GameObject(string.Format("slot_{0}", i));
            go.transform.SetParent(config.inventoryRoot);
            go.transform.localPosition = Vector3.up * config.inventorySlotSpacing * i;
            itemTransforms[i] = go.transform;
        }

        inventory = new Inventory(itemTransforms);
    }

    void Update()
    {
        if (!health.IsDead) {
            updateMovement();

            if (!IsGrabbing) {
                if (Input.GetButtonDown("Grab") && inventory.HasAvailableSpace) {
                    pickupReceiver.StartGrab();
                }
            }
            else {
                if (Input.GetButtonUp("Grab")) {
                    pickupReceiver.EndGrab();
                }
            }
        }
        else {
            velocity.y -= GameManagerLocator.GameManager.gameConfig.gravity * Time.deltaTime;
            Vector2 position = transform.position;
            position += velocity * Time.deltaTime;
            transform.position = position;
        }

        updateAnimationController();
    }

    private void updateMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");

        if (horizontal < 0) {
            facingLeft = true;
        }
        else if (horizontal > 0) {
            facingLeft = false;
        }

        acceleration = horizontal * getAcceleration() * Time.deltaTime;
        velocity.x += acceleration;
        velocity.x = Mathf.Clamp(velocity.x, -config.maxSpeed, config.maxSpeed);

        float decay = 0f;
        if (!Mathf.Approximately(velocity.x, 0) && Mathf.Approximately(horizontal, 0f)) {
            decay = getMovementDecay() * -Mathf.Sign(velocity.x) * Time.deltaTime;
        }

        if (velocity.x < 0 && velocity.x + decay > 0) {
            velocity.x = 0;
        }
        else if (velocity.x > 0 && velocity.x - decay < 0) {
            velocity.x = 0;
        }
        else {
            velocity.x += decay;
        }

        Vector2 position = transform.position;
        position += velocity * Time.deltaTime;

        if (position.x < config.minPositionX) {
            position.x = config.minPositionX;
            velocity.x = 0f;
        }
        else if (position.x > config.maxPositionX) {
            position.x = config.maxPositionX;
            velocity.x = 0f;
        }

        transform.position = position;
    }

    private void updateAnimationController()
    {
        animator.SetFloat("absolute_speed", Mathf.Abs(velocity.x));
        animator.SetFloat("absolute_accel", Mathf.Abs(acceleration));

        animator.transform.localScale = new Vector3((facingLeft) ? -1f : 1f, 1f, 1f);
        animator.SetBool("facing_left", facingLeft);

        animator.SetBool("is_grabbing", IsGrabbing);

        animator.SetBool("is_dead", GetComponent<HealthProperty>().IsDead);
    }

    private void onPickupReceived(PickupController pickup)
    {
        if (pickup.payload.name == "carrot") {
            addCarrot(pickup.transform.position);
        }
    }

    private void onDeath(GameObject sender)
    {
        inventory.RemoveAllItems();
        velocity = Vector2.up * config.deathBounceSpeed;
    }

    private bool canPickup(PickupController pickup)
    {
        return inventory.HasAvailableSpace;
    }

    private void addCarrot(Vector3 spawnPosition)
    {
        if (!inventory.HasAvailableSpace) {
            return;
        }

        var carrot = Instantiate(GameManagerLocator.GameManager.GetPrefab("carrot_item"));
        var heldItem = carrot.GetComponent<HeldItem>();
        inventory.AddItem(heldItem);
        carrot.transform.position = spawnPosition;
    }

    public HeldItem PopItem()
    {
        return inventory.PopItem();
    }

    private float getAcceleration()
    {
        if (!IsGrabbing) {
            return config.acceleration;
        }
        else {
            return 0f;
        }
    }

    private float getMovementDecay()
    {
        if (!IsGrabbing) {
            return config.movementDecay;
        }
        else {
            return config.movementDecay * 8;
        }
    }
}