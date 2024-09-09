using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FarmingTracker
{
	public class CsvFileExporter
	{
		public string ModuleFolderPath { get; }

		public CsvFileExporter(string moduleFolderPath)
		{
			ModuleFolderPath = moduleFolderPath;
		}

		public async void ExportSummaryAsCsvFile(Model model)
		{
			try
			{
				string csvFileText = CreateCsvFileText(model);
				string csvFileName = $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss_fff}.csv";
				await FileSaver.WriteFileAsync(Path.Combine(ModuleFolderPath, csvFileName), csvFileText);
			}
			catch (Exception exception)
			{
				Module.Logger.Error(exception, "Failed to save csv file. :(");
			}
		}

		private static string CreateCsvFileText(Model model)
		{
			string csvFileText = "item_id,item_name,item_amount,currency_id,currency_amount\n";
			StatsSnapshot statsSnapshot = model.StatsSnapshot;
			List<Stat> items = statsSnapshot.ItemById.Values.Where((Stat s) => s.Count != 0).ToList();
			List<Stat> currencies = statsSnapshot.CurrencyById.Values.Where((Stat s) => s.Count != 0).ToList();
			int linesCount = Math.Max(items.Count, currencies.Count);
			for (int i = 0; i < linesCount; i++)
			{
				csvFileText += ((i < items.Count) ? $"{items[i].ApiId},{EscapeCsvField(items[i].Details.Name)},{items[i].Count}," : ",,,");
				csvFileText += ((i < currencies.Count) ? $"{currencies[i].ApiId},{currencies[i].Count}\n" : ",\n");
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
