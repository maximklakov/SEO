using SEO.Service.Models;
using System.Collections.Generic;

namespace SEO.Service.Interfaces
{
	public interface ITextAnalysisService
	{
		IEnumerable<WordsCount> GetWordsCount(string text, bool isFilterOutStopWords);
		int GetLinksCount(IEnumerable<WordsCount> wordsCount);
	}
}
