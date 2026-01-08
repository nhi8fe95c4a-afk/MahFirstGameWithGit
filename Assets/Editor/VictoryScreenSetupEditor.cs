using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
/// <summary>
/// Editor tool to automatically set up the Victory Screen UI.
/// Accessible from Tools > Setup Victory Screen in Unity Editor.
/// </summary>
public class VictoryScreenSetupEditor : EditorWindow
{
    [MenuItem("Tools/Setup Victory Screen")]
    public static void ShowWindow()
    {
        GetWindow<VictoryScreenSetupEditor>("Victory Screen Setup");
    }

    void OnGUI()
    {
        GUILayout.Label("Victory Screen Setup", EditorStyles.boldLabel);
        GUILayout.Space(10);

        GUILayout.Label("This tool will create:", EditorStyles.wordWrappedLabel);
        GUILayout.Label("• Black background panel (full screen)", EditorStyles.wordWrappedLabel);
        GUILayout.Label("• Red victory text in center", EditorStyles.wordWrappedLabel);
        GUILayout.Label("• VictoryScreen component", EditorStyles.wordWrappedLabel);
        GUILayout.Label("• Link to ScoreManager", EditorStyles.wordWrappedLabel);
        GUILayout.Space(10);

        if (GUILayout.Button("Setup Victory Screen", GUILayout.Height(30)))
        {
            SetupVictoryScreen();
        }

        GUILayout.Space(10);
        GUILayout.Label("Note: Ensure you have a Canvas in the scene.", EditorStyles.helpBox);
    }

    private void SetupVictoryScreen()
    {
        // Find or create canvas
        Canvas canvas = FindAnyObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("VictoryCanvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100; // Ensure it renders on top of everything
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            Debug.Log("Created new Canvas for Victory Screen");
        }
        else
        {
            // Ensure existing canvas is set to overlay mode and high sort order
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;
        }

        // Create VictoryScreen GameObject
        GameObject victoryScreenObj = new GameObject("VictoryScreen");
        victoryScreenObj.transform.SetParent(canvas.transform, false);
        VictoryScreen victoryScreen = victoryScreenObj.AddComponent<VictoryScreen>();

        // Create Black Background
        GameObject bgObj = new GameObject("BlackBackground");
        bgObj.transform.SetParent(victoryScreenObj.transform, false);
        Image bgImage = bgObj.AddComponent<Image>();
        bgImage.color = new Color(0, 0, 0, 1); // Pure black
        
        // Stretch to full screen
        RectTransform bgRect = bgObj.GetComponent<RectTransform>();
        bgRect.anchorMin = new Vector2(0, 0);
        bgRect.anchorMax = new Vector2(1, 1);
        bgRect.sizeDelta = Vector2.zero;
        bgRect.anchoredPosition = Vector2.zero;

        // Create Victory Text
        GameObject textObj = new GameObject("VictoryText");
        textObj.transform.SetParent(victoryScreenObj.transform, false);
        Text victoryText = textObj.AddComponent<Text>();
        victoryText.text = "Вы собрали все яблоки. Теперь вы довольны?";
        victoryText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        victoryText.fontSize = 40;
        victoryText.color = Color.red;
        victoryText.alignment = TextAnchor.MiddleCenter;
        victoryText.horizontalOverflow = HorizontalWrapMode.Wrap;
        
        // Center the text
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0.5f, 0.5f);
        textRect.anchorMax = new Vector2(0.5f, 0.5f);
        textRect.sizeDelta = new Vector2(800, 200);
        textRect.anchoredPosition = Vector2.zero;

        // Link components to VictoryScreen
        victoryScreen.blackBackground = bgImage;
        victoryScreen.victoryText = victoryText;

        // Find and update ScoreManager
        ScoreManager scoreManager = FindAnyObjectByType<ScoreManager>();
        if (scoreManager != null)
        {
            SerializedObject so = new SerializedObject(scoreManager);
            so.FindProperty("victoryScreen").objectReferenceValue = victoryScreen;
            so.FindProperty("totalApples").intValue = 20;
            so.ApplyModifiedProperties();
            Debug.Log("Victory Screen linked to ScoreManager");
        }
        else
        {
            Debug.LogWarning("ScoreManager not found in scene. Please link VictoryScreen manually.");
        }

        // Mark the scene as dirty so Unity knows to save changes
        EditorUtility.SetDirty(victoryScreenObj);
        if (scoreManager != null)
        {
            EditorUtility.SetDirty(scoreManager);
        }

        Debug.Log("Victory Screen setup complete!");
        
        // Select the victory screen object
        Selection.activeGameObject = victoryScreenObj;
    }
}
#endif
