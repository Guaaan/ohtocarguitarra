

```
==============================
🎮 Settings System for Unity 🎮
==============================

Version: 1.0  
Unity version: 6.0 or higher  
Target platforms: PC & macOS

This package provides a **modular, persistent settings system** for Unity, including:  
✅ Audio settings (music, SFX, ambience, effects)  
✅ Graphics settings (resolution, fullscreen, UI scale)  
✅ Game settings (default level, language, mouse sensitivity)  
✅ Scene loader helper  
✅ Prefab with a ready-to-use settings menu UI

Persistence: settings are saved with PlayerPrefs and persist between sessions and scenes.

==============================
📁 Folder Structure
==============================

```

/Scripts/
Settings/
SettingsManager.cs        - Singleton manager that loads/saves all settings
AudioSettings.cs          - ScriptableObject for audio volumes
GraphicsSettings.cs       - ScriptableObject for graphics options
GameSettings.cs           - ScriptableObject for general game options
UI/
SettingsMenuUI.cs         - Script that connects the UI prefab to the settings
Utils/
SceneLoader.cs            - Static helper for loading scenes and quitting the game

/Resources/
Settings/
DefaultAudioSettings.asset     - Default AudioSettings ScriptableObject
DefaultGraphicsSettings.asset  - Default GraphicsSettings ScriptableObject
DefaultGameSettings.asset      - Default GameSettings ScriptableObject

/Prefabs/
SettingsMenu.prefab         - Ready-to-use settings menu UI

```

==============================
🚀 How to Use
==============================

1️⃣ Import this package into your Unity project (`Assets > Import Package > Custom Package…`)  

2️⃣ In your scene:
- Drag the prefab `/Prefabs/SettingsMenu.prefab` into your Canvas.
- Add an empty GameObject called `SettingsManager` to your scene.
- Attach the script `SettingsManager.cs` to it.
- Assign the three ScriptableObjects from `/Resources/Settings/` to the `SettingsManager` fields in the Inspector.

✅ Make sure `SettingsManager` is present in every scene (or mark it as DontDestroyOnLoad — already handled by the script).

3️⃣ On the SettingsMenu prefab:
- Make sure all UI references (sliders, dropdowns, toggles, buttons) are correctly assigned in the Inspector to the `SettingsMenuUI` component.
- Optionally: assign the feedback `SavedMessage` text to display when settings are applied.

4️⃣ Add your scenes to Build Settings (`File > Build Settings…`) so that the default level selector can list them.

5️⃣ Run the scene and test:
- Change settings in the menu
- Click “Apply”
- Settings persist when changing scenes and between play sessions.

==============================
🔷 Additional Notes
==============================

🎛️ Audio settings: you need to implement your own AudioMixer to apply the volume values in runtime.  
🖼️ UI Scale: applies to CanvasScaler — you can script this yourself if needed.  
🌐 Language: returns the selected language code (e.g. `"en"`, `"es"`) for your localization system.  
🎮 Default level: index of the scene as per Build Settings.

==============================
📞 Support & Contribution
==============================

Feel free to modify and extend this system to fit your project’s needs!  
If you find bugs or want to contribute improvements, let the maintainer know.

Enjoy! 🎮✨
```