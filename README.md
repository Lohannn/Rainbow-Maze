# Rainbow Maze

> *An UNDERTALE inspired game where every puzzle has a solution.* 

## About the Project
**Rainbow Maze** is a puzzle game developed with a strong inspiration from the classic *UNDERTALE*. It combines nostalgic gameplay with challenging, yet always solvable, grid-based puzzles. 

## Key Mechanics & Features
*  **Swipe Input:** Input for mobile gameplay.
*  **Grid-Based Movement:** Precise, tile-by-tile navigation.
*  **Smart Puzzle Generation:** A dynamic random level generator that guarantees every maze is 100% winnable.

## Rules of the Maze
Navigate carefully! Each color has a specific effect on your movement:
* **Pink Tiles:** Do absolutely nothing. You are safe here.
* **Red Tiles:** Impassable. They act as solid walls.
* **Orange Tiles:** Step on these and your scent changes to *orange*.
* **Yellow Tiles:** They will shock you and force you back to the previous tile.
* **Purple Tiles:** They force you to slide to the next tile (if possible) and change your scent to *lemon*.
* **Blue Tiles:** Water! If there is an adjacent Yellow tile, or if you currently have the *orange* scent, they will force you back. Otherwise, they do nothing.

## Learning Journey
During development, I specifically learned and implemented:
* **Custom Grid System:** Built from scratch to handle the game's logic and movement perfectly.
* **BFS (Breadth-First Search) Pathfinding:** Used algorithmically alongside the random generator to validate paths and ensure no impossible puzzles are ever created.

## 🛠️ Built With
* **[Unity](https://unity.com/)** - Game Engine
* **C#** - Programming Language

##
<p align="center">
  <img src="https://github.com/user-attachments/assets/e0d6a419-d0cc-4afa-954a-7a6910dc7acc" alt="Rainbow Maze Gameplay Preview">
</p>
