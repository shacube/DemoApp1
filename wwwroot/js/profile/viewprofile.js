// Enter view profile code.
(function(){
    window.DemoApp1 = window.DemoApp1 || {};
    window.DemoApp1.ProfileInfo = window.DemoApp1.ProfileInfo || {};
    var profileInfoObj = window.DemoApp1.ProfileInfo;

    var _initSettings = {userName: ''};    

    profileInfoObj.Initialize = function(initObj)
    {
        _initSettings.userName = initObj.userName;        
    }
})();