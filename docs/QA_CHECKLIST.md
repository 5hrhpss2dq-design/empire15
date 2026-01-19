# Empire 15 - QA Testing Checklist

## Test Environment
- **Unity Version**: 2021.3 LTS or newer
- **Platform**: Windows/Mac/Linux Standalone
- **Scene**: PrototypeScene.unity

---

## Feature 1: TPS Movement & Camera Feel

### Movement Tests
- [ ] **WASD Movement**
  - Press W: Character moves forward relative to camera
  - Press S: Character moves backward
  - Press A: Character strafes left
  - Press D: Character strafes right
  - Movement feels smooth with no stuttering
  - Character rotates smoothly to face movement direction

- [ ] **Sprint**
  - Hold Left Shift + W: Character moves faster
  - Release Shift: Character returns to normal walk speed
  - Sprint speed is noticeably faster than walk

- [ ] **Crouch**
  - Hold Left Control: Character moves slower
  - Movement speed reduced appropriately
  - Can still move in all directions while crouched

- [ ] **Jump**
  - Press Spacebar: Character jumps
  - Jump height feels appropriate
  - Can only jump when grounded
  - Gravity applies correctly after jump

### Camera Tests
- [ ] **Mouse Look**
  - Move mouse: Camera rotates smoothly around character
  - No camera jitter or stuttering
  - Camera follows character position smoothly

- [ ] **RMB Orbit**
  - Hold Right Mouse Button: Camera orbits when moving mouse
  - Works independently of character movement
  - Smooth transition in/out of orbit mode

- [ ] **Mouse Wheel Zoom**
  - Scroll up: Camera zooms in (gets closer)
  - Scroll down: Camera zooms out (gets farther)
  - Min/max distance limits work correctly
  - Zoom is smooth and responsive

- [ ] **Camera Collision**
  - Walk character near wall: Camera doesn't clip through geometry
  - Camera smoothly moves closer when obstructed
  - Camera returns to normal distance when clear

---

## Feature 2: Capture Zone System

### Zone Activation Tests
- [ ] **Zone Identification**
  - Zones are visually distinct
  - Can identify neutral vs active zones
  - Only one zone active at a time

- [ ] **Zone Capture**
  - Enter active zone: Zone color starts changing to yellow
  - Stay in zone: Progress increases toward 100%
  - Leave zone: Progress slowly decreases (regresses)
  - Progress reaches 100%: Zone turns green (captured)

- [ ] **Zone Color States**
  - Neutral: Gray color
  - Contested (being captured): Yellow color with gradient
  - Owned (captured): Green color

### Zone Progression Tests
- [ ] **Capture Progress**
  - Progress increases at consistent rate
  - Multiple players speed up capture (if multiplayer)
  - Progress visible on HUD as percentage

- [ ] **Progress Regression**
  - Leave zone before capture: Progress decreases slowly
  - Regression rate is slower than capture rate
  - Progress stops at 0%, doesn't go negative

- [ ] **Zone Cycle**
  - Capture Zone A: Zone B becomes active
  - Capture Zone B: Zone C becomes active
  - Capture last zone: Cycle resets or continues

---

## Feature 3: HUD Display

### HUD Elements Tests
- [ ] **Role Display**
  - HUD shows "ROLE: SOLDIER" or "ROLE: LEADER"
  - Role updates when pressing R key
  - Text is clearly readable

- [ ] **Cycle Timer**
  - Timer counts down from cycle duration
  - Format is MM:SS (e.g., "01:00")
  - Timer color changes:
    - White when > 30 seconds remaining
    - Yellow when 10-30 seconds remaining
    - Red when < 10 seconds remaining

- [ ] **Zone Status**
  - Shows current active zone name
  - Shows zone status (NEUTRAL/CONTESTED/OWNED)
  - Shows capture progress percentage
  - Updates in real-time as zone changes

- [ ] **Cycle Number**
  - Displays current cycle number
  - Increments when cycle completes
  - Format: "CYCLE: X"

### HUD Updates Tests
- [ ] **Real-time Updates**
  - Timer updates every second
  - Zone progress updates smoothly
  - Status changes reflect immediately

- [ ] **Event-driven Updates**
  - Zone capture triggers HUD update
  - Cycle end/start triggers HUD update
  - Role change triggers HUD update

---

## Feature 4: Leader Drawing System

### Drawing Tests
- [ ] **Line Drawing**
  - Press R to switch to LEADER role
  - Hold Right Mouse Button + Drag: Line appears on ground
  - Line is visible (cyan color)
  - Release RMB: Line is finalized

- [ ] **Multiple Lines**
  - Can draw multiple lines
  - All lines remain visible
  - Press C: All lines are cleared

- [ ] **Line Quality**
  - Line follows mouse cursor smoothly
  - No excessive point clustering
  - Line is visible on terrain

### Gameplay Connection Tests
- [ ] **Zone Activation**
  - Draw line ending near a zone
  - Release RMB: Nearest zone becomes owned (green)
  - HUD updates to show zone ownership
  - Visual feedback appears (cyan sphere)

