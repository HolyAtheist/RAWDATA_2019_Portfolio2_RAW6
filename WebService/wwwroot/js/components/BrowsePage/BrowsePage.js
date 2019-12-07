define(['knockout', 'browseService'], function (ko, bs) {

    return function () {

        let questionlist = ko.observableArray([]);
        let p = 1; //initial page
        let ps = 5; //initial pagesize

        let nexturi = '666'; //placeholder for grabbing querystring page=
        let prevuri = '666'; //placeholder for grabbing querystring page=

        let pgsizepreset = ko.observableArray(['5', '10', '20', '30', '40', '50']) //selection of pagesizes
        let loaded = ko.observable(false); //todo: supposed to help with hiding elements until initial data has been loaded 
        let getpgsize = ko.observable(); //for getting new pagesize

        //grab data when pagesize change
        let pgsizechanged = function setPgSize(context) {
            console.log("getpgsiz: ", context.getpgsize());
            if (context.getpgsize()) {
                ps = context.getpgsize();
                p = 1;
                bs.getBrowseItems(p, ps, function (data) {
                    console.log("Data from api call search : ", data);
                    if (data) {
                        questionlist(data);
                        nexturi = data.next;
                        prevuri = data.prev;
                    }
                })
            };
        };

        //grab data when page change
        function getPg(direction) {
            let npg = null;
            if (direction == 'next') {
                npg = getParameterByName('page', nexturi);
            } else if (direction == 'prev') { npg = getParameterByName('page', prevuri); }

            console.log("dat: ", direction);
            console.log("param: ", npg);
            if (npg) {
                bs.getBrowseItems(npg, ps, function (data) {
                    console.log("Data from api call search : ", data);
                    if (data) {
                        questionlist(data);
                        nexturi = data.next;
                        prevuri = data.prev;
                    }
                })
            };
        };

        //return named querystring value
        function getParameterByName(name, url) {
            if (!url) url = window.location.href;
            name = name.replace(/[\[\]]/g, '\\$&');
            var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, ' '));
        }

        //grab data for initial view
        bs.getBrowseItems(p, ps, function (data) {
            console.log("Data from api call search : ", data);

            if (data) {
                loaded(true);
                questionlist(data);
                nexturi = data.next;
                prevuri = data.prev;

            }
        });
        
        //stuff available for binding
        return {
            questionlist,
            loaded,
            getPg,
            pgsizepreset,
            getpgsize,
            pgsizechanged

        };
    };

});

