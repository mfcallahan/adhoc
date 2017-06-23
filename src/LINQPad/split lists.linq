<Query Kind="Program" />

void Main()
{
	List<string> listOrig = new List<string>();
	listOrig.Add("a");
	listOrig.Add("b");
	listOrig.Add("c");
	listOrig.Add("d");
	listOrig.Add("e");
	listOrig.Add("f");
	listOrig.Add("g");
	listOrig.Add("h");
	listOrig.Add("i");
	
	List<List<string>> listSplit = SplitAdrList(listOrig, 8);
	
	foreach(var l in listSplit)
	{
		Console.WriteLine(l);
		
	}
	
}

// Define other methods and classes here

public static List<List<string>> SplitAdrList(List<string> source, int num)
{
	return source
		.Select((x, i) => new { Index = i, Value = x })
		.GroupBy(x => x.Index / num)
		.Select(x => x.Select(v => v.Value).ToList())
		.ToList();
}