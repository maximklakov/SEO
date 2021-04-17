using SEO.Service.Models;
using System.Collections.Generic;

namespace SEO.Service.Interfaces
{
	public interface IUrlAnalysisService
	{
		string GetHtmlContentFromURL(string url);
		IEnumerable<WordsCount> GetWordsCountDataFromHtmlContent(string htmlContent, bool isFilterOutStopWords);
		IEnumerable<WordsCount> GetKeywordsCountDataFromHtmlContent(string htmlContent, bool isFilterOutStopWords);
		int GetAnchorLinksCountFromHtmlContent(string htmlContent);
		int GetTextLinksCount(IEnumerable<WordsCount> wordsCount);
	}
}
