using SEO.Service.Interfaces;
using SEO.Service.Models;
using StopWord;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SEO.Service.Services
{
	public class AnalysisService : ITextAnalysisService, IURLAnalysisService
	{
		public IEnumerable<WordsCount> GetWordsCount(string text, bool isFilterOutStopWords)
		{
			string filteredText = text;

			if (isFilterOutStopWords)
				filteredText = text.RemoveStopWords("en");

			return filteredText
				.Replace(Environment.NewLine, " ")
				.Split(' ')
				.GroupBy(word => word)
				.Select(wordGroup => new WordsCount
				{
					Word = wordGroup.Key,
					Count = wordGroup.Count()
				})
				.Where(item => !string.IsNullOrWhiteSpace(item.Word));
		}

		public int GetLinksCount(IEnumerable<WordsCount> wordsCount)
		{
			return wordsCount.Where(item => IsValidURL(item.Word)).Sum(item => item.Count);
		}

		private bool IsValidURL(string url)
		{
			return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
				&& (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
		}
	}
}