- [ ] **Leader vs Soldier**
  - SOLDIER role: Cannot draw lines (RMB disabled for drawing)
  - LEADER role: Can draw and activate zones
  - Switch with R key: Drawing enabled/disabled appropriately

---

## Feature 5: War Cycle Management

### Cycle Timer Tests
- [ ] **Timer Countdown**
  - Cycle starts with full time
  - Timer decrements every second
  - Timer reaches 00:00: Cycle ends

- [ ] **Cycle Events**
  - Cycle start: Logged in console
  - Cycle end: Logged in console
  - Events trigger HUD updates

### Cycle Reset Tests
- [ ] **End of Cycle**
  - Timer reaches 00:00: All zones reset to neutral
  - Cycle number increments
  - New cycle begins with fresh timer
  - First zone becomes active again

- [ ] **Zone Ownership**
  - Zone ownership tracked during cycle
  - Console logs capture events
  - Ownership resets at cycle end

---

## Feature 6: Visual Clarity

### Color System Tests
- [ ] **Zone Colors**
  - Gray = Neutral (inactive)
  - Yellow = Being captured (contested)
  - Green = Captured (owned)
  - Colors are clearly distinguishable

- [ ] **HUD Colors**
  - White = Normal state
  - Yellow = Warning (< 30s)
  - Red = Critical (< 10s)
  - Green = Success (zone captured)

### Visual Feedback Tests
- [ ] **Zone Visual Feedback**
  - Color transitions are smooth
  - No sudden color pops
  - Zones are easy to identify from distance

- [ ] **Compass/Minimap (if implemented)**
  - Compass shows orientation
  - Minimap shows player position
  - Minimap shows zone locations

---

## Feature 7: Integration Tests

### Complete Game Loop Test
- [ ] **Full Cycle Playthrough**
  1. Start scene: Cycle begins
  2. Move to Zone A: Enter and capture
  3. Move to Zone B: Enter and capture  
  4. Move to Zone C: Enter and capture
  5. Timer expires: Cycle resets
  6. New cycle begins: All zones neutral again

### Leader Command Test
- [ ] **Leader Gameplay**
  1. Press R: Switch to LEADER role
  2. Draw line ending near Zone A
  3. Verify: Zone A becomes owned immediately
  4. Draw line near Zone B
  5. Verify: Zone B becomes owned
  6. Press C: All lines cleared

### Multi-System Test
- [ ] **Combined Features**
  - Movement + Camera: Works smoothly together
  - Movement + Zone Capture: Character can enter and capture
  - HUD + Zone System: HUD reflects zone changes
  - Leader Drawing + Zones: Lines affect zone ownership
  - Timer + Zones: Cycle resets zones correctly

---

## Performance Tests

- [ ] **Frame Rate**
  - Maintains 60 FPS or higher
  - No significant frame drops during gameplay
  - Smooth performance with multiple zones

- [ ] **Memory**
  - No memory leaks over extended play
  - Memory usage remains stable

- [ ] **Input Responsiveness**
  - No input lag
  - All controls respond immediately
  - Camera and movement feel responsive

---

## Known Limitations & Issues

### Client-Trust Implementation
⚠️ **WARNING**: This is a prototype with client-side trust only
- No server-side validation of captures
- No anti-cheat implementation
- Zone ownership can be manipulated client-side
- **NOT suitable for competitive multiplayer without server authority**

### Placeholder Systems
- PlayFab authentication is placeholder only
- Photon networking not implemented
- No actual multiplayer functionality
- Single-player prototype only

### Scope Limitations
- No shooting mechanics
- No AI opponents
- No inventory system
- No persistence between sessions
- No audio/sound effects
- Limited visual effects

### Edge Cases to Note
- Camera can clip in tight spaces
- Drawing on steep terrain may have issues
- Zone detection relies on trigger colliders
- Player must be tagged "Player" for zones to detect

---

## Bug Reporting Template

When reporting bugs, please include:

```
**Bug Title**: [Brief description]

**Steps to Reproduce**:
1. 
2. 
3. 

**Expected Result**:
[What should happen]

**Actual Result**:
[What actually happens]

**Frequency**: [Always / Sometimes / Rarely]

**Unity Version**: 
**Platform**: [Windows / Mac / Linux]
**Scene**: PrototypeScene.unity

**Screenshots/Video**: [If applicable]

**Console Errors**: [Copy any error messages]
```

---

## Test Sign-Off

- **Tester Name**: _______________
- **Date**: _______________
- **Unity Version**: _______________
- **Platform**: _______________
- **Overall Assessment**: [Pass / Fail / Pass with Issues]
- **Notes**: 

---

## Regression Testing

After any code changes, re-test these critical paths:
1. Movement and camera controls
2. Zone capture progression
3. HUD updates
4. Leader drawing and zone activation
5. Cycle timer and reset

---

## Future Test Additions

As features are added, extend this checklist with:
- Shooting mechanics tests
- Multiplayer synchronization tests
- AI behavior tests
- Audio tests
- Network latency tests
- Save/load tests

---

**Last Updated**: 2026-01-19
**Document Version**: 1.0
**Status**: Initial Release
