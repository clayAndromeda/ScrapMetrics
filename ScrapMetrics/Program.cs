using System.Text.Json;
using ConsoleAppFramework;
using ScrapMetrics;

ConsoleApp.Run(args, ReadBackupFile);

static void ReadBackupFile(string path)
{
	// pathのファイルを読み込む
	var fileText = File.ReadAllText(path);
	
	var scrapboxData = JsonSerializer.Deserialize<ScrapboxFormat>(fileText);
	
	// デシリアライズされたデータの使用例
	Console.WriteLine($"Name: {scrapboxData.Name}");
	Console.WriteLine($"DisplayName: {scrapboxData.DisplayName}");
	Console.WriteLine($"Exported: {scrapboxData.Exported}");
	Console.WriteLine($"Number of Users: {scrapboxData.Users.Count}");
	Console.WriteLine($"Number of Pages: {scrapboxData.Pages.Count}");
}
