**Introduction**
The player must fight against waves of zombies, manage resources, and survive as long as possible. The game features a save/load system, supportive NPCs, a comprehensive UI, and special events such as helicopter reinforcements.

**Core Gameplay**
- Player spawn & restore:
- On fresh start → new game begins.
- If save data exists → player stats, inventory, weapon, NPCs, and survival time are restored.
- Survival loop:
- Timer tracks elapsed time.
- Auto-save every 5 minutes.
- Helicopter reinforcements arrive at minutes 6, 15, 25, 35.
- Death handling:
- On player death → save data cleared, leaderboard updated with survival time.  
**Systems Implemented**
1. Save/Load System
- Save player HP, Armor, Ammo, Grenades, Weapon, NPC count, Survival time, Scene index.
- Restore all these values correctly into PlayerStats, Inventory, PlayerController, NPCManager, SurvivalTimer.
- Auto-save triggered by survival milestones.
- Clear save while preserving leaderboard and audio settings.
2. SurvivalTimer
- Tracks elapsed time in seconds.
- Fires events for survival time updates and milestones.
- Auto-save milestone every 300 seconds (5 minutes).
- Helicopter events at 6, 15, 25, 35 minutes.
- Records survival time to leaderboard when player dies.
3. NPC System
NPCHelper:
- Detects zombies in range.
- Rotates continuously toward target.
- Fires bullets with damage, muzzle flash, audio, impact effects.
- Reloads after 30 shots with delay.
4. Event System
- GameEventManager:
- Centralized event hub for UI, Player, Weapon, Gameplay, Enemy, NPC.
- Ensures decoupled communication between systems.
- Used for updating UI, triggering helicopter events, handling player death, etc
5. Animation System
- Separated animation layers for player and enemy:
- Player: shooting, reloading, grenade throwing.
- Enemy: Attack, death animations.
- Layer separation ensures independent blending and smoother gameplay visuals.
6. Enemy AI
- Implemented using Behavior Tree (BTree):
- Patrol, chase, attack states.
- Decision-making based on player proximity and visibility.
- Modular AI design for scalability.
7. Object Pool System
- Centralized ObjectPoolManager for spawning and reusing objects:
- impacts, grenade prefabs, popup UI.
- Improves performance by avoiding frequent instantiation/destruction.
8. Shader Graph Effects
- Custom Shader Graph for enemy hit and death effects:
- Visual feedback when enemies take damage.
- Stylized dissolve or impact shaders when enemies die.
9. Joystick & Control System
- Virtual joystick implemented for player movement.
- Events handled via GameEventManager (OnMoveJoystick, OnMoveRelease).
- Button events for shooting, reloading, and grenade throwing.
- Smooth top‑down control using Cinemachine camera follow & Shake Camera.
10. UI System: HP/Armor bars, ammo/grenade counters, weapon icon, popups.
11.Weapon System
Gun Shooting:
- Uses CapsuleCast to detect enemies hit by bullets.
- On explosion, uses SphereOverlap to detect all enemies within blast radius.
- Provides accurate hit detection along the bullet path, including body/head colliders.  
