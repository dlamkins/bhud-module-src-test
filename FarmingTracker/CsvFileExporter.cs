using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FarmingTracker
{
	public class CsvFileExporter
	{
		public string ModuleFolderPath { get; }

		public CsvFileExporter(string moduleFolderPath)
		{
			ModuleFolderPath = moduleFolderPath;
		}

		public async Task ExportSummaryAsCsvFile(Model model)
		{
			try
			{
				string csvFileText = CreateCsvFileText(model);
				string csvFileName = $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss_fff}.csv";
				string csvFolderPath = Path.Combine(ModuleFolderPath, "csv");
				await FileSaver.WriteFileAsync(Path.Combine(csvFolderPath, csvFileName), csvFileText);
				Process.Start("explorer.exe", csvFolderPath);
			}
			catch (Exception exception)
			{
				Module.Logger.Error(exception, "Failed to save csv file. :(");
			}
		}

		private static string CreateCsvFileText(Model model)
		{
			StatsSnapshot statsSnapshot = model.Stats.StatsSnapshot;
			List<Stat> items = statsSnapshot.ItemById.Values.Where((Stat s) => s.Signed_Count != 0).ToList();
			List<Stat> currencies = statsSnapshot.CurrencyById.Values.Where((Stat s) => s.Signed_Count != 0).ToList();
			int linesCount = Math.Max(items.Count, currencies.Count);
			string csvFileText = "item_id,item_name,item_amount,currency_id,currency_amount\n";
			for (int i = 0; i < linesCount; i++)
			{
				csvFileText += ((i < items.Count) ? $"{items[i].ApiId},{EscapeCsvField(items[i].Details.Name)},{items[i].Signed_Count}," : ",,,");
				csvFileText += ((i < currencies.Count) ? $"{currencies[i].ApiId},{currencies[i].Signed_Count}\n" : ",\n");
			}
			return csvFileText;
		}

		private static string EscapeCsvField(string field)
		{
			string csvEscapedField = field.Replace("\"", "\"\"");
			return "\"" + csvEscapedField + "\"";
		}
	}
}
