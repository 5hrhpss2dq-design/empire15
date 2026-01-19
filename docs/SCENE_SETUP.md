# Empire 15 - PrototypeScene Setup Instructions

This document provides step-by-step instructions for setting up the playable prototype scene in Unity.

## Prerequisites
- Unity 2021.3 LTS or newer installed
- Empire 15 project opened in Unity Editor

## Scene Structure Overview

```
PrototypeScene
├── Environment
│   └── Ground (Plane)
├── Zones
│   ├── CaptureZone_A
│   ├── CaptureZone_B
│   └── CaptureZone_C
├── GameManagers
│   ├── WarCycleManager
│   ├── SpawnManager
│   └── PlayFabAuthManager
├── Player
│   ├── CharacterController
│   ├── ThirdPersonController
│   ├── PlayerRoleManager
│   └── LeaderTools (child)
│       ├── LeaderDrawing
│       └── LeaderCommandHandler
├── MainCamera
│   └── CameraController
└── UI
    └── Canvas
        └── HUDManager
```

## Step-by-Step Setup

### 1. Create New Scene
1. File → New Scene
2. Save as `Assets/Scenes/PrototypeScene.unity`

### 2. Setup Ground
1. GameObject → 3D Object → Plane
2. Rename to "Ground"
3. Transform:
   - Position: (0, 0, 0)
   - Rotation: (0, 0, 0)
   - Scale: (10, 1, 10)
4. Add Layer "Ground" (Edit → Project Settings → Tags and Layers)
5. Set Ground's Layer to "Ground"

### 3. Create Capture Zones

**Zone A:**
1. GameObject → 3D Object → Cylinder
2. Rename to "CaptureZone_A"
3. Transform:
   - Position: (-20, 0.5, 0)
   - Rotation: (0, 0, 0)
   - Scale: (10, 0.5, 10)
4. Add Component → CaptureZone (Empire15.GameLoop)
5. Configure CaptureZone:
   - Zone Name: "Zone A"
   - Capture Radius: 5
   - Capture Time: 5
   - Assign Cylinder's MeshRenderer to "Zone Renderer"
6. Assign material: ZoneNeutral.mat

**Zone B:**
- Repeat above with Position: (0, 0.5, 20), Name: "Zone B"

**Zone C:**
- Repeat above with Position: (20, 0.5, -10), Name: "Zone C"

### 4. Create Player

1. GameObject → 3D Object → Capsule
2. Rename to "Player"
3. Transform:
   - Position: (0, 1, 0)
   - Rotation: (0, 0, 0)
   - Scale: (1, 1, 1)
4. Tag as "Player" (Inspector → Tag dropdown)
5. Add Component → Character Controller
   - Radius: 0.5
   - Height: 2
   - Center: (0, 1, 0)
6. Add Component → ThirdPersonController (Empire15.Movement)
7. Add Component → PlayerRoleManager (Empire15.Strategy)

**Create LeaderTools child:**
1. Right-click Player → Create Empty
2. Rename to "LeaderTools"
3. Add Component → LeaderDrawing (Empire15.Strategy)
   - Assign LeaderPath.mat to "Line Material"
   - Set "Ground Layer" to Ground layer
4. Add Component → LeaderCommandHandler (Empire15.Strategy)

### 5. Setup Camera

1. Select Main Camera
2. Transform:
   - Position: (0, 5, -5)
   - Rotation: (30, 0, 0)
3. Add Component → CameraController (Empire15.Camera)
4. Configure:
   - Target: Drag Player GameObject here
   - Default Distance: 3.5
   - Collision Layers: Everything except Player

### 6. Create Spawn Points

1. GameObject → Create Empty, rename to "SpawnPoint1"
   - Position: (0, 1, 0)
2. GameObject → Create Empty, rename to "SpawnPoint2"
   - Position: (-15, 1, -15)
3. GameObject → Create Empty, rename to "SpawnPoint3"
   - Position: (15, 1, 15)

### 7. Create Game Managers

1. GameObject → Create Empty, rename to "GameManagers"
2. Position: (0, 0, 0)

**Add WarCycleManager:**
1. With GameManagers selected, Add Component → WarCycleManager
2. Configure:
   - Cycle Duration: 60 (or shorter for testing, e.g., 30)
   - Capture Zones: Drag all 3 CaptureZone objects into array

**Add SpawnManager:**
1. Add Component → SpawnManager
2. Configure:
   - Spawn Points: Drag all 3 SpawnPoint objects into array
   - Player Prefab: Drag Player GameObject to create prefab, then assign

**Add PlayFabAuthManager:**
1. Add Component → PlayFabAuthManager
2. Note: This is a placeholder - TitleId remains "PLACEHOLDER_TITLEID"

