﻿using HtmlAgilityPack;
using SEO.Service.Interfaces;
using SEO.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace SEO.Service.Services
{
	public class UrlAnalysisService : TextAnalysisService, IUrlAnalysisService
	{
		public string GetHtmlContentFromURL(string url)
		{
			if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri)
				|| !new[] { Uri.UriSchemeHttp, Uri.UriSchemeHttps }.Contains(uri.Scheme))
				return "";

			string result = "";

			try { result = new HttpClient().GetStringAsync(url).Result; }
			catch (Exception) { }

			return result;
		}

		public IEnumerable<WordCount> GetWordsCountDataFromHtmlContent(string htmlContent, bool isFilterOutStopWords)
		{
			var htmlDoc = GetHtmlDocument(htmlContent);

			htmlDoc.DocumentNode.Descendants()
				.Where(n => n.Name.Equals("script", StringComparison.OrdinalIgnoreCase)
						|| n.Name.Equals("style", StringComparison.OrdinalIgnoreCase)
						|| n.Name.Equals("#comment", StringComparison.OrdinalIgnoreCase))
				.ToList()
				.ForEach(n => n.Remove());

			string textContent = "";
			htmlDoc.DocumentNode.SelectNodes("//text()").ToList()
				.ForEach(n => textContent += n.InnerText + " ");

			return GetWordsCountData(textContent, isFilterOutStopWords);
		}

		public IEnumerable<WordCount> GetKeywordsCountDataFromHtmlContent(string htmlContent, bool isFilterOutStopWords)
		{
			var htmlDoc = GetHtmlDocument(htmlContent);

			var keywordsList = htmlDoc.DocumentNode.Descendants()
				.Where(n => n.Name.Equals("meta", StringComparison.OrdinalIgnoreCase)
						&& n.GetAttributeValue("name", "").Equals("keywords", StringComparison.OrdinalIgnoreCase))
				.Select(n => n.GetAttributeValue("content", ""));

			string keywords = string.Join(" ", keywordsList).Replace(",", " ");

			return GetWordsCountData(keywords, isFilterOutStopWords);
		}

		public int GetAnchorLinksCountFromHtmlContent(string htmlContent)
		{
			var htmlDoc = GetHtmlDocument(htmlContent);

			return htmlDoc.DocumentNode.Descendants("a")
				.Where(a => IsContainingURL(a.GetAttributeValue("href", "")))
				.Count();
		}

		private HtmlDocument GetHtmlDocument(string htmlContent)
		{
			var htmlDoc = new HtmlDocument();
			htmlDoc.LoadHtml(htmlContent);

			return htmlDoc;
		}

	}
}
