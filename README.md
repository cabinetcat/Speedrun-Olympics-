# speedrunolympicslib
## Usage
```c#
SpeedrunComClient client = new SpeedrunComClient(); //make a new instance of the SpeedrunCom client          
Game game = client.Games.SearchGame("Sheppard Software Geography"); //initialize game
Player player = game.Runs.ElementAt(0).Player; //make a new player
GameHandle GHandle = new GameHandle(game, Gametype.levels); //new instance of the speedrunolympicslib GameHandle
GHandle.PointsReference.CreateReference(3, 2, 1, .5f); //always create a points reference before declaring a PlayerhHandle
PlayerHandle PHandle = new PlayerHandle(player,GHandle); //new instance of the speedrunolympicslib PlayerHandle
PHandle.UpdatePoints(game.Runs.ElementAt(0)); //update the points of the player 
Console.Writeline("{0} - {1}",PHandle.firstplaces(),PHandle.Name); //display the amount of first places they have.
```
	
