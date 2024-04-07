document.addEventListener('DOMContentLoaded', function () {
    // Select all elements with the data-url attribute
    var titles = document.querySelectorAll('h6[data-url]');
    titles.forEach(function (title) {
        // Check if the event listener is already attached
        if (!title.dataset.listener) {
            // Set a flag to indicate that the event listener is attached
            title.dataset.listener = true;

            // Add click event listener
            title.addEventListener('click', function (event) {
                var relativeUrl = title.dataset.url;
                var baseUrl = 'https://localhost:7208';
                var proxyUrl = baseUrl + '/api/Proxy' + relativeUrl;
                debugger;
                fetch(proxyUrl)
                    .then(response => response.text())
                    .then(data => {
                        // Display the article content in a new page
                        showArticleInNewPage(data, relativeUrl);
                    })
                    .catch(error => {
                        console.error('Error fetching post content:', error);
                    });
            });
        }
    });
});

// Function to display the article content in a new page
function showArticleInNewPage(content, relativeUrl) {
    // Open a new browser window
    var newWindow = window.open();
    // Write the "Read More" button and the article content to the new window
    newWindow.document.write('<p>' + content + '</p>');
    newWindow.document.close();
}
