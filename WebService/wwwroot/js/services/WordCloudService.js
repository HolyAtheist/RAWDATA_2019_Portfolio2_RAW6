define([], function () {

    let getWCItems = async function (s, st, max, callback) {
        let response = null;
        let toekn = window.localStorage.getItem('userToken');
        if (max == 0) {
            response = await fetch(
                buildUrl("api/search/wordrank", {
                    s: s,
                    stype: st
                }),
                {
                    method: "GET",
                    headers: {
                        Authorization: "Bearer " + toekn
                    }
                }
            );
        }
        else {
            response = await fetch(
                buildUrl("api/search/wordrank", {
                    s: s,
                    stype: st,
                    maxresults: max
                }),
                {
                    method: "GET",
                    headers: {
                        Authorization: "Bearer " + toekn
                    }
                }
            );
        }


        let data = await response;
        if (response.status != 401) //we are not unauthorized
        {
            try {
                data = await response.json();    //try to parse
            }
            catch (error) {         //json was incomplete
                let errorresponse = new Object();
                errorresponse.status = 666; //custom status code
                data = errorresponse;
            }
        } else if (response.status == 401) { //we are unauthorized!
            let errorresponse = new Object();
            errorresponse.status = response.status;  //send back status 401
            data = errorresponse;
        }
        callback(data);     //ok? then send it back
    };


    function buildUrl(url, parameters) {
        let qs = "";
        for (const key in parameters) {
            if (parameters.hasOwnProperty(key)) {
                const value = parameters[key];
                qs +=
                    encodeURIComponent(key) + "=" + encodeURIComponent(value) + "&";
            }
        }
        if (qs.length > 0) {
            qs = qs.substring(0, qs.length - 1); //chop off last "&"
            url = url + "?" + qs;
        }
        return url;
    }

    return {
        getWCItems
    }
});