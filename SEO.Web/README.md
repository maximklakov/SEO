### What is considered as links for the *Links Count*
- Starts with `http://` or `https://`
- Even if the URL has prefix and/or suffix around it. e.g., `"http://site.com"` or `>>http://site.com<<`
- URL is either written in the text literally or it is the href of anchor tag `<a>`.

### What is considered as words for the *Words Count*
- All characters including aplhanumeric, punctuation, quotes, etc.
- Words are separated by space.
    e.g., `"one,two, three four, five"` will be counted as **4 words**
- If Stop-Words option is on, it will filter these words out based on those stop-words.

### What is considered as keywords for the *Keywords Count*
- Words that is stated in the content of `<meta name="keywords">` tags.
- Keywords are separated by comma `,` and also by space.
    e.g., `"one,two, three four, five"` will be counted as **5 keywords**
- Keywords is affected by Stop-Words option as well.

### Third party packages used
- HtmlAgilityPack
    Search and find content from HTML document.

- dotnet-stop-words
    List of stop-words.

- Microsoft.AspNet.WebFormsDependencyInjection.Unity
    Dependency injection.

- Velyo.AspNet.Markdown
    Render documentation markdown file.