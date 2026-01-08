# Victory Screen Setup Instructions

This document explains how to set up the Victory Screen in Unity Editor for the apple collection game.

## Components Created

1. **VictoryScreen.cs** - Script that manages the victory screen display
2. **Updated ScoreManager.cs** - Now detects when all 20 apples are collected
3. **Updated PlayerController2D.cs** - Fixed physics for smooth movement

## Unity Editor Setup Steps

### 1. Create Victory UI Canvas

1. In the Hierarchy, create a new UI Canvas (if you need a separate one for victory):
   - Right-click in Hierarchy → UI → Canvas
   - Name it "VictoryCanvas"
   - Set Canvas Scaler to "Scale With Screen Size" (recommended)

2. Or use the existing ScoreCanvas

### 2. Create Black Background Panel

1. In the VictoryCanvas (or ScoreCanvas):
   - Right-click → UI → Image
   - Name it "BlackBackground"
   - In the Inspector:
     - Set the Image component's Color to pure black (R:0, G:0, B:0, A:255)
     - Set the Rect Transform to stretch full screen:
       - Anchor Presets: click the square in bottom-right (stretch both)
       - Left, Right, Top, Bottom: all set to 0
   - Make sure it's the first child so it renders behind everything

### 3. Create Victory Text

1. In the VictoryCanvas (or ScoreCanvas):
   - Right-click → UI → Text
   - Name it "VictoryText"
   - In the Inspector:
     - Text: "Вы собрали все яблоки. Теперь вы довольны?"
     - Font Size: 36-48 (or larger, depending on your screen resolution)
     - Color: Red (R:255, G:0, B:0, A:255)
     - Alignment: Center (both horizontal and vertical)
     - Best Fit: Enable this for automatic sizing (optional)
   - Position the text in the center of the screen:
     - Anchor Presets: center-center
     - Pos X: 0, Pos Y: 0
     - Width: 800, Height: 200 (adjust as needed)

### 4. Add VictoryScreen Component

1. Select the VictoryCanvas (or ScoreCanvas) in Hierarchy
2. In Inspector, click "Add Component"
3. Search for "VictoryScreen" and add it
4. In the VictoryScreen component:
   - Drag the BlackBackground Image to the "Black Background" field
   - Drag the VictoryText Text to the "Victory Text" field
   - The victory message should already be set to the Russian text

### 5. Update ScoreManager

1. Find the GameObject with the ScoreManager component (likely in ScoreCanvas)
2. In the Inspector, in the ScoreManager component:
   - Set "Total Apples" to 20 (should be default)
   - Drag the GameObject with the VictoryScreen component to the "Victory Screen" field

### 6. Verify Apple Count

Make sure you have exactly 20 Apple objects in the scene with the AppleCollectible script.

## Expected Behavior

1. **Apple Collection**: When Player touches an Apple:
   - Apple disappears
   - Score increases by 1
   - Score display updates in top-right corner

2. **Victory Condition**: When the 20th apple is collected:
   - Screen turns black (BlackBackground panel appears)
   - Red victory message appears in the center
   - Game continues running (player can still move if desired)

## Physics Improvements

The PlayerController2D has been updated with:

1. **Interpolation**: `rb.interpolation = RigidbodyInterpolation2D.Interpolate`
   - Fixes jerky movement
   - Ensures smooth rendering between physics steps

2. **Frictionless Material**: Zero-friction PhysicsMaterial2D
   - Prevents sticking/slowdown when landing on floor
   - Eliminates drag from collisions
   - Allows smooth continuous movement

## Testing Checklist

- [ ] Apple collection works (apples disappear on contact)
- [ ] Score counter increases correctly
- [ ] Victory screen appears after collecting 20th apple
- [ ] Victory screen has black background
- [ ] Victory message is in red
- [ ] Player movement is smooth (no jerks)
- [ ] No sticking when landing on floor while moving

## Troubleshooting

### Victory Screen doesn't appear
- Check that ScoreManager has VictoryScreen reference assigned
- Check that totalApples is set to 20 in ScoreManager
- Verify all 20 apples have AppleCollectible script

### Black background doesn't cover full screen
- Check BlackBackground Rect Transform settings
- Ensure anchors are set to stretch-stretch
- Verify Canvas Scaler settings

### Text is not visible
- Check text color (should be red)
- Verify text is not behind black background (check hierarchy order)
- Check font size and canvas resolution

### Player still has jerky movement
- Verify the changes were saved in PlayerController2D.cs
- Check that Rigidbody2D component exists on Player
- Make sure Time.timeScale is 1.0 (not paused)

### Player still sticks when landing
- The PhysicsMaterial2D is created at runtime
- Make sure PlayerController2D.Awake() is being called
- Check that Player has a Collider2D component
