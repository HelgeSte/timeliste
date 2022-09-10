using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using timeliste.Controllers;
using timeliste.Models;

namespace timeliste.Utilities {
	public class CSVGenerator {
		private readonly IWebHostEnvironment _hostEnv;
		private readonly ILogger<TimeController> _logger;

		private class Entry {
			public string AnsattNr { get; set; }
			public double Timer { get; set; }
		}

		public CSVGenerator(ILogger<TimeController> logger, IWebHostEnvironment hostEnv) {
			_logger = logger;
			_hostEnv = hostEnv;
		}

		public string GenerateFile(List<Time> timer, int prosjektId) {

			string fileName = GenerateFileName(timer);

			if(fileName == null) {
				_logger.LogError("Empty 'timer' list, cannot generate empty file.");
				return null;
			}

			if(!System.IO.Directory.Exists(_hostEnv.WebRootPath + "/CSV/" + "prosjekt-" + prosjektId + "/")) {
				System.IO.Directory.CreateDirectory(_hostEnv.WebRootPath + "/CSV/" + "prosjekt-" + prosjektId + "/");
			}

			string path = Path.Combine(_hostEnv.WebRootPath + "/CSV/" + "prosjekt-" + prosjektId + "/", fileName);

			List<Entry> entries = new List<Entry>();

			foreach(Time time in timer) {
				var entry = entries.FirstOrDefault(e => e.AnsattNr == time.AnsattNr);
				if(entry != null) { 
					entry.Timer += time.Timer;
				} else {
					entries.Add(new Entry { AnsattNr = time.AnsattNr, Timer = time.Timer });
				}
			}

			using(var writer = System.IO.File.CreateText(path))
			using(var csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture)) {
				csvWriter.WriteRecords(entries);
			}

			return fileName;
		}

		private string GenerateFileName(List<Time> timer) {

			if(timer.Count == 0) {
				return null;
			}

			DateTime firstDate = timer.Min(date => date.Start);
			DateTime lastDate = timer.Max(date => date.Slutt);

			string fileName = firstDate.ToShortDateString() + "-" 
				 + lastDate.ToShortDateString() + ".csv";

			return fileName;
		}
	}
}
