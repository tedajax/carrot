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
}

public class PlayerController : MonoBehaviour, IPickupHolder
{
    public PlayerConfig config;
    public Transform[] itemTransforms;

    private float acceleration = 0f;
    private Vector2 velocity = Vector2.zero;
    private Animator animator;
    private bool facingLeft;

    private PickupReceiver pickupReceiver;
    private Inventory inventory;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        pickupReceiver = GetComponent<PickupReceiver>();
        pickupReceiver.onPickupReceived += onPickupReceived;
        pickupReceiver.canPickupCallback += canPickup;

        inventory = new Inventory(itemTransforms);
    }

    void Update()
    {
        updateMovement();

        bool grabPressed = Input.GetButtonDown("Grab");
        if (grabPressed) {
            pickupReceiver.AttemptPickup();
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

        acceleration = horizontal * config.acceleration * Time.deltaTime;
        velocity.x += acceleration;
        velocity.x = Mathf.Clamp(velocity.x, -config.maxSpeed, config.maxSpeed);

        float decay = 0f;
        if (!Mathf.Approximately(velocity.x, 0) && Mathf.Approximately(horizontal, 0f)) {
            decay = config.movementDecay * -Mathf.Sign(velocity.x) * Time.deltaTime;
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
    }

    private void onPickupReceived(PickupPayload payload)
    {
        if (payload.name == "carrot") {
            addCarrot();
        }
    }

    private bool canPickup(PickupPayload payload)
    {
        return inventory.HasAvailableSpace;
    }

    private void addCarrot()
    {
        if (!inventory.HasAvailableSpace) {
            return;
        }

        var carrot = Instantiate(GameManagerLocator.GameManager.GetPrefab("carrot_item"));
        var heldItem = carrot.GetComponent<HeldItem>();
        inventory.AddItem(heldItem);
    }

    public HeldItem PopItem()
    {
        return inventory.PopItem();
    }
}