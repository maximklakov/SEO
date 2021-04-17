<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
	CodeBehind="Default.aspx.cs" Inherits="SEO.Web.Default" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<asp:ScriptManagerProxy ID="MyManagerProxy" runat="server">
		<Scripts>
			<asp:ScriptReference Path="~/Scripts/Home.js" />
		</Scripts>
	</asp:ScriptManagerProxy>

	<asp:HiddenField runat="server" ID="SelectedTab" ClientIDMode="Static" />

	<section runat="server" id="InputSection">
		<div class="c-tab-group c-with-spacer">
			<div class="row">
				<div id="tab-text" class="col-md-6 c-tab" onclick="onClickTab('text')">Text</div>
				<div id="tab-url" class="col-md-6 c-tab" onclick="onClickTab('url')">URL</div>
			</div>
		</div>

		<div class="c-with-spacer-half text-center">
			<asp:Label runat="server" ID="ErrorMessage" ClientIDMode="Static" ForeColor="Red" />
		</div>

		<div id="tab-text-content" class="c-tab-content c-with-spacer" hidden>
			<asp:TextBox runat="server" ID="InputText" ClientIDMode="Static" TextMode="MultiLine" />
		</div>

		<div id="tab-url-content" class="c-tab-content c-with-spacer" hidden>
			<asp:TextBox runat="server" ID="InputUrl" ClientIDMode="Static" placeholder="https://www.sitecore.com" />
		</div>

		<div id="submit-container" class="c-with-spacer" hidden>
			<div class="c-center">
				<div class="c-with-spacer">
					<b>Processing Options</b>
					<div>
						<asp:CheckBox runat="server" ID="FilterStopWords" Text="Filters out stop-words (e.g.: ‘or’, ‘and’, ‘a’, ‘the’ etc.)" />
					</div>
				</div>

				<div class="c-with-spacer">
					<b>Calculation Options (select at least 1)</b>
					<asp:CheckBoxList runat="server" ID="CalcOptions" ClientIDMode="Static">
						<asp:ListItem Value="links">Calculates number of external links</asp:ListItem>
						<asp:ListItem Value="words">Calculates word occurrences</asp:ListItem>
						<asp:ListItem Value="keywords">Calculates word occurrences in keywords meta tags</asp:ListItem>
					</asp:CheckBoxList>
				</div>

				<div class="c-center text-center">
					<asp:Button runat="server" ID="Submit" ClientIDMode="Static" Text="Submit"
						OnClick="Submit_Click" OnClientClick="return onClickSubmit();" />
				</div>
			</div>
		</div>
	</section>

	<section runat="server" id="ResultsSection" visible="false" class="text-center">

		<div runat="server" id="LinksCountSection" class="c-with-spacer" visible="false">
			<h3>Links Count</h3>
			<div class="c-border c-center">
				<asp:Label runat="server" ID="LinksCount" />
			</div>
			<hr />
		</div>

		<div runat="server" id="WordsCountSection" class="c-with-spacer" visible="false">
			<h3>Words Count</h3>
			<asp:GridView runat="server" ID="WordsCount" AutoGenerateColumns="false"
				AllowSorting="true" OnSorting="WordsCount_Sorting">
				<Columns>
					<asp:BoundField HeaderText="Word" DataField="Word" SortExpression="Word" />
					<asp:BoundField HeaderText="Count" DataField="Count" SortExpression="Count" />
				</Columns>
			</asp:GridView>
			<hr />
		</div>

		<div runat="server" id="KeywordsCountSection" class="c-with-spacer" visible="false">
			<h3>Keywords Count</h3>
			<asp:GridView runat="server" ID="KeywordsCount" AutoGenerateColumns="false" EmptyDataText="No Keyword Found"
				AllowSorting="true" OnSorting="KeywordsCount_Sorting">
				<Columns>
					<asp:BoundField HeaderText="Keyword" DataField="Word" SortExpression="Word" />
					<asp:BoundField HeaderText="Count" DataField="Count" SortExpression="Count" />
				</Columns>
			</asp:GridView>
			<hr />
		</div>

		<a href="/">Go Back To Home</a>
	</section>

</asp:Content>
