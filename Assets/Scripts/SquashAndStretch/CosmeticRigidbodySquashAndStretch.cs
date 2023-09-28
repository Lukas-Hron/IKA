using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CosmeticRigidbodySquashAndStretch : MonoBehaviour
{
    #region Serialized Fields
    [Tooltip("Minimum velocity for squashing and stretching")]
    [SerializeField] private float minVelocity = 0.01f;

    [Tooltip("Bias for squashing and stretching")]
    [SerializeField] private float bias = 1f;

    [Tooltip("Strength of the squash and stretch effect")]
    [SerializeField] private float strength = 0.08f;

    [Tooltip("Transform for the squasher object")]
    [SerializeField] private Transform squasher;

    [Tooltip("Transform for the upright object")]
    [SerializeField] private Transform upright;

    [Tooltip("If bouncing should be calculated")]
    [SerializeField] private bool shouldBounce;

    [Tooltip("Time taken for a single bounce")]
    [SerializeField] private float bounceTime = 1;
    #endregion

    #region Private Variables
    private Vector3 squasherStartScale;
    private AnimationCurve curve;
    private new Rigidbody rigidbody;
    private Vector3 velocity;
    private Vector3 lastVelocity;
    private float bounceMultiplier;
    private int bounceSequence;
    private float bounceTimeKeeper;
    private bool bouncing;
    #endregion

    #region Unity Lifecycle Methods
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        curve = Resources.Load<AnimationCurvePreset>("AnimationCurvePresets/bounce").curve;
        squasherStartScale = squasher.localScale;
    }

    void Update()
    {
        velocity = rigidbody.velocity;

        if (bouncing) Bounce();
        else ScaleByVelocity();

        LookAlongVelocity();
        ForceRotationIdentity();
    }

    private void FixedUpdate()
    {
        lastVelocity = rigidbody.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!shouldBounce) return;
        bouncing = true;
        bounceTimeKeeper = 0;
        bounceSequence = 0;
        Vector3 bounceNormal = collision.contacts[0].normal;
        float bounceIntensity = Mathf.Clamp(lastVelocity.magnitude, 0, 30) / 30f;
        bounceMultiplier = Mathf.Clamp01(Vector3.Dot(bounceNormal, -lastVelocity.normalized) * bounceIntensity);
    }
    #endregion

    #region Custom Methods
    void LookAlongVelocity()
    {
        if (velocity.magnitude < minVelocity)
            return;

        Quaternion rotation = Quaternion.LookRotation(velocity);
        squasher.rotation = rotation;
    }

    void ForceRotationIdentity()
    {
        upright.rotation = Quaternion.identity;
    }

    void ScaleByVelocity()
    {
        if (Mathf.Approximately(velocity.magnitude, 0f))
            return;

        float amount = velocity.magnitude * strength + bias;
        float inverseAmount = (1f / amount);
        squasher.localScale = new Vector3(inverseAmount, inverseAmount, amount);
    }

    public void Bounce()
    {
        bounceTimeKeeper += Time.deltaTime / bounceTime * 2;
        float amount = 1;

        switch (bounceSequence)
        {
            case 0:
                amount = Mathf.Lerp(velocity.magnitude * strength + bias, 1, bounceTimeKeeper * 10);
                if (bounceTimeKeeper * 10 > 1)
                {
                    bounceTimeKeeper -= 0.1f;
                    bounceSequence++;
                }
                break;
            case 1:
                amount = Mathf.Lerp(1, curve.Evaluate(bounceTimeKeeper), bounceMultiplier);
                if (bounceTimeKeeper > 0.5)
                {
                    bounceTimeKeeper -= 0.5f;
                    bounceSequence++;
                }
                break;
            case 2:
                amount = Mathf.Lerp(Mathf.Lerp(1, curve.Evaluate(bounceTimeKeeper + 0.5f), bounceMultiplier), velocity.magnitude * strength + bias, bounceTimeKeeper * 2);
                if (bounceTimeKeeper > 0.5)
                {
                    bounceTimeKeeper = 0;
                    bouncing = false;
                }
                break;
            default:
                return;
        }

        float inverseAmount = (1f / amount);
        squasher.localScale = new Vector3(inverseAmount, inverseAmount, amount);
    }
    #endregion
}
