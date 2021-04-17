using SEO.Service.Models;
using System.Collections.Generic;

namespace SEO.Service.Interfaces
{
	public interface IUrlAnalysisService
	{
		string GetHtmlContentFromURL(string url);
		IEnumerable<WordCount> GetWordsCountDataFromHtmlContent(string htmlContent, bool isFilterOutStopWords);
		IEnumerable<WordCount> GetKeywordsCountDataFromHtmlContent(string htmlContent, bool isFilterOutStopWords);
		int GetAnchorLinksCountFromHtmlContent(string htmlContent);
		int GetTextLinksCount(IEnumerable<WordCount> wordsCountData);
	}
}
