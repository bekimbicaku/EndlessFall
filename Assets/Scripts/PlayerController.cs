using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;
using static UnityEngine.UI.ScrollRect;


public class PlayerController : MonoBehaviour
{
    public enum MovementType { Tilt, Slide }
    public MovementType movementType;

    public float fallSpeed = 5f;
    public float slowFactor = 2f;
     private float fallSpeedIncreaseRate = 0.05f; // Speed increase rate over time
    private float maxFallSpeed = 12f; // Maximum limit for fall speed
    private float fallTimeElapsed = 0f; // Time counter for fall speed increment

    public float tiltSensitivity = 15f;
    private Rigidbody2D rb;
    private float originalFallSpeed;
    private bool isTouching = false;
    private bool isJumping = false;
    public bool isHelding = false;
    private string originalTag = "Player";

    public GameObject shield;
    public GameObject speedEffect;
    public GameObject fireEffect;
    public GameObject playerDecoration; // Imazhi rreth lojtarit
    public GameObject playerTrail; // Trail rreth lojtarit
    public float duration = 5f;
    public TextMeshProUGUI scoreTxt;
    public Animator coinanim;

    public bool scoreCalculationEnabled = true;
    private float scoreBeforeTeleport = 0f; // Stores the score before teleportation

    public float scoreMultiplier = 1f; // Multiplier for score calculation
    private float startYPosition; // Initial Y position of the player
    private float distanceTraveled; // Distance the player has traveled downwards
    private float score;
    public int intScore;
    public int highScore;
    private float extraPoints; // Extra points added by double points effect

    public bool doublePointsActive = false;
    public float doublePointsEndTime = 0f;
    public bool magnetActive = false;
    public float magnetEndTime = 0f;
    public bool shrinkActive = false;
    public float shrinkEndTime = 0f;
    private Stack<Vector3> positions;
    private bool timeWarpActive = false;
    private float timeWarpDuration;
    SpeedButtonControlle SpeedScript;
    PlayerHealth health;
    private GameManager gameManager;
    private PowerUpManager powerUpManager;

    public GameObject timeWarpPanel; // Panel asking if player wants to use Time Warp
    public TextMeshProUGUI timeWarpCountText; // Text displaying available Time Warps
    public int timeWarpCount = 0; // Number of Time Warps collected

    private bool hasPlayedSound = false;
    public Button timeWarpButton; // Button to trigger Time Warp
    public Button quitButton; // Button to quit the game
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI coinsText;
    public GameObject gameOverPanel;

    private Vector2 touchStartPos;
    private Vector2 touchCurrentPos;
    private int tapCount = 0;
    private float tapTimer = 0;
    private float doubleTapTime = 0.3f; // Time window for double-tap
    private float slideSensitivity = 10f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalFallSpeed = fallSpeed;
        startYPosition = transform.position.y;
        positions = new Stack<Vector3>();
        highScore = PlayerPrefs.GetInt("HighScore", 0);

        // Load movement settings from PlayerPrefs
        movementType = (MovementType)PlayerPrefs.GetInt("MovementType", (int)MovementType.Tilt);

        // Load tilt and slide sensitivity
        tiltSensitivity = PlayerPrefs.GetFloat("TiltSensitivity", 2f);
        slideSensitivity = PlayerPrefs.GetFloat("SlideSensitivity", 2f); // Load slide sensitivity

        timeWarpButton.onClick.AddListener(UseTimeWarp);
        quitButton.onClick.AddListener(QuitGame);
        gameOverPanel.SetActive(false);

        health = FindObjectOfType<PlayerHealth>();
        SpeedScript = FindObjectOfType<SpeedButtonControlle>();
        powerUpManager = FindObjectOfType<PowerUpManager>();
        gameManager = FindObjectOfType<GameManager>();

        if (SpeedScript == null) Debug.LogError("SpeedScript is not initialized");
        if (health == null) Debug.LogError("Health is not initialized");
        if (gameManager == null) Debug.LogError("GameManager is not initialized");
    }


    private void Update()
    {
        scoreTxt.text = intScore.ToString();
        HandleTouch();
        HandleFalling();
        HandleMovement(); // Handle movement based on player settings
        UpdateDistanceTraveled();
        UpdateScore();

        // Handle power-up deactivation
        if (doublePointsActive && Time.time > doublePointsEndTime) DeactivateDoublePoints();
        if (magnetActive && Time.time > magnetEndTime) DeactivateMagnet();
        if (shrinkActive && Time.time > shrinkEndTime) DeactivateShrink();

        if (!timeWarpActive)
        {
            positions.Push(transform.position);
            if (positions.Count > 500) positions.TrimExcess();
        }
    }


    private void HandleTouch()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                isTouching = true;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isTouching = false;
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary)
                {
                    isTouching = true;
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    isTouching = false;
                }
            }
        }
        else
        {
            isTouching = false;
        }
