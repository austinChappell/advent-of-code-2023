using System.Linq;

var main = new Main();

var sum = main.Run();

Console.WriteLine($"The sum is {sum}");

public class Main
{
    private string[] GetInput()
    {
        // not sure why the current directory is where it is, but this works
        var path = Path.Combine(Directory.GetCurrentDirectory(), "../../../input.txt");

        string text = File.ReadAllText(path);

        return text.Split("\n");
    }

    private List<int> GetSymbolIndexFromLine(string? line)
    {
        if (line == null)
        {
            return new List<int>();
        }
        
        var indexes = new List<int>();

        var splitLine = line.ToCharArray().ToList();

        var i = 0;
        
        splitLine.ForEach((c) =>
        {
            try
            {
                Int32.Parse(c.ToString());
            }
            catch
            {
                if (c.ToString() != ".")
                {
                    indexes.Add(i);
                }
            }

            i++;
        });

        return indexes;
    }

    private Dictionary<int, int> NumbersWithIndex(string? line)
    {
        var dictionary = new Dictionary<int, int>();
        
        if (line == null)
        {
            return dictionary;
        }
        
        var splitLine = line.ToCharArray().ToList();

        var i = 0;
        var endIndex = 0;

        splitLine.ForEach((c) =>
        {
            if (i < endIndex)
            {
                i++;
                return;
            }
            
            try
            {
                Int32.Parse(c.ToString());
                var substring = line.Substring(i);

                var nextNonNumberIndex = substring.ToCharArray().ToList().FindIndex(a =>
                {
                    int result;
                    
                    bool success = Int32.TryParse(a.ToString(), out result);

                    return !success;
                });

                endIndex = i + nextNonNumberIndex;

                var num = nextNonNumberIndex == -1 
                    ? Int32.Parse(substring) 
                    : Int32.Parse(substring.Substring(0, nextNonNumberIndex));
                
                dictionary.Add(i, num);
            }
            catch
            {
                // ignored
            }

            i++;
        });

        return dictionary;
    }

    public int Run()
    {
        var input = GetInput();

        var validNumbers = new List<int>();
        
        for (var i = 0; i < input.Length; i++)
        {
            var line = input[i];

            var currentLineSymbolIndexes = GetSymbolIndexFromLine(line);
            var currentLineNumbers = NumbersWithIndex(line);

            var previousLine = i > 0 ? input[i - 1] : null;
            var nextLine = i < input.Length - 1 ? input[i + 1] : null;

            var previousLineSymbolIndexes = GetSymbolIndexFromLine(previousLine);
            var previousLineNumbers = NumbersWithIndex(previousLine);
            
            var nextLineSymbolIndexes = GetSymbolIndexFromLine(nextLine);
            var nextLineNumbers = NumbersWithIndex(nextLine);
            
            foreach (var currentLineNumber in currentLineNumbers)
            {
                var currentLineNumberIndex = currentLineNumber.Key;
                var currentLineNumberValue = currentLineNumber.Value;

                var currentLineNumberDigitCount = currentLineNumberValue.ToString().Length;

                var minIndex = currentLineNumberIndex - 1;
                var maxIndex = currentLineNumberIndex + currentLineNumberDigitCount;

                var validCurrentLineIndexes = new List<int> { minIndex, maxIndex };
                var validAdjacentLineIndexes = Enumerable.Range(minIndex, maxIndex + 1 - minIndex).ToList();

                var currentLineHasAdjacentSymbol =
                    validCurrentLineIndexes.Any(idx => currentLineSymbolIndexes.Contains(idx));
                var previousLineHasAdjacentSymbol =
                    validAdjacentLineIndexes.Any(idx => previousLineSymbolIndexes.Contains(idx));
                var nextLineHasAdjacentSymbol =
                    validAdjacentLineIndexes.Any(idx => nextLineSymbolIndexes.Contains(idx));

                if (currentLineHasAdjacentSymbol || previousLineHasAdjacentSymbol || nextLineHasAdjacentSymbol)
                {
                    validNumbers.Add(currentLineNumberValue);
                }
            }
        }

        return validNumbers.Sum();
    }
}