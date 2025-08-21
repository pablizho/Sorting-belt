# Sorting Belt

This repository contains the source code for a simple 2D sorting game built with Unity.  
Players must flick various pieces of rubbish into the correct bin before the items fall off the conveyor belt.  
The goal is to score as many points as possible within a 60‑second round.

## How it works

- Items move along a conveyor belt from left to right.  
- When an item enters the interaction window (roughly the centre of the screen), the player performs a short swipe to throw the item into one of three bins.  
- The three bins represent **Paper**, **Plastic/Metal** and **Organic** waste.  
- Correctly sorting items increases the score and combo multiplier, while mistakes decrease the score and reset the combo.

## Project structure

```
Sorting-belt/
├── Assets/
│   └── Scripts/       # C# scripts that implement game logic
├── .gitignore         # Standard Unity ignore rules
├── .gitattributes     # Git line‑ending configuration
└── README.md          # This file
```

The Unity scene and prefabs are not included in this repository.  
You will need to create a new Unity project and import the scripts under `Assets/Scripts/`.  
Set up a simple scene with a conveyor belt, an interaction window, three bins and a game manager as described below.

## Getting started

1. Create a new **2D** Unity project.  
2. Copy the contents of this repository into the root of your Unity project (including the `Assets` folder).  
3. In the Unity editor, create the following objects and assign the corresponding scripts:
   - **GameManager** – empty GameObject with the `GameManager` script attached.  
     Assign references to item prefabs, UI text fields, spawn point, etc.  
   - **InteractionWindow** – rectangular trigger collider that defines the area where swipes are accepted.  
     Attach the `InteractionWindow` script.
   - **PlayerInput** – empty GameObject with the `PlayerInput` script attached.  
     Assign the `InteractionWindow` and `GameManager` references.  
   - **BinLeft**, **BinCenter**, **BinRight** – each with a collider and (optional) mesh or sprite.  
     No script is required on the bins, but you can add a simple script if you wish to display feedback.
   - **ItemPrefabs** – create three prefabs (e.g. paper, plastic, organic) with the `TrashItem` script attached.  
     Set the `itemType` field accordingly and adjust the sprite and collider to match your art.

4. Create UI Text objects for score, combo multiplier and timer.  Assign them to the `GameManager` in the inspector.
5. Play the scene to test the game.  Adjust spawn rates, speeds and scoring parameters on the `GameManager` to fine‑tune the experience.

## Scripts

The core game logic is implemented in several C# scripts under `Assets/Scripts/`.  These scripts are designed to be easy to understand and modify.  You can expand upon them to add features such as special items, power‑ups, difficulty settings and more.

## License

This project is provided for educational purposes.  You are free to modify and distribute the code.
