using SEO.Service.Interfaces;
using SEO.Service.Models;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;

namespace SEO.Web
{
	public partial class Default : Page
	{
		private readonly ITextAnalysisService _analysisService;

		private string[] SelectedCalculationOptions =>
			CalcOptions.Items.Cast<ListItem>()
			.Where(item => item.Selected)
			.Select(item => item.Value)
			.ToArray();

		public Default(ITextAnalysisService analysisService) => _analysisService = analysisService;

		protected void Page_Load(object sender, EventArgs e)
		{

		}

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

		protected void WordsCount_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
		{
			var results = (IEnumerable<WordsCount>)ViewState["results"];

			var sortProp = typeof(WordsCount).GetProperty(e.SortExpression);
			object sortPropValue(WordsCount result) => sortProp.GetValue(result);

			if ((SortDirection)ViewState["sortDirection"] == SortDirection.Ascending)
			{
				results = results.OrderByDescending(sortPropValue);
				ViewState["sortDirection"] = SortDirection.Descending;
			}
			else
			{
				results = results.OrderBy(sortPropValue);
				ViewState["sortDirection"] = SortDirection.Ascending;
			}

			WordsCount.DataSource = results;
			WordsCount.DataBind();
		}

		protected void LinksCount_Sorting(object sender, GridViewSortEventArgs e)
		{
			WordsCount_Sorting(sender, e);
		}

		private bool IsValidDataInputs()
		{
			if (string.IsNullOrWhiteSpace(InputText.Text) || CalcOptions.SelectedIndex == -1)
				return false;

			return true;
		}

		private void AnalyzeText()
		{
			var wordsCount = GetWordsCount(InputText.Text);

			if (SelectedCalculationOptions.Contains("words"))
				ShowWordsCount(wordsCount);

			if (SelectedCalculationOptions.Contains("links"))
				ShowLinksCount(_analysisService.GetLinksCount(wordsCount));
		}

		private void AnalyzeURL()
		{

		}

		private WordsCount[] GetWordsCount(string text)
		{
			return _analysisService.GetWordsCount(text, FilterStopWords.Checked)
				.OrderByDescending(result => result.Count)
				.ToArray();
		}

		private void ShowWordsCount(WordsCount[] wordsCount)
		{
			WordsCount.DataSource = wordsCount;
			WordsCount.DataBind();

			ViewState["results"] = wordsCount.ToArray();
			ViewState["sortDirection"] = SortDirection.Descending;

			WordsCountSection.Visible = true;
		}

		private void ShowLinksCount(int linksCount)
		{
			LinksCount.Text = linksCount.ToString();
			LinksCountSection.Visible = true;
		}
	}
}