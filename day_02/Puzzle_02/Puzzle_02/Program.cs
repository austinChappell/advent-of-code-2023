// See https://aka.ms/new-console-template for more information

var sum = Main.Run(12, 13, 14);

Console.WriteLine($"The sum is {sum}");

class Main
{
    public static int Run(int redCubeCount, int greenCubeCount, int blueCubeCount)
    {
        var powers = new List<int>();
        
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
            var minRed = 0;
            var minGreen = 0;
            var minBlue = 0;

            game.Value.ForEach(hand =>
            {
                hand.ForEach(set =>
                {
                    foreach (var keyValuePair in set)
                    {
                        var color = keyValuePair.Key;
                        var count = keyValuePair.Value;

                        if (color == "red" && count > minRed)
                        {
                            minRed = count;
                        }

                        if (color == "green" && count > minGreen)
                        {
                            minGreen = count;
                        }

                        if (color == "blue" && count > minBlue)
                        {
                            minBlue = count;
                        }
                    }
                });
            });
            
            powers.Add(minRed * minGreen * minBlue);
        }

        return powers.Sum();
    }
}