# Setup Instructions for Unity Editor

## Quick Start

The easiest way to complete the game setup is to use the automated editor tool:

### Automated Setup (Recommended)

1. **Open the project in Unity Editor** (version 6000.0.40f1 or later)
2. **Open the main scene**: `Assets/Scenes/SampleScene.unity`
3. **Run the setup tool**: Go to menu `Tools → Setup Apple Collection System`
4. **Wait for completion**: The tool will:
   - Create 19 additional apples (Apple 2-20) at various heights
   - Setup the score UI (ScoreCanvas with ScoreCounter)
   - Add CameraFollow component to Main Camera with dead zone
   - Disable the dialogue system
5. **Save the scene**: Press Ctrl+S (Cmd+S on Mac)
6. **Test the game**: Press Play and try collecting apples!

### What Gets Set Up

#### 1. Apple Collection System
- **20 apples total** (Apple 1-20) placed at different heights (Y: 2-8, X: -10 to 10)
- Each apple has:
  - `AppleCollectible` script with levitation animation
  - Trigger collider for detection
  - Red color for visibility

#### 2. Score Display
- **ScoreCanvas**: UI canvas in Screen Space Overlay mode
- **ScoreCounter**: Text element in top-right corner showing "00"
- **ScoreManager**: GameObject managing score updates
- Connected automatically - score increases when collecting apples

#### 3. Camera Follow System
- **CameraFollow** component added to Main Camera
- Features:
  - **Dead Zone**: 3x2 area where camera doesn't move
  - **Smooth Following**: Gradual camera movement (smoothSpeed: 0.125)
  - **Target**: Automatically set to Player
  - **Offset**: Z: -10 for proper 2D depth
- Player can move freely within the dead zone before camera follows

#### 4. Dialogue System
- DialogueManager and DialogueCanvas are **disabled** (but not deleted)
- Can be re-enabled later if needed

## Manual Setup (Alternative)

If you prefer to set things up manually, see `APPLE_COLLECTION_SETUP.md` for detailed step-by-step instructions.

## Verification Checklist

After running the setup tool, verify:

- [ ] 20 apples visible in the scene (Apple 1 through Apple 20)
- [ ] Score counter "00" visible in top-right corner
- [ ] Main Camera has CameraFollow component with Player as target
- [ ] DialogueManager and DialogueCanvas are inactive (unchecked in hierarchy)
- [ ] All apples have AppleCollectible script attached
- [ ] All apples have BoxCollider2D with "Is Trigger" enabled

## Testing

1. **Press Play** in Unity Editor
2. **Move the player** using arrow keys or WASD
3. **Jump** using Space bar
4. **Collect apples** by touching them - they should disappear and score should increase
5. **Observe camera** - it should follow player smoothly with a dead zone
6. **Check levitation** - apples should float up and down

## Troubleshooting

### Apples not collecting
- Ensure Player GameObject has tag "Player"
- Check that apples have trigger colliders
- Verify AppleCollectible script is attached

### Score not updating
- Check ScoreManager has reference to ScoreCounter Text
- Look for errors in Console window

### Camera not following
- Verify CameraFollow script is on Main Camera
- Check that target is set to Player transform
- Ensure camera Z offset is -10

### Dialogue still active
- Manually disable DialogueManager and DialogueCanvas in hierarchy
- Check that both GameObjects exist in scene

## What's Next?

With the setup complete, you can:
- Adjust apple positions in the Transform component
- Tune camera dead zone size in CameraFollow component
- Modify levitation speed/height in AppleCollectible components
- Customize score display appearance
- Add more game mechanics!

## Files Reference

- `Assets/AppleCollectible.cs` - Apple collection and levitation logic
- `Assets/ScoreManager.cs` - Score tracking and display
- `Assets/CameraFollow.cs` - Camera follow with dead zone
- `Assets/PlayerController2D.cs` - Player movement and jumping
- `Assets/Editor/AppleSetupEditor.cs` - Automated setup tool

For more details, see:
- `APPLE_COLLECTION_SETUP.md` - Detailed setup instructions
- `FEATURES.md` - Complete feature documentation (Russian)
- `IMPLEMENTATION_SUMMARY.md` - Implementation details
