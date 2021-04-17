using SEO.Service.Interfaces;
using SEO.Service.Models;
using StopWord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SEO.Service.Services
{
	public class TextAnalysisService : ITextAnalysisService
	{
		private readonly Regex _urlRegex;

		public TextAnalysisService() =>
			_urlRegex = new Regex(
				@"http(s)?:\/\/[\w][\w.-]*(?:\.[\w\.-]+)+[\w\-\._~:\/?#[\]@!\$&'\(\)\*\+,;=.]+.*$",
				RegexOptions.Compiled | RegexOptions.IgnoreCase);

		public IEnumerable<WordCount> GetWordsCountData(string text, bool isFilterOutStopWords)
		{
			string filteredText = text.ToLower();

			if (isFilterOutStopWords)
				filteredText = filteredText.RemoveStopWords("en");

			return filteredText
				.Replace(Environment.NewLine, " ")
				.Split(' ')
				.GroupBy(word => word)
				.Select(wordGroup => new WordCount
				{
					Word = wordGroup.Key,
					Count = wordGroup.Count()
				})
				.Where(item => !string.IsNullOrWhiteSpace(item.Word))
				.OrderByDescending(result => result.Count);
		}

		public int GetTextLinksCount(IEnumerable<WordCount> wordsCountData)
		{
			return wordsCountData.Where(item => IsContainingURL(item.Word)).Sum(item => item.Count);
		}

		protected internal bool IsContainingURL(string text)
		{
			return _urlRegex.IsMatch(text);
		}
	}
}
