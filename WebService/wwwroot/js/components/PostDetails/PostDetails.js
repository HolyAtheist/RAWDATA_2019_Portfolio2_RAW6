﻿define(['knockout', 'postservice', 'messaging'], function (ko, postservice ,messaging) {
    return function () {
        let postUrl = ko.observable(messaging.getState().selectedPost);
        let annotationBodyText = ko.observable("");
        let annotatedPostValues = ko.observable();

        let postDetails = ko.observable([]);
        let postAnnotationsArray = ko.observable([]);
        let showspinner = ko.observable(true);
        let showAnnotTextArea = ko.observable(false);
        let responseData = ko.observable(false);
        let newAnnotation = ko.observable({});
       
        postservice.getAllChildDataOfPostUrl(postUrl(), function (responseData) {
            if (responseData) {
                console.log("What is passed to postDetails: ", responseData[0].annotations);
                postDetails(responseData);
                console.log("What is passed to postDetails: ", postDetails()[1].annotations);
                postAnnotationsArray(responseData)
                showspinner(false);
            } 

        });

        let addAnnotation = function (value, event) {
            console.log("value ", value);
            console.log("you clicked " + event.target.id);
            console.log(showAnnotTextArea());
            showAnnotTextArea(true);
        };

        
        let addBookmark = function (value) {
            console.log(value.id, value.createBookmarkLink);
            const createBookmarkUrl = value.createBookmarkLink;
            postservice.savePostAsBookmark(createBookmarkUrl, function (responseFromServer) {
                responseData(responseFromServer);
            });
        };

        annotationBodyText.subscribe(function (annotBody) {
            console.log("Value from test: ", annotBody);
            console.log("myStinkingValue: ", annotatedPostValues());
            
            if (annotBody.length === 0 ) {
                return;
            } 
            const createAnnotObject = {
                postid: annotatedPostValues().id,
                annotBody
            };
            postservice.saveAnnotationOnPost(createAnnotObject, function (responseData) {
                if (responseData) {
                    console.log("Moni, you were a smart kookie here :D ", responseData);
                    annotationBodyText("");
                    newAnnotation(responseData)
                    console.log("what i wanna do: ", newAnnotation());
                    //TODO: delete an annotation 
                    //TODO: update an annotation
                }

            });
        });

        return {
            postUrl,
            postDetails,
            addAnnotation,
            addBookmark,
            showspinner,
            showAnnotTextArea,
            responseData,
            annotationBodyText,
            annotatedPostValues,
            newAnnotation
        };
    };
});