### 8. Create HUD

1. GameObject → UI → Canvas
2. Canvas settings:
   - Render Mode: Screen Space - Overlay
   - Canvas Scaler: Scale with Screen Size
   - Reference Resolution: 1920 x 1080

**Add HUDManager script:**
1. Select Canvas
2. Add Component → HUDManager (Empire15.UI)

**Create Role Text:**
1. Right-click Canvas → UI → Text
2. Rename to "RoleText"
3. Rect Transform:
   - Anchor: Top-Left
   - Position: (-850, 500, 0)
   - Width: 300, Height: 50
4. Text settings:
   - Font Size: 24
   - Color: White
   - Alignment: Left, Top

**Create Timer Text:**
1. Right-click Canvas → UI → Text
2. Rename to "TimerText"
3. Rect Transform:
   - Anchor: Top-Right
   - Position: (850, 500, 0)
   - Width: 200, Height: 50
4. Text settings:
   - Font Size: 28
   - Color: White
   - Alignment: Right, Top

**Create Zone Status Text:**
1. Right-click Canvas → UI → Text
2. Rename to "ZoneStatusText"
3. Rect Transform:
   - Anchor: Top-Center
   - Position: (0, 450, 0)
   - Width: 400, Height: 80
4. Text settings:
   - Font Size: 20
   - Color: White
   - Alignment: Center, Top

**Create Cycle Number Text:**
1. Right-click Canvas → UI → Text
2. Rename to "CycleNumberText"
3. Rect Transform:
   - Anchor: Bottom-Right
   - Position: (850, -500, 0)
   - Width: 200, Height: 40
4. Text settings:
   - Font Size: 20
   - Color: White
   - Alignment: Right, Bottom

**Wire up HUDManager:**
1. Select Canvas
2. In HUDManager component:
   - Role Name Text: Drag RoleText
   - Cycle Timer Text: Drag TimerText
   - Zone Status Text: Drag ZoneStatusText
   - Cycle Number Text: Drag CycleNumberText

### 9. Configure Physics

1. Edit → Project Settings → Physics
2. Ensure collision matrix allows:
   - Player collides with Ground
   - Player collides with Default

### 10. Create Player Prefab

1. Drag Player GameObject from Hierarchy to Assets/Prefabs/
2. This creates Player.prefab
3. Assign this prefab to SpawnManager's "Player Prefab" field
4. Delete Player from scene (SpawnManager will spawn it)

### 11. Final Scene Configuration

1. **Lighting**: Add Directional Light if not present
2. **Skybox**: Keep default or assign custom
3. **Scene View**: Frame all objects to verify placement
4. **Save**: Ctrl+S (Cmd+S on Mac)

## Testing the Scene

1. **Press Play** in Unity Editor
2. **Verify**:
   - Player spawns at random spawn point
   - Camera follows player
   - WASD movement works
   - Mouse camera control works
   - HUD displays information
   - One zone is active (different color)

3. **Test Zone Capture**:
   - Move to active zone
   - Watch color change from gray to yellow
   - Stay in zone until it turns green
   - Check HUD for progress update

4. **Test Leader Drawing**:
   - Press R to switch to LEADER
   - Hold Right Mouse Button and drag
   - Watch line appear
   - Release near a zone
   - Zone should turn green immediately

5. **Test Cycle**:
   - Wait for timer to reach 00:00
   - All zones should reset
   - Cycle number should increment

## Troubleshooting

**Player falls through ground:**
- Ensure Ground has Mesh Collider or Box Collider
- Check CharacterController settings

**Camera doesn't follow:**
- Verify Target is assigned in CameraController
- Check camera isn't child of Player

**Zones don't capture:**
- Verify Player has "Player" tag
- Check CaptureZone radius is large enough
- Ensure WarCycleManager has zones assigned

**HUD doesn't update:**
- Verify all Text references are assigned in HUDManager
- Check Canvas is in scene

**Drawing doesn't work:**
- Make sure Ground layer is set correctly
- Verify Ground layer is assigned in LeaderDrawing
- Press R to switch to LEADER role

## Next Steps

After setup:
1. Test all features per QA_CHECKLIST.md
2. Adjust values for gameplay feel (speeds, timers, etc.)
3. Add additional zones as needed
4. Customize colors and materials
5. Add spawn protection zones if desired

## Notes

- This setup creates a single-player prototype
- Multiplayer requires Photon PUN integration (not in scope)
- PlayFab is placeholder only
- Save scene frequently during setup

---

**Setup Time**: ~30-45 minutes
**Difficulty**: Intermediate
**Last Updated**: 2026-01-19
