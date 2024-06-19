using System.Text.Json;
using ConsoleAppFramework;
using ScottPlot;
using ScottPlot.TickGenerators;
using ScrapMetrics;

ConsoleApp.Run(args, Commands.ReadBackupFile);

static class Commands
{
	/// <summary>
	/// Read Scrapbox backup file and output monthly page count
	/// </summary>
	/// <param name="input">input json path</param>
	/// <param name="output">-o, output png file path</param>
	/// <param name="startYear">開始年</param>
	/// <param name="endYear">終了年</param>
	public static void ReadBackupFile(
		[Argument] string input,
		string output,
		int? startYear = null,
		int? endYear = null)
	{
		// pathのファイルを読み込む
		var fileText = File.ReadAllText(input);

		var scrapboxData = JsonSerializer.Deserialize<ScrapboxFormat>(fileText);

		// scrapboxData.Pages.CreatedをUnix時間からDateTimeに変換して、月ごとに集計する
		var monthlyPageGroups = scrapboxData.Pages
			.Select(x => (p: x, DateTimeOffset.FromUnixTimeSeconds(x.Created).DateTime))
			.GroupBy(x => (x.Item2.Year, x.Item2.Month))
			.Where(x =>
			{
				// まず開始年でフィルタリング
				if (startYear == null) return true; // 開始年フィルタなし
				return x.Key.Year >= startYear.Value;
			})
			.Where(x =>
			{
				// 終了年でフィルタリング
				if (endYear == null) return true; // 終了年フィルタなし
				return x.Key.Year <= endYear.Value;
			})
			.ToArray();

		foreach (var monthlyPageGroup in monthlyPageGroups)
		{
			Console.WriteLine(
				$"{monthlyPageGroup.Key.Year}-{monthlyPageGroup.Key.Month}: {monthlyPageGroup.Count()} pages");
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
}

