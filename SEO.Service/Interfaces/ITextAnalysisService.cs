using SEO.Service.Models;
using System.Collections.Generic;

namespace SEO.Service.Interfaces
{
	public interface ITextAnalysisService
	{
		IEnumerable<WordCount> GetWordsCountData(string text, bool isFilterOutStopWords);
		int GetTextLinksCount(IEnumerable<WordCount> wordsCountData);
	}
}