#endif
    }
    private void HandleMovement()
    {
        switch (movementType)
        {
            case MovementType.Tilt:
                HandleTiltMovement();
                break;
            case MovementType.Slide:
                HandleSlideMovement();
                break;
        }
    }

    // Tilt movement method
    private void HandleTiltMovement()
    {
        float move = 0f;

#if UNITY_EDITOR
        // Use arrow keys for movement in the editor
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            move = -tiltSensitivity; // Move left
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            move = tiltSensitivity; // Move right
        }
#else
    // Use tilt controls on mobile devices
    Vector3 tilt = Input.acceleration;
    move = tilt.x * tiltSensitivity;
#endif

        rb.linearVelocity = new Vector2(move, rb.linearVelocity.y);
    }

    // Slide movement method with adjustable sensitivity
    private void HandleSlideMovement()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                tapCount++;
                if (tapCount == 1)
                {
                    tapTimer = Time.time + doubleTapTime; // Start timer for detecting double-tap
                }
                else if (tapCount == 2 && Time.time <= tapTimer) // Detect double tap
                {
                    // If a double tap is detected, slow down the player when holding
                    fallSpeed = originalFallSpeed * 0.5f; // Reduce speed
                }

                touchStartPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                touchCurrentPos = touch.position;
                float moveX = (touchCurrentPos.x - touchStartPos.x) / Screen.width * slideSensitivity; // Apply slide sensitivity
                rb.linearVelocity = new Vector2(moveX * 10, rb.linearVelocity.y); // Apply movement
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                // Reset tap count and speed back to original after releasing the touch
                if (tapCount >= 2)
                {
                    fallSpeed = originalFallSpeed; // Reset speed after double tap ends
                }

                tapCount = 0; // Reset tap count after touch ends
            }
        }

        // Reset tap count if only a single tap is detected and too much time has passed
        if (tapCount == 1 && Time.time > tapTimer)
            tapCount = 0;
    }

    private void HandleFalling()
    {
        // Falling logic remains the same
        if (isJumping)
        {
            fallSpeed = 0f;
            fallTimeElapsed = 0f;
        }
        else if (isTouching)
        {
            fallSpeed -= slowFactor * Time.deltaTime * 6;
            fallSpeed = Mathf.Max(fallSpeed, originalFallSpeed * 0.1f);
            fallTimeElapsed = 0f;
        }
        else if (isHelding)
        {
            fallSpeed = 12f;
            fallTimeElapsed = 0f;
        }
        else
        {
            fallTimeElapsed += Time.deltaTime;
            fallSpeed = Mathf.Min(originalFallSpeed + fallTimeElapsed * fallSpeedIncreaseRate, maxFallSpeed);
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, -fallSpeed);
    }

    public void IncreaseFallSpeed()
    {
        float goingspeed = SpeedScript.indicatorValue;
        if (goingspeed >= 0.5)
        {
            fallSpeed = 12f;
            ActivateSpeedEffect();
        }
    }


    public void ResetFallSpeed()
    {
        fallSpeed = originalFallSpeed;
        StartCoroutine(DeactivateSpeedEffect());
    }
    private void ActivateSpeedEffect()
    {
        speedEffect.SetActive(true);
        fireEffect.SetActive(true);
        playerDecoration.SetActive(false);
        playerTrail.SetActive(false);

        ParticleSystem ps = speedEffect.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            var main = ps.main;
            main.loop = false;
        }
    }

    private IEnumerator DeactivateSpeedEffect()
    {
        yield return new WaitForSeconds(0.5f);
        fireEffect.SetActive(false);
        speedEffect.SetActive(false); // Deactivate the effect
        playerDecoration.SetActive(true);
        playerTrail.SetActive(true);
    }
    private void HandleTilt()
    {
        float move = 0f;

#if UNITY_EDITOR
        // Use keyboard arrow keys for movement in the editor
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            move = -tiltSensitivity; // Move left
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            move = tiltSensitivity; // Move right
        }
#else
    // Use tilt controls on mobile devices
    Vector3 tilt = Input.acceleration;
    move = tilt.x * tiltSensitivity;
