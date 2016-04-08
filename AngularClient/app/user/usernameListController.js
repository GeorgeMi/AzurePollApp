(function () {
    "use strict";
    angular
        .module("userManagement")
        .controller("UsernameController", ["userResource", "$rootScope", UsernameController]);

    function UsernameController(userResource, $rootScope) {
        var vm = this;

        userResource.getUsername.getUsername(function (data) {

            vm.users = data;

         
        });
    }
})();
