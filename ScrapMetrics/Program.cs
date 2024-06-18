using System.Text.Json;
using ConsoleAppFramework;
using ScottPlot;
using ScottPlot.TickGenerators;
using ScrapMetrics;

ConsoleApp.Run(args, ReadBackupFile);

static void ReadBackupFile(string path, string output)
{
	// pathのファイルを読み込む
	var fileText = File.ReadAllText(path);
	
	var scrapboxData = JsonSerializer.Deserialize<ScrapboxFormat>(fileText);
	
	// scrapboxData.Pages.CreatedをUnix時間からDateTimeに変換して、月ごとに集計する
	var monthlyPageGroups = scrapboxData.Pages
		.Select(x => (p: x, DateTimeOffset.FromUnixTimeSeconds(x.Created).DateTime))
		.GroupBy(x => (x.Item2.Year, x.Item2.Month))
		.Where(x => x.Key.Year >= 2023)
		.ToArray();
	
	foreach (var monthlyPageGroup in monthlyPageGroups)
	{
		Console.WriteLine($"{monthlyPageGroup.Key.Year}-{monthlyPageGroup.Key.Month}: {monthlyPageGroup.Count()} pages");
	}

	ScottPlot.Plot plot = new();
	double[] pageCounts = monthlyPageGroups.Select(x => (double)x.Count()).ToArray();
	var barPlot = plot.Add.Bars(pageCounts);
	foreach (var bar in barPlot.Bars)
	{
		bar.Label = ((int)bar.Value).ToString();
	}

	Tick[] ticks = monthlyPageGroups
		.Select(x => $"{x.Key.Year}-{x.Key.Month}")
		.Select((label, i) => new Tick(i, label))
		.ToArray();
	plot.Axes.Bottom.TickGenerator = new NumericManual(ticks);
	
	plot.SavePng(output, width: 1400, height: 400);
}
