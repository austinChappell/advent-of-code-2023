// See https://aka.ms/new-console-template for more information

var sum = Main.Run(12, 13, 14);

Console.WriteLine($"The sum is {sum}");

class Main
{
    public static int Run(int redCubeCount, int greenCubeCount, int blueCubeCount)
    {
        var possibleGameIds = new List<int>();
        
        // not sure why the current directory is where it is, but this works
        var path = Path.Combine(Directory.GetCurrentDirectory(), "../../../input.txt");

        string text = File.ReadAllText(path);
        Console.WriteLine(text);

        var games = text.Split("\n");

        var gamesWithIdsAndHands = new Dictionary<int, List<List<Dictionary<string, int>>>>();
        
        foreach (var game in games)
        {
            var splitGame = game.Split(": ");
            var gameId = Int32.Parse(splitGame[0].Split("Game ")[1]);
            var hands = splitGame[1];
            var splitHands = hands.Split("; ");

            var gameHands = new List<List<Dictionary<string, int>>>();
            
            Console.WriteLine(gameId);

            foreach (var hand in splitHands)
            {
                var cubeSets = hand.Split(", ");

                var sets = new List<Dictionary<string, int>>();
                
                foreach (var cubeSet in cubeSets)
                {
                    var splitCubeSet = cubeSet.Split(" ");
                    var cubeSetCount = Int32.Parse(splitCubeSet[0]);
                    var cubeSetColor = splitCubeSet[1];
                    
                    sets.Add(new Dictionary<string, int>() { {cubeSetColor, cubeSetCount} });
                }
                
                gameHands.Add(sets);
            }
            
            gamesWithIdsAndHands.Add(gameId, gameHands);
        }
        
        Console.WriteLine("hello");
        
        foreach (var game in gamesWithIdsAndHands)
        {
            var hasEnoughReds = true;
            var hasEnoughGreens = true;
            var hasEnoughBlues = true;

            game.Value.ForEach(hand =>
            {
                hand.ForEach(set =>
                {
                    foreach (var keyValuePair in set)
                    {
                        var color = keyValuePair.Key;
                        var count = keyValuePair.Value;

                        if (color == "red" && count > redCubeCount)
                        {
                            hasEnoughReds = false;
                        }

                        if (color == "green" && count > greenCubeCount)
                        {
                            hasEnoughGreens = false;
                        }

                        if (color == "blue" && count > blueCubeCount)
                        {
                            hasEnoughBlues = false;
                        }
                    }
                });
            });

            if (hasEnoughReds && hasEnoughGreens && hasEnoughBlues)
            {
                possibleGameIds.Add(game.Key);
            }
        }

        return possibleGameIds.Sum();
    }
}