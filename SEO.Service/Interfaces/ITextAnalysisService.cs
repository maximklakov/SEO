using SEO.Service.Models;
using System.Collections.Generic;

namespace SEO.Service.Interfaces
{
	public interface ITextAnalysisService
	{
		IEnumerable<WordsCount> GetWordsCountData(string text, bool isFilterOutStopWords);
		int GetTextLinksCount(IEnumerable<WordsCount> wordsCount);
	}
}
