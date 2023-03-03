# Vege Emporium - match 3 game for PC
My first finished project without using any assets. I wanted to make my game from scratch, draw every sprite, button or background. Also I tried to test myself and haven't used many tutorials (probably only the save system is based on some idea from youtube). Music is made by my friend because I'm terrible at it.\
<img src="https://user-images.githubusercontent.com/43621858/222664295-11c4ec70-4c03-4495-b1b5-b590c1fe76d3.png" width="50%" height="50%">
### Programming
My greatest success in this project is that it works. It was really challenging to code logic behind this simple game, be sure to find all possible moves, patterns on board after move, remove them correctly and refill board with new vegetables. There are a lot of edge case scenarios and possibilities to make a mistake. But I even managed to make my levels based on .txt generator so everyone, without any knowledge about coding can prepare a new level for this game.
https://github.com/cichy30002/Match3Game/blob/33af8ccc35af5ccfde1a5c6c3744b65cf2b6c70c/Match3Game/Assets/Resources/Levels/mission1.txt#L1-L4
But there is a dark side of this project - the code got really messy and it's hard to work with. It is a side effect of complicated logic that hides behind this game and I think at that time I wasn't able to avoid it. To understand what I mean it's best to take a look on [Level Manager Script](https://github.com/cichy30002/Match3Game/blob/main/Match3Game/Assets/Scripts/LevelManager.cs). It works, it's quite good in terms of computational complexity but it is a real spaghetti.
### Game
You can check out my game on [itch.io](https://cichy30002.itch.io/vege-emporium). There are 10 levels and I won't continue development.
<img src="https://user-images.githubusercontent.com/43621858/222665181-e0ebc203-0e2d-4e07-ae19-ec7ca3a288c6.png" width="50%" height="50%">
### Conclusion
My conclusions from this project were clean code, clean code and clean code. Honestly I wanted to improve everything in this area. I was really proud that I was able to do a game all by myself and wanted to continue on this path. You can check how it went on [my next project repo](https://github.com/cichy30002/MobileGame).
