(function ()  {
    "use strict";
    angular
        .module("contactManagement")
        .controller("ContactController", ["contactResource", "$cookies", "$rootScope", ContactController]);

    function ContactController(contactResource, $cookies,  $rootScope) {
        var vm = this;
        vm.sent = false;
        $rootScope.isLoading = false;

        vm.contact = {
            category: "Message",
            message: ""
        }
        var x = JSON.stringify(vm.contact);

        vm.sendMessage = function () {
           
            if (vm.contact.message != '') {

                $rootScope.isLoading = true;

                var x = JSON.stringify(vm.contact);

                contactResource.send.sendMessage(x,
                   //s-a trimis cu succes
                   function (data) {
                      
                       vm.contact.message = '';
                       vm.contact.category = '';
                       vm.messageContact = 'Message sent successfully';
                       vm.sent = true;

                   },

                  //nu s-a trimis
                   function (response) {
                       if (response.data.error) {
                           vm.messageContact = response.data.error;
                       }
                       else {

                       }
                   });

                $rootScope.isLoading = false;
            }
        }

    }

}());