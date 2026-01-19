# Empire 15

**A tactical third-person shooter prototype with leader-driven strategy mechanics**

---

## 🎮 How the Prototype Works

Empire 15 is a military strategy game prototype built in Unity that combines smooth PUBG-like TPS mechanics with a unique leader-soldier command system.

### Core Game Loop

The game follows a simple, repeatable cycle:

```
Spawn → Move → Capture Zone → Zone Changes Color → Timer Updates → Repeat
```

1. **Spawn**: Players spawn at designated points at the start of each cycle
2. **Move**: Players use PUBG-like smooth movement controls to navigate the battlefield
3. **Capture Zone**: Players enter active zones to capture them over time
4. **Zone Changes**: Captured zones change color (Gray → Yellow → Green)
5. **Timer Updates**: Each cycle has a fixed duration; when time expires, a new cycle begins
6. **Repeat**: The cycle continues with zones resetting and new objectives activating

---

## 🎯 Game Systems

### Layer 1: TPS Movement & Camera

**PUBG-Authentic Movement System:**
- **Run (Default)**: WASD movement at 4.7 m/s - standard PUBG movement speed
- **Sprint**: Hold Left Shift + W for 6.3 m/s - fastest movement
- **Walk**: Hold Left Alt + WASD for 1.7 m/s - slow, tactical movement
- **Crouch Run**: Hold Left Ctrl + WASD for 3.4 m/s
- **Crouch Sprint**: Hold Ctrl + Shift + W for 4.8 m/s
- **Crouch Walk**: Hold Ctrl + Alt + WASD for 1.3 m/s
- **Jump**: Spacebar
- **Camera**: Mouse to look around with smooth interpolation
- **Zoom**: Mouse scroll wheel to adjust camera distance

*All movement speeds match official PUBG values for authentic gameplay feel.*

**Scripts:**
- `ThirdPersonController.cs` - Character movement with authentic PUBG speeds
- `CameraController.cs` - Smooth third-person camera with collision detection

### Layer 2: Core Game Loop

**Capture Zone System:**
- Zones become active one at a time
- Players must stand in the capture radius to capture zones
- Capture progress shown visually through color changes
- When captured, the next zone automatically activates

**Cycle Timer:**
- Each game cycle has a set duration (default: 60 seconds)
- When time expires, all zones reset and a new cycle begins
- Cycles are numbered for tracking progress

**Scripts:**
- `SpawnManager.cs` - Handles player spawning at designated points
- `CaptureZone.cs` - Individual zone logic, capture progress, color changes
- `GameLoopManager.cs` - Orchestrates the entire game loop and cycle management

### Layer 3: Visual Clarity

**Minimal Military-Style HUD:**
- **Role Display**: Shows current role (SOLDIER or LEADER)
- **Cycle Timer**: Countdown timer with color warnings (white → yellow → red)
- **Zone Status**: Active zone name, capture progress, and status
- **Cycle Number**: Current cycle counter

**Color System:**
- Gray = Neutral/Inactive zone
- Yellow = Zone being captured
- Green = Zone captured
- White = Normal HUD state
- Yellow = Warning (< 30s remaining)
- Red = Critical (< 10s remaining)

**Scripts:**
- `MinimalHUD.cs` - Clean, readable HUD with essential information only

### Layer 4: Strategy DNA

**Leader Drawing System:**
- **Leader Role**: Can draw tactical paths on the battlefield (Right Mouse Button + Drag)
- **Soldier Role**: Can see paths drawn by leaders
- **Tactical Paths**: Drawn lines affect zone objectives and provide strategic guidance
- **Clear Paths**: Press C to clear all drawn paths
- **Role Toggle**: Press R to switch between Leader and Soldier roles (for testing)

**How It Works:**
1. Leader right-clicks and drags to draw a path on the ground
2. The path appears as a colored line visible to all players
3. Zones near the drawn path are identified and can be influenced by the strategy
4. This creates a visual command system where leaders guide soldiers

**Scripts:**
- `LeaderDrawingSystem.cs` - Path drawing, visualization, and gameplay connection
- `PlayerRoleManager.cs` - Role assignment and system enablement

### Layer 5: Stability & Control

**Project Organization:**
```
Assets/
├── Scripts/           # All C# gameplay scripts
├── Scenes/           # Unity scene files
├── Prefabs/          # Reusable game objects
└── Materials/        # Visual materials and shaders

ProjectSettings/      # Unity project configuration
Packages/             # Unity package dependencies
```

**Key Features:**
- Clean folder structure following Unity best practices
- Well-commented code with XML documentation
- Singleton pattern for managers (GameLoopManager, SpawnManager)
- Clear separation of concerns (Movement, Camera, Game Loop, UI, Strategy)
- Proper use of namespaces (Empire15.Movement, Empire15.GameLoop, etc.)

---

## 🚀 Getting Started

### Prerequisites
- Unity 2021.3 LTS or newer
- Basic understanding of Unity Editor

### Setup
1. Clone this repository
2. Open the project in Unity Hub
3. Open the main scene in `Assets/Scenes/`
4. Create the required game objects (see Scene Setup below)

### Scene Setup

**Required GameObjects:**

1. **Player**
   - Add `CharacterController` component
   - Add `TPSCharacterController` script
   - Add `PlayerRoleManager` script
   - Tag as "Player"

2. **Main Camera**
   - Add `TPSCameraController` script
   - Assign Player transform as target

3. **GameLoopManager** (Empty GameObject)
   - Add `GameLoopManager` script
   - Assign all CaptureZone objects

4. **SpawnManager** (Empty GameObject)
   - Add `SpawnManager` script
   - Create spawn point transforms and assign them
   - Assign player prefab

5. **CaptureZones** (Cylinder or custom mesh)
   - Add `CaptureZone` script
   - Add `MeshRenderer` for visual feedback
   - Position around the map

6. **UI Canvas**
   - Add `MinimalHUD` script
   - Create UI Text elements for role, timer, and zone status

### Controls

| Input | Action | Speed (PUBG Authentic) |
|-------|--------|------------------------|
| WASD | Move (Run) | 4.7 m/s |
| Mouse | Look around | - |
| Shift + W | Sprint | 6.3 m/s |
| Alt + WASD | Walk (Slow) | 1.7 m/s |
| Ctrl + WASD | Crouch Run | 3.4 m/s |
| Ctrl + Shift + W | Crouch Sprint | 4.8 m/s |
| Ctrl + Alt + WASD | Crouch Walk | 1.3 m/s |
| Space | Jump | - |
| Mouse Scroll | Zoom camera | - |
| Right Mouse Button + Drag | Draw path (Leader only) | - |
| C | Clear all paths | - |
| R | Toggle role (Soldier/Leader) | - |
| Esc | Toggle cursor lock | - |

---

## 🎯 Design Philosophy

This prototype focuses on **simple, strong systems** rather than feature explosion:

1. **Clear Purpose**: Every mechanic serves the core loop
2. **Readable World**: Visual feedback makes the game state obvious
3. **Minimal but Complete**: Essential features only, polished to smoothness
4. **Strategic Depth**: Leader system adds unique gameplay without complexity
5. **Professional Foundation**: Clean code and structure for future expansion

---

## 📝 Future Enhancements

Potential additions (not in current scope):
- Shooting mechanics
- AI soldiers
- More zone types
- Voice/text communication
- Multiplayer networking
- Advanced minimap
- Sound effects and music
- Multiple maps

---

## 🔧 Technical Notes

- **Physics**: Uses Unity's built-in CharacterController for movement
- **Input**: Classic Unity Input system (can be upgraded to new Input System)
- **UI**: Unity UI (uGUI) with TextMeshPro recommended for production
- **Rendering**: Compatible with Built-in, URP, and HDRP render pipelines
- **Performance**: Optimized for 60+ FPS on mid-range hardware

### PlayFab & Photon Configuration (Placeholders)

This prototype includes placeholder integration points for PlayFab and Photon networking. **No credentials are committed to the repository.**

**To configure PlayFab:**
1. Create a PlayFab account at [playfab.com](https://playfab.com)
2. Create a new title and obtain your Title ID
3. In Unity, select GameManagers GameObject
4. In PlayFabAuthManager component, replace "PLACEHOLDER_TITLEID" with your actual Title ID
5. Install PlayFab SDK from Unity Package Manager or Asset Store

**To configure Photon:**
1. Create a Photon account at [photonengine.com](https://www.photonengine.com)
2. Create a new Photon PUN app and obtain your App ID
3. Install Photon PUN 2 from Unity Asset Store
4. In Photon settings, enter your App ID
5. Implement network synchronization for multiplayer (not in current scope)

**Important**: Never commit credentials to source control. Use:
- Environment variables for builds
- Unity's Resources or StreamingAssets with .gitignore
- Secure key management systems for production

---

## 📄 License

This prototype is provided as-is for development and demonstration purposes.

---

## 🤝 Contributing

This is a prototype project. For improvements or suggestions, please open an issue or submit a pull request.

---

**Built with Unity • Designed for Strategy • Polished for Smoothness**