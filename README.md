# Snake_game

A classic game remade in the manners of OOP

## How to Play

- Press F2 to reset the game
- Press keys "WASD" to control the pink snake
- Press the four direction keys to control the blue snake

## Files Info

### Structure

- "./Snake_game/MainWindow.xaml" deals with the GUI
- "./Snake_game/MainWindow.xaml.vb" is the code behind the main window

- "./Snake_game/GameManager.vb" is in the first layer of the class structure

- "./Snake_game/Map.vb" is parallelled with "GameManager"

- "./Snake_game/Bonus.vb", "./Snake_game/Snake.vb", and "./Snake_game/Wall.vb" are all inherited from "./Snake_game/Mapping.vb" and are under the second layer

### Function

- "./Snake_game/MainWindow.xaml" deals with the GUI
- "./Snake_game/MainWindow.xaml.vb" is the code behind the main window

- "./Snake_game/GameManager.vb" controls the game in general and communication between the three class in the second layer

- "./Snake_game/Mapping.vb" controls the graphics and display

- "./Snake_game/Map.vb" maintains a global dynamic in-game map

## Appreciation

- [YasinIbrahim's Snake Game](https://github.com/YasinIbrahim/Snake-Game-WPF) - a great reference for this project
- and authors whose original code is at the website whose links were written between my codes.

## TODO

- [ ] fix graphical seams issue
- [ ] add menu
