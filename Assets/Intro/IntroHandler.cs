using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroLetterController : MonoBehaviour
{
    #region Variables

    // List of IntroLetterAnimation components to manage
    public List<IntroLetterAnimation> introLetterAnimations;

    // Duration for the entire sequence
    public float duration;

    // Animation curve for controlling the animation
    public AnimationCurve animationCurve;

    // Internal variable to track time
    private float elapsedTime = 0f;

    #endregion

    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        // Initialize anything if needed
    }

    // Update is called once per frame
    void Update()
    {
        // Increase the elapsed time
        elapsedTime += Time.deltaTime;

        // Clamp the elapsed time to the duration
        float clampedTime = Mathf.Clamp(elapsedTime, 0, duration);

        // Normalize the clamped time
        float normalizedTime = clampedTime / duration;

        // Get the value from the animation curve
        float curveValue = animationCurve.Evaluate(normalizedTime);

        // Calculate the index to call StartLetterAnim() on
        int index = Mathf.FloorToInt(curveValue * (introLetterAnimations.Count - 1));

        // Make sure the index is within the range of the list
        if (index >= 0 && index < introLetterAnimations.Count)
        {
            // Run the StartLetterAnim() method on the component at the calculated index
            introLetterAnimations[index].StartLetterAnim();
        }

        if (elapsedTime > duration + 5)
        {
            SceneManager.LoadScene("SandboxScene");
        }
    }

    #endregion
}
