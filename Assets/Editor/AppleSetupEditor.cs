using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Editor utility to set up the apple collection system in the scene.
/// Run this from the Unity menu: Tools > Setup Apple Collection System
/// </summary>
public class AppleSetupEditor : EditorWindow
{
    [MenuItem("Tools/Setup Apple Collection System")]
    static void SetupAppleCollectionSystem()
    {
        // Load the scene if not already loaded
        Scene scene = SceneManager.GetActiveScene();
        if (!scene.isLoaded)
        {
            Debug.LogError("No scene loaded!");
            return;
        }

        Debug.Log("Setting up Apple Collection System...");

        // Step 1: Find and update Apple 1
        GameObject apple1 = GameObject.Find("Apple 1");
        if (apple1 != null)
        {
            SetupApple(apple1);
            Debug.Log("Apple 1 updated successfully");
        }
        else
        {
            Debug.LogError("Apple 1 not found in scene!");
            return;
        }

        // Step 2: Create 19 more apples (Apple 2 through Apple 20)
        for (int i = 2; i <= 20; i++)
        {
            GameObject newApple = CreateApple(i, apple1);
            Debug.Log($"Created Apple {i}");
        }

        // Step 3: Setup UI for Score Counter
        SetupScoreUI();

        // Step 4: Disable dialogue system
        DisableDialogueSystem();

        // Mark scene as dirty and save
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("Apple Collection System setup complete!");
        EditorUtility.DisplayDialog("Success", "Apple Collection System has been set up successfully!", "OK");
    }

    static void SetupApple(GameObject apple)
    {
        // Remove Interactable script if exists
        Interactable interactable = apple.GetComponent<Interactable>();
        if (interactable != null)
        {
            DestroyImmediate(interactable);
        }

        // Remove Rigidbody2D if exists (apples should not have physics)
        Rigidbody2D rb = apple.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            DestroyImmediate(rb);
        }

        // Add or update BoxCollider2D to be a trigger
        BoxCollider2D collider = apple.GetComponent<BoxCollider2D>();
        if (collider == null)
        {
            collider = apple.AddComponent<BoxCollider2D>();
        }
        collider.isTrigger = true;

        // Add AppleCollectible script if not present
        if (apple.GetComponent<AppleCollectible>() == null)
        {
            apple.AddComponent<AppleCollectible>();
        }
    }

    static GameObject CreateApple(int index, GameObject referenceApple)
    {
        // Clone the reference apple
        GameObject newApple = Instantiate(referenceApple);
        newApple.name = $"Apple {index}";

        // Generate random position at different heights
        float xRange = 20f; // Spread across the floor
        float minY = 2f;
        float maxY = 8f;
        
        float x = Random.Range(-xRange/2, xRange/2);
        float y = Random.Range(minY, maxY);
        
        newApple.transform.position = new Vector3(x, y, 0);

        return newApple;
    }

    static void SetupScoreUI()
    {
        // Find or create Canvas
        Canvas canvas = FindAnyObjectByType<Canvas>();
        GameObject canvasObj;
        
        if (canvas == null)
        {
            canvasObj = new GameObject("ScoreCanvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }
        else
        {
            canvasObj = canvas.gameObject;
        }

        // Create ScoreCounter Text object
        GameObject scoreCounterObj = new GameObject("ScoreCounter");
        scoreCounterObj.transform.SetParent(canvasObj.transform, false);

        Text scoreText = scoreCounterObj.AddComponent<Text>();
        scoreText.text = "00";
        scoreText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        scoreText.fontSize = 48;
        scoreText.color = Color.white;
        scoreText.alignment = TextAnchor.UpperRight;

        // Position in top-right corner
        RectTransform rectTransform = scoreCounterObj.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(1, 1);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.pivot = new Vector2(1, 1);
        rectTransform.anchoredPosition = new Vector2(-20, -20);
        rectTransform.sizeDelta = new Vector2(200, 60);

        // Create or find ScoreManager
        GameObject scoreManagerObj = GameObject.Find("ScoreManager");
        if (scoreManagerObj == null)
        {
            scoreManagerObj = new GameObject("ScoreManager");
        }

        ScoreManager scoreManager = scoreManagerObj.GetComponent<ScoreManager>();
        if (scoreManager == null)
        {
            scoreManager = scoreManagerObj.AddComponent<ScoreManager>();
        }
        scoreManager.scoreText = scoreText;

        Debug.Log("Score UI setup complete");
    }

    static void DisableDialogueSystem()
    {
        // Find DialogueManager GameObject
        GameObject dialogueManager = GameObject.Find("DialogueManager");
        if (dialogueManager != null)
        {
            dialogueManager.SetActive(false);
            Debug.Log("Dialogue system disabled");
        }

        // Find DialogueCanvas GameObject  
        GameObject dialogueCanvas = GameObject.Find("DialogueCanvas");
        if (dialogueCanvas != null)
        {
            dialogueCanvas.SetActive(false);
            Debug.Log("Dialogue canvas disabled");
        }
    }
}