#endif

        // Set the velocity for both editor and mobile
        rb.linearVelocity = new Vector2(move, rb.linearVelocity.y);
    }


    public void TriggerJump(float duration)
    {
        isJumping = true;
        StartCoroutine(ResetJumpState(duration));
    }

    private IEnumerator ResetJumpState(float duration)
    {
        yield return new WaitForSeconds(duration);
        isJumping = false;
    }

    public void ActivateShield()
    {
        shield.SetActive(true);
        ParticleSystem ps = shield.GetComponent<ParticleSystem>();
        SoundManager.instance.PlaySFX("OpenedShield");
        powerUpManager.ActivateShieldSlider(5f);
        if (ps != null)
        {
            var main = ps.main;
            main.loop = false;
        }
        StartCoroutine(ShieldCoroutine());
    }

    private IEnumerator ShieldCoroutine()
    {
        yield return new WaitForSeconds(duration);
        shield.SetActive(false);
        SoundManager.instance.StopSFX();

    }

    public void Coin()
    {
        coinanim.SetTrigger("Coinpop");
    }
    private void UpdateDistanceTraveled()
    {
        distanceTraveled = startYPosition - transform.position.y;
    }
    private void UpdateScore()
    {
        if (!scoreCalculationEnabled) return; // Skip score calculation if the flag is false

        // Calculate the current score based on distance traveled since last portal
        score = scoreBeforeTeleport + (distanceTraveled * scoreMultiplier) + extraPoints;
        intScore = Mathf.FloorToInt(score);
    }
    public void SaveScoreBeforeTeleport()
    {
        scoreBeforeTeleport = intScore; // Save the current score
    }
    public void ScoreAfterTeleoprt()
    {
        intScore = (int)scoreBeforeTeleport;
        startYPosition = transform.position.y; // Reset start position for distance calculation
    }
    private void UpdateScoreUI()
    {
        if (scoreTxt != null)
        {
            scoreTxt.text = "Score: " + intScore.ToString();
        }
    }

    public void ActivateDoublePoints(float duration)
    {
        if (doublePointsActive)
        {
            doublePointsEndTime = Time.time + duration;
            powerUpManager.ActivateDoublePointsSlider(10f);
        }
        else
        {
            doublePointsActive = true;
            doublePointsEndTime = Time.time + duration;
            StartCoroutine(DoublePointsCoroutine(duration));
        }
    }

    private IEnumerator DoublePointsCoroutine(float duration)
    {
        while (Time.time < doublePointsEndTime)
        {
            extraPoints += 2;
            //UpdateScoreUI();
            yield return new WaitForSeconds(1f);
        }
        DeactivateDoublePoints();
    }

    private void DeactivateDoublePoints()
    {
        doublePointsActive = false;
    }

    public void ActivateMagnet(float duration)
    {
        magnetActive = true;
        magnetEndTime = Time.time + duration;
        powerUpManager.ActivateMagnetSlider(10f);    
            
    }

    private void DeactivateMagnet()
    {
        magnetActive = false;
        // Remove magnet effect visuals
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (magnetActive)
        {
            if(other.CompareTag("Coin") || other.CompareTag("Heart") || other.CompareTag("ShieldObj") || other.CompareTag("2X"))
            {
                other.transform.position = Vector3.MoveTowards(other.transform.position, transform.position, 5f * Time.deltaTime);
            }
            
        }
        
    }
    public void ActivateShrink(float duration)
    {
        shrinkActive = true;
        shrinkEndTime = Time.time + duration;
        transform.localScale *= 0.5f;
    }

    private void DeactivateShrink()
    {
        shrinkActive = false;
        transform.localScale *= 2f;
    }

    public void ActivateTimeWarpPanel()
    {
        timeWarpPanel.SetActive(true);
        SoundManager.instance.PlaySFX("Dead");
        Time.timeScale = 0f;
        timeWarpCountText.text = PlayerPrefs.GetInt("TimeWarp", 0).ToString();
    }
    public void ActivateTimeWarp(float duration)
    {
        timeWarpActive = true;
        timeWarpDuration = duration;
        StartCoroutine(RewindTime());
    }

    private IEnumerator RewindTime()
    {
        float endTime = Time.time + timeWarpDuration;

        while (Time.time < endTime && positions.Count > 0)
        {
            transform.position = positions.Pop();
            yield return null;
        }

        timeWarpActive = false;
    }

    public void UseTimeWarp()
    {
        if (PlayerPrefs.GetInt("TimeWarp", 0) > 0)
        {
            PlayerPrefs.SetInt("TimeWarp", PlayerPrefs.GetInt("TimeWarp", 0) - 1);
            timeWarpPanel.SetActive(false);
            SoundManager.instance.StopSFX();
            Time.timeScale = 1f;
            health.RestoreHealth(150);
           
            StartCoroutine(ChangeTagTemporarily("Untagged", 5f));
            ActivateTimeWarp(3f); // Rewind 3 seconds
        }
    }
    private IEnumerator ChangeTagTemporarily(string newTag, float duration)
    {
        originalTag = gameObject.tag;
        gameObject.tag = newTag;

        yield return new WaitForSeconds(duration);

        gameObject.tag = originalTag;
    }
    public void LoseGame()
    {
        // Call the GameOver method in the GameManager
        GameOver();
    }
    public void GameOver()
    {
        // Show game-over panel
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;


        coinsText.text = "Coins: " + PlayerPrefs.GetInt("Coins", 0).ToString();
        currentScoreText.text = "Score: " + intScore.ToString();

        // Update the high score if the current score is higher
        if (intScore > highScore)
        {
            highScore = intScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();

            highScoreText.text = "New Highscore: " + highScore.ToString();
            highScoreText.color = Color.green;
        }
        else
        {
            highScoreText.text = "High Score: " + highScore.ToString();
        }
    }
    public void QuitGame()
    {
        timeWarpPanel.SetActive(false);
        SoundManager.instance.PlaySFX("EndGame");

        LoseGame();
    }
}
