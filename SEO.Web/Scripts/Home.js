const TAB_CSS = 'c-tab';
const TAB_ACTIVE_CSS = 'active';
const TAB_ID_FORMAT = 'tab-{0}';
const TAB_CONTENT_ID_FORMAT = 'tab-{0}-content';

const getSelectedTab = () => document.getElementById('SelectedTab').value;
const getCheckboxList = () => document.getElementById('CalcOptions');

const onClickTab = (selectedTab) => {
	document.getElementById('SelectedTab').value = selectedTab;

	handleTabsStyle(selectedTab);
	prepareCalculationOptions();

	document.getElementById('submit-container').hidden = false;
}

const handleTabsStyle = (selectedTab) => {
	const tabList = [...document.getElementsByClassName(TAB_CSS)];
	tabList.forEach(tab => {
		const tabContent = document.getElementById(`${tab.id}-content`);

		if (tab.id === TAB_ID_FORMAT.replace('{0}', selectedTab)) {
			tab.classList.add(TAB_ACTIVE_CSS);
			tabContent.hidden = false;
			return;
		}

		tab.classList.remove(TAB_ACTIVE_CSS);
		tabContent.hidden = true;
	});

	setErrorMessage('');
}

const prepareCalculationOptions = () => {
	const keywordsCheckbox = [...getCheckboxList().getElementsByTagName('input')]
		.find(input => input.type === 'checkbox' && input.value === 'keywords');

	const keywordsLabel = [...document.getElementsByTagName('label')].find(label => label.htmlFor === keywordsCheckbox.id);

	const isTextTabSelected = getSelectedTab() === 'text';

	keywordsCheckbox.hidden = keywordsLabel.hidden = isTextTabSelected;
}


const onClickSubmit = () => {
	if (!isValidInput() || !isCalcOptionSelected())
		return false;

	setErrorMessage('');
	return true;
}

const isValidInput = () => {
	const selectedTab = getSelectedTab();
	const capitalizedSelectedTab = selectedTab.charAt(0).toUpperCase() + selectedTab.slice(1);
	const input = document.getElementById(`Input${capitalizedSelectedTab}`);

	if (!input.value) {
		setErrorMessage('Please fill in the input.');
		return false;
	}

	if (selectedTab === 'url' && !isValidURL(input.value)) {
		setErrorMessage('Please fill in with valid URL.');
		return false;
	}

	return true;
}

const URL_REGEX = /http(s)?:\/\/[\w][\w.-]*(?:\.[\w\.-]+)+[\w\-\._~:\/?#[\]@!\$&'\(\)\*\+,;=.]+.*$/;
const isValidURL = (text) => URL_REGEX.test(text);

const isCalcOptionSelected = () => {
	const checkboxes = [...getCheckboxList().getElementsByTagName('input')];

	if (checkboxes.every(checkbox => !checkbox.checked)) {
		setErrorMessage('Please select at least 1 calculation option.');
		return false;
	}

	return true;
}

const setErrorMessage = (errorMessage) => {
	const errorLabel = document.getElementById('ErrorMessage');
	errorLabel.textContent = errorMessage;
}