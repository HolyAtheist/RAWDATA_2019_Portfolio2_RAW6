﻿define(["knockout", "authservice", "messaging"], function (ko, authservice, messaging) {

    return function () {

        //LogInForm
        let loginUsername = ko.observable("Username");
        let loginPassword = ko.observable("Password");
        let loginUser = function (data) {
            let username = loginUsername();
            let password = loginPassword();
            if (username && username !== "Username" && password && password !== "Password") {
                console.log("Correct");
                const incomingUserCredentials = { Username: username, Password: password };
                authservice.getLoginUser(incomingUserCredentials, function (authenticationResponse) {
                    const token = authenticationResponse.token;
                    if (token) {
                        console.log('If token yes');
                        window.localStorage.setItem("userToken", token);
                        console.log(messaging.actions);
                        messaging.dispatch(messaging.actions.selectMenu("Home"));
                    }
                });

            } else {
                console.log("Incorrect");
            }
        };

        var newUserSignup = function () {
            console.log("I have been clicked for signup new user");
        }

        return {
            loginUsername,
            loginPassword,
            loginUser,
            newUserSignup
            
        }
    }

});