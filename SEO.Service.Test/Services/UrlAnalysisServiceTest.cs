using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SEO.Service.Interfaces;
using SEO.Service.Models;
using SEO.Service.Services;
using System.Linq;

namespace SEO.Service.Test.Services
{
	[TestClass]
	public class UrlAnalysisServiceTest
	{
		private UrlAnalysisService _target;
		private Mock<IHttpClientHelper> _httpClientHelperMock;

		private TestContext _testContext;
		public TestContext TestContext
		{
			get => _testContext;
			set => _testContext = value;
		}

		[TestInitialize]
		public void TestInitialize()
		{
			_httpClientHelperMock = new Mock<IHttpClientHelper>();
			_target = new UrlAnalysisService(_httpClientHelperMock.Object);
		}

		[TestMethod]
		[DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
			@"|DataDirectory|\Services\TestSupport\UrlAnalysisServiceTestData.xml",
			"GetHtmlContentFromURL_DataDriven",
			DataAccessMethod.Sequential)]
		public void GetHtmlContentFromURL_DataDriven()
		{
			string text = TestContext.DataRow["text"].ToString();
			bool.TryParse(TestContext.DataRow["isValid"].ToString(), out bool isValid);

			_httpClientHelperMock.Setup(x => x.GetStringAsync(text).Result).Returns("<div>test</div>");

			string result = _target.GetHtmlContentFromURL(text);

			Assert.AreEqual(isValid, !string.IsNullOrEmpty(result), text);
		}

		[TestMethod]
		[DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
			@"|DataDirectory|\Services\TestSupport\UrlAnalysisServiceTestData.xml",
			"GetWordsCountDataFromHtmlContent_DataDriven",
			DataAccessMethod.Sequential)]
		public void GetWordsCountDataFromHtmlContent_DataDriven()
		{
			var htmlContent = TestContext.DataRow["htmlContent"].ToString();
			bool.TryParse(TestContext.DataRow["isFilterOutStopWords"].ToString(), out bool isFilterOutStopWords);
			var expectedWordCountData = TestContext.DataRow.GetChildRows("GetWordsCountDataFromHtmlContent_DataDriven_ExpectedWordCount");

			var wordsCountData = _target.GetWordsCountDataFromHtmlContent(htmlContent, isFilterOutStopWords);

			foreach (var expectedWordCount in expectedWordCountData)
			{
				var expectedWord = expectedWordCount["word"].ToString();
				int.TryParse(expectedWordCount["count"].ToString(), out int expectedCount);

				var wordCount = wordsCountData.FirstOrDefault(res => res.Word == expectedWord);
				Assert.AreEqual(expectedCount, wordCount?.Count ?? 0, expectedWord);
			}
		}

		[TestMethod]
		[DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
			@"|DataDirectory|\Services\TestSupport\UrlAnalysisServiceTestData.xml",
			"GetKeywordsCountDataFromHtmlContent_DataDriven",
			DataAccessMethod.Sequential)]
		public void GetKeywordsCountDataFromHtmlContent_DataDriven()
		{
			var htmlContent = TestContext.DataRow["htmlContent"].ToString();
			bool.TryParse(TestContext.DataRow["isFilterOutStopWords"].ToString(), out bool isFilterOutStopWords);
			var expectedWordCountData = TestContext.DataRow.GetChildRows("GetKeywordsCountDataFromHtmlContent_DataDriven_ExpectedWordCount");

			var wordsCountData = _target.GetKeywordsCountDataFromHtmlContent(htmlContent, isFilterOutStopWords);

			foreach (var expectedWordCount in expectedWordCountData)
			{
				var expectedWord = expectedWordCount["word"].ToString();
				int.TryParse(expectedWordCount["count"].ToString(), out int expectedCount);

				var wordCount = wordsCountData.FirstOrDefault(res => res.Word == expectedWord);
				Assert.AreEqual(expectedCount, wordCount?.Count ?? 0, expectedWord);
			}
		}

		[TestMethod]
		[DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
			@"|DataDirectory|\Services\TestSupport\UrlAnalysisServiceTestData.xml",
			"GetAnchorLinksCountFromHtmlContent_DataDriven",
			DataAccessMethod.Sequential)]
		public void GetAnchorLinksCountFromHtmlContent_DataDriven()
		{
			string htmlContent = TestContext.DataRow["htmlContent"].ToString();
			int.TryParse(TestContext.DataRow["expected"].ToString(), out int expected);

			int result = _target.GetAnchorLinksCountFromHtmlContent(htmlContent);

			Assert.AreEqual(expected, result);
		}
	}
}
