using Microsoft.VisualStudio.TestTools.UnitTesting;
using SEO.Service.Interfaces;
using SEO.Service.Models;
using SEO.Service.Services;
using System.Linq;

namespace SEO.Service.Test.Services
{
	[TestClass]
	public class TextAnalysisServiceTest
	{
		private TextAnalysisService _target;

		private TestContext _testContext;
		public TestContext TestContext
		{
			get => _testContext;
			set => _testContext = value;
		}

		[TestInitialize]
		public void TestInitialize()
		{
			_target = new TextAnalysisService();
		}

		[TestMethod]
		[DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
			@"|DataDirectory|\Services\TestSupport\TextAnalysisServiceTestData.xml",
			"GetWordsCountData_DataDriven",
			DataAccessMethod.Sequential)]
		public void GetWordsCountData_DataDriven()
		{
			var text = TestContext.DataRow["text"].ToString();
			bool.TryParse(TestContext.DataRow["isFilterOutStopWords"].ToString(), out bool isFilterOutStopWords);
			var expectedList = TestContext.DataRow.GetChildRows("GetWordsCountData_DataDriven_Expected");

			var wordsCountData = _target.GetWordsCountData(text, isFilterOutStopWords);

			foreach (var expected in expectedList)
			{
				var expectedWord = expected["word"].ToString();
				int.TryParse(expected["count"].ToString(), out int expectedCount);

				var wordCount = wordsCountData.FirstOrDefault(res => res.Word == expectedWord);
				Assert.IsNotNull(wordCount, expectedWord);
				Assert.AreEqual(expectedCount, wordCount.Count, expectedWord);
			}
		}

		[TestMethod]
		[DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
			@"|DataDirectory|\Services\TestSupport\TextAnalysisServiceTestData.xml",
			"GetTextLinksCount_DataDriven",
			DataAccessMethod.Sequential)]
		public void GetTextLinksCount_DataDriven()
		{
			int.TryParse(TestContext.DataRow["expected"].ToString(), out int expected);

			var wordsCountData = TestContext.DataRow.GetChildRows("GetTextLinksCount_DataDriven_WordCount")
				.Select(dr => new WordCount() { Word = dr["word"].ToString(), Count = int.Parse(dr["count"].ToString()) });

			int textLinksCount = _target.GetTextLinksCount(wordsCountData);

			Assert.AreEqual(expected, textLinksCount);
		}

		[TestMethod]
		[DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
			@"|DataDirectory|\Services\TestSupport\TextAnalysisServiceTestData.xml",
			"IsContainingURL_DataDriven",
			DataAccessMethod.Sequential)]
		public void IsContainingURL_DataDriven()
		{
			string text = TestContext.DataRow["text"].ToString();
			bool.TryParse(TestContext.DataRow["expected"].ToString(), out bool expected);

			bool result = _target.IsContainingURL(text);

			Assert.AreEqual(expected, result, text);
		}
	}
}
