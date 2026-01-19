# Empire 15 - Quick Setup Guide

This guide will help you set up a basic playable scene in Unity.

## Step 1: Create the Terrain/Ground

1. Create a plane: `GameObject → 3D Object → Plane`
2. Scale it to 10x, 1, 10 to create a large ground
3. Position at (0, 0, 0)
4. Set layer to "Ground" (create if doesn't exist)
5. Name it "Ground"

## Step 2: Create Capture Zones

For each capture zone (create at least 3):

1. Create a cylinder: `GameObject → 3D Object → Cylinder`
2. Scale to (10, 0.5, 10) for a flat capture area
3. Position at different locations (e.g., (-20, 0.5, 0), (0, 0.5, 20), (20, 0.5, -10))
4. Add component: `CaptureZone` script
5. Set zone name (Zone A, Zone B, Zone C)
6. Set capture radius to 5
7. Set capture time to 5 seconds
8. Assign the cylinder's MeshRenderer to the "Zone Renderer" field
9. Create a material and assign it to the cylinder

## Step 3: Create the Player

1. Create a capsule: `GameObject → 3D Object → Capsule`
2. Position at (0, 1, 0)
3. Tag as "Player"
4. Add component: `Character Controller`
   - Set radius to 0.5
   - Set height to 2
   - Set center to (0, 1, 0)
5. Add component: `TPSCharacterController` script
6. Add component: `PlayerRoleManager` script

## Step 4: Setup the Camera

1. Select Main Camera
2. Add component: `TPSCameraController` script
3. Drag the Player capsule into the "Target" field
4. Set default distance to 3.5
5. Set collision layers to include everything except Player

## Step 5: Create Spawn Points

1. Create empty GameObjects for spawn points
2. Position them around the map (at least 2-3 locations)
3. Name them "SpawnPoint1", "SpawnPoint2", etc.

## Step 6: Create the SpawnManager

1. Create empty GameObject: `GameObject → Create Empty`
2. Name it "SpawnManager"
3. Add component: `SpawnManager` script
4. Drag all spawn point transforms into the "Spawn Points" array
5. Create a prefab of the Player capsule (drag to Assets folder)
6. Assign the player prefab to "Player Prefab" field

## Step 7: Create the GameLoopManager

1. Create empty GameObject: `GameObject → Create Empty`
2. Name it "GameLoopManager"
3. Add component: `GameLoopManager` script
4. Set cycle duration to 60 seconds
5. Drag all CaptureZone cylinders into the "Capture Zones" array

## Step 8: Create the HUD

1. Create UI Canvas: `GameObject → UI → Canvas`
2. Set Canvas Scaler to "Scale with Screen Size" (reference resolution: 1920x1080)
3. Create UI Text for Role (top-left):
   - Position: (-850, 500)
   - Anchor: top-left
   - Font size: 24
4. Create UI Text for Timer (top-right):
   - Position: (850, 500)
   - Anchor: top-right
   - Font size: 28
5. Create UI Text for Zone Status (top-center):
   - Position: (0, 450)
   - Anchor: top-center
   - Font size: 20
6. Create UI Text for Cycle (bottom-right):
   - Position: (850, -500)
   - Anchor: bottom-right
   - Font size: 20
7. Add `MinimalHUD` script to Canvas
8. Assign all text elements to their respective fields

## Step 9: Configure Layers and Physics

1. Go to `Edit → Project Settings → Tags and Layers`
2. Add layer "Ground" if not exists
3. Set Ground plane to "Ground" layer
4. Go to `Edit → Project Settings → Physics`
5. Ensure proper collision matrix (Player should collide with Ground)

## Step 10: Materials for Leader Drawing

1. Create a material: `Assets → Create → Material`
2. Name it "LeaderPathMaterial"
3. Set shader to "Sprites/Default" or "Unlit/Color"
4. Set color to cyan (0, 255, 255, 255)
5. Select Player GameObject
6. In LeaderDrawingSystem component, assign this material

## Step 11: Test the Scene

1. Click Play
2. Test movement (WASD)
3. Test camera (Mouse)
4. Move to a capture zone and verify it changes color
5. Press R to switch to Leader role
6. Right-click and drag to draw paths
7. Press C to clear paths

## Common Issues

**Player falls through ground:**
- Make sure Character Controller is properly configured
- Check Ground layer collision settings

**Camera doesn't follow:**
- Verify Target is assigned in TPSCameraController
- Check camera isn't a child of player

**Zones don't capture:**
- Verify Player is tagged as "Player"
- Check capture radius is large enough
- Ensure GameLoopManager has zones assigned

**HUD doesn't update:**
- Verify all text references are assigned
- Check GameLoopManager is in the scene

**Drawing doesn't work:**
- Verify ground has proper layer
- Check layer is set in LeaderDrawingSystem
- Make sure Player has PlayerRoleManager component
- Press R to switch to Leader role

## Optimization Tips

- Use object pooling for frequently spawned objects
- Bake lighting for better performance
- Use occlusion culling for large maps
- Profile with Unity Profiler to identify bottlenecks

## Next Steps

- Add more capture zones for variety
- Adjust capture times and cycle duration for game balance
- Customize colors and materials for visual polish
- Add spawn protection or safe zones
- Implement team-based gameplay
