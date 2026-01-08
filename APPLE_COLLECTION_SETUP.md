# Apple Collection System Setup Guide

This guide explains how to set up the apple collection system in Unity Editor.

## Overview
The apple collection system allows the player to collect floating apples that increase their score. The dialogue system has been fixed but will be disabled.

## Files Created
1. `Assets/AppleCollectible.cs` - Script for collectible apples with floating animation
2. `Assets/ScoreManager.cs` - Script to manage score display
3. `Assets/CameraFollow.cs` - Script for camera follow with flexible dead zone
4. `Assets/DialogueUI.cs` - Fixed to properly close dialogue on E key press
5. `Assets/Editor/AppleSetupEditor.cs` - Editor tool to automate setup (recommended)

## Manual Setup Instructions

### 1. Fix and Disable Dialogue System

**Already Fixed:**
- DialogueUI.cs has been updated to properly close when pressing E while dialogue is open

**To Disable:**
1. In Unity Hierarchy, find "DialogueManager" GameObject
2. Uncheck the checkbox next to its name in the Inspector to disable it
3. Find "DialogueCanvas" GameObject
4. Uncheck the checkbox next to its name to disable it

### 2. Update Apple 1

1. Select "Apple" GameObject in Hierarchy (it's already renamed to "Apple 1" in the scene file)
2. In Inspector, remove the "Interactable" component (click three dots → Remove Component)
3. In Inspector, find "Box Collider 2D" component
4. Check the "Is Trigger" checkbox
5. Click "Add Component" button
6. Search for and add "Apple Collectible" script
7. Set levitation parameters if desired (default: speed=1, height=0.3)

### 3. Create Additional Apples (Apple 2 through Apple 20)

For each apple from 2 to 20:
1. Right-click "Apple 1" in Hierarchy
2. Choose "Duplicate" (or press Ctrl+D / Cmd+D)
3. Rename the duplicate to "Apple 2" (then "Apple 3", etc.)
4. In Inspector, change the Transform Position to place at different heights:
   - X: Random between -10 and +10
   - Y: Random between 2 and 8 (different heights above ground)
   - Z: Keep at 0

**Quick positions (suggested):**
- Apple 2: (-7, 6, 0)
- Apple 3: (-2, 5, 0)
- Apple 4: (-8, 5, 0)
- Apple 5: (-9, 8, 0)
- Apple 6: (-9, 6, 0)
- Apple 7: (-7, 8, 0)
- Apple 8: (-7, 7, 0)
- Apple 9: (-3, 5, 0)
- Apple 10: (-2, 6, 0)
- Apple 11: (2, 2, 0)
- Apple 12: (-7, 4, 0)
- Apple 13: (9, 5, 0)
- Apple 14: (9, 8, 0)
- Apple 15: (3, 7, 0)
- Apple 16: (0, 4, 0)
- Apple 17: (6, 6, 0)
- Apple 18: (4, 8, 0)
- Apple 19: (-2, 8, 0)
- Apple 20: (0, 5, 0)

### 4. Create Score UI

#### A. Create Canvas (if not exists)
1. Right-click in Hierarchy → UI → Canvas
2. Name it "ScoreCanvas"
3. Canvas Scaler component should be added automatically

#### B. Create Score Counter Text
1. Right-click "ScoreCanvas" in Hierarchy → UI → Legacy → Text
2. Rename to "ScoreCounter"
3. In Inspector, set Text component properties:
   - Text: "00"
   - Font Size: 48
   - Color: White
   - Alignment: Right (horizontal) and Top (vertical)
4. In Rect Transform:
   - Anchor: Top-Right (click preset, hold Alt+Shift, click top-right)
   - Pos X: -20
   - Pos Y: -20
   - Width: 200
   - Height: 60

#### C. Create Score Manager
1. Right-click in Hierarchy → Create Empty
2. Rename to "ScoreManager"
3. In Inspector, click "Add Component"
4. Search for and add "Score Manager" script
5. Drag the "ScoreCounter" Text object into the "Score Text" field in Inspector

### 5. Configure Player for Collision Detection

The Player should already have a collider. Make sure:
1. Player has a Collider2D component
2. Player GameObject is named "Player" or has tag "Player"

### 6. Test the System

1. Click Play in Unity Editor
2. Move the player to touch apples
3. Apples should float up and down
4. When touching an apple, it should disappear and score should increase
5. Score should display in top-right corner as "00", "01", "02", etc.

## Using the Editor Tool (Recommended)

The automated setup tool is the easiest way to set everything up:
1. In Unity menu bar, go to: Tools → Setup Apple Collection System
2. This will automatically:
   - Update Apple 1 to use AppleCollectible
   - Create 19 more apples at random positions
   - Create the Score UI
   - Setup Camera Follow with dead zone
   - Disable the dialogue system
3. Click OK when the success dialog appears
4. Save the scene (Ctrl+S / Cmd+S)

## Manual Camera Follow Setup

If you want to set up camera follow manually:
1. Select "Main Camera" in Hierarchy
2. In Inspector, click "Add Component"
3. Search for and add "Camera Follow" script
4. Drag the "Player" GameObject into the "Target" field
5. Configure dead zone settings:
   - Dead Zone Width: 3 (horizontal tolerance before camera moves)
   - Dead Zone Height: 2 (vertical tolerance before camera moves)
   - Smooth Speed: 0.125 (how smoothly camera follows)
   - Offset: (0, 0, -10) (camera depth)

## Troubleshooting

**Camera doesn't follow player:**
- Verify CameraFollow script is attached to Main Camera
- Check that Target field points to Player GameObject
- Ensure camera Z position (offset) is negative (e.g., -10)

**Apples don't disappear when touched:**
- Make sure Apple colliders have "Is Trigger" checked
- Ensure Player has a Collider2D component
- Check that AppleCollectible script is attached to apples

**Score doesn't update:**
- Verify ScoreManager has reference to ScoreCounter Text
- Check Console for errors

**Apples don't float:**
- Make sure AppleCollectible script is attached
- Check levitation speed and height values are not zero

**Dialogue still works:**
- Disable DialogueManager and DialogueCanvas GameObjects
- The scripts remain in the project but are inactive
