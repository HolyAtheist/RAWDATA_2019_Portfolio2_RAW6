define(["knockout", "historyService"], function (ko, ds) {

    return function () {
        let token = window.localStorage.getItem('userToken');

        let maxPages = ko.observable(10);
        let totalPages = ko.observable();
        let prevUrl = ko.observable();
        let nextUrl = ko.observable();
        let items = ko.observableArray();
        console.log("maxpage value is: " + maxPages());  //todo remove

        let getData = function (url) {
            ds.getHistory(token, url, function (response) {
                totalPages(response.numberOfPages);
                prevUrl(response.prev);
                nextUrl(response.next);
                items(response.items);
            });
        };

        let page = 1;
        let url = ds.buildUrl(page, maxPages());
        getData(url);

        let navPage = function (url) {
            if (url != null) {
                getData(url);
            }
        };

        let deletions = function () {
            ds.deleteHistory("goat", function (response) {
                return response;
            })
        };

        return {
            maxPages,
            items,
            navPage,
            nextUrl,
            prevUrl,
            deletions
        };

    };
});
