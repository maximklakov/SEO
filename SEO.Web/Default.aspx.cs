using SEO.Service.Interfaces;
using SEO.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SEO.Web
{
	public partial class Default : Page
	{
		private const string SortDirectionKeyPrefix = "SortDirection_";

		private readonly ITextAnalysisService _textAnalysisService;
		private readonly IUrlAnalysisService _urlAnalysisService;

		private enum CountType
		{
			WordsCount,
			KeywordsCount
		}

		public Default(ITextAnalysisService textAnalysisService, IUrlAnalysisService urlAnalysisService) =>
			(_textAnalysisService, _urlAnalysisService) = (textAnalysisService, urlAnalysisService);

		protected void Submit_Click(object sender, EventArgs e)
		{
			if (!IsValidDataInputs())
				return;

			if (SelectedTab.Value == "text")
				AnalyzeText();
			else
				AnalyzeURL();

			InputSection.Visible = false;
			ResultsSection.Visible = true;
		}

		protected void WordsCount_Sorting(object sender, GridViewSortEventArgs e)
		{
			HandleSorting(CountType.WordsCount, (GridView)sender, e.SortExpression);
		}

		protected void KeywordsCount_Sorting(object sender, GridViewSortEventArgs e)
		{
			HandleSorting(CountType.KeywordsCount, (GridView)sender, e.SortExpression);
		}

		private void HandleSorting(CountType ct, GridView gv, string sortExpression)
		{
			string countType = Enum.GetName(typeof(CountType), ct);
			string sortDirectionKey = GetSortDirectionKey(countType);

			var wordsCountData = (IEnumerable<WordsCount>)ViewState[countType];

			var sortProp = typeof(WordsCount).GetProperty(sortExpression);
			object sortPropValue(WordsCount wordCount) => sortProp.GetValue(wordCount);

			if ((SortDirection)ViewState[sortDirectionKey] == SortDirection.Ascending)
			{
				wordsCountData = wordsCountData.OrderByDescending(sortPropValue);
				ViewState[sortDirectionKey] = SortDirection.Descending;
			}
			else
			{
				wordsCountData = wordsCountData.OrderBy(sortPropValue);
				ViewState[sortDirectionKey] = SortDirection.Ascending;
			}

			gv.DataSource = wordsCountData;
			gv.DataBind();
		}

		private bool IsValidDataInputs()
		{
			if (CalcOptions.SelectedIndex == -1)
				return false;

			if (SelectedTab.Value == "text")
				return !string.IsNullOrWhiteSpace(InputText.Text);

			return !string.IsNullOrWhiteSpace(InputUrl.Text);
		}

		private void AnalyzeText()
		{
			var wordsCountData = _textAnalysisService.GetWordsCountData(InputText.Text, FilterStopWords.Checked);

			var selectedCalculationOptions = GetSelectedCalculationOptions();

			Task.WaitAll(
				Task.Run(() =>
				{
					if (selectedCalculationOptions.Contains("words"))
						ShowWordsCount(wordsCountData);
				}),
				Task.Run(() =>
				{
					if (selectedCalculationOptions.Contains("links"))
					{
						ShowLinksCount(_textAnalysisService.GetTextLinksCount(wordsCountData));
					}
				})
			);
		}

		private void AnalyzeURL()
		{
			var htmlContent = _urlAnalysisService.GetHtmlContentFromURL(InputUrl.Text);

			var wordsCountData = _urlAnalysisService.GetWordsCountDataFromHtmlContent(htmlContent, FilterStopWords.Checked);

			var selectedCalculationOptions = GetSelectedCalculationOptions();

			Task.WaitAll(
				Task.Run(() =>
				{
					if (selectedCalculationOptions.Contains("words"))
						ShowWordsCount(wordsCountData);
				}),
				Task.Run(() =>
				{
					if (selectedCalculationOptions.Contains("links"))
					{
						var linksCount = Task.Run(() => _urlAnalysisService.GetTextLinksCount(wordsCountData));
						var anchorLinksCount = Task.Run(() => _urlAnalysisService.GetAnchorLinksCountFromHtmlContent(htmlContent));

						ShowLinksCount(linksCount.Result + anchorLinksCount.Result);
					}
				}),
				Task.Run(() =>
				{
					if (selectedCalculationOptions.Contains("keywords"))
					{
						var keywordsCountData =
							_urlAnalysisService.GetKeywordsCountDataFromHtmlContent(htmlContent, FilterStopWords.Checked);
						
						ShowKeywordsCount(keywordsCountData);
					}
				})
			);
		}

		private void ShowLinksCount(int linksCount)
		{
			LinksCount.Text = linksCount.ToString();
			LinksCountSection.Visible = true;
		}

		private void ShowWordsCount(IEnumerable<WordsCount> wordsCountData)
		{
			LoadWordsCountData(CountType.WordsCount, WordsCount, wordsCountData);
			WordsCountSection.Visible = true;
		}

		private void ShowKeywordsCount(IEnumerable<WordsCount> keywordsCountData)
		{
			LoadWordsCountData(CountType.KeywordsCount, KeywordsCount, keywordsCountData);
			KeywordsCountSection.Visible = true;
		}

		private void LoadWordsCountData(CountType ct, GridView gv, IEnumerable<WordsCount> wordsCountData)
		{
			string countType = Enum.GetName(typeof(CountType), ct);
			string sortDirectionKey = GetSortDirectionKey(countType);

			gv.DataSource = wordsCountData;
			gv.DataBind();

			ViewState[countType] = wordsCountData.ToArray();
			ViewState[sortDirectionKey] = SortDirection.Descending;
		}

		private string[] GetSelectedCalculationOptions() =>
			CalcOptions.Items.Cast<ListItem>()
				.Where(item => item.Selected)
				.Select(item => item.Value)
				.ToArray();

		private string GetSortDirectionKey(string countType) => $"{SortDirectionKeyPrefix}{countType}";

	}
}