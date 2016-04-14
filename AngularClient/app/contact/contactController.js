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
            message: "",
            receiver: -1
        }
        var x = JSON.stringify(vm.contact);

        vm.sendUserMessage = function () {
           
            if (vm.contact.message != '') {

                $rootScope.isLoading = true;
                vm.contact.receiver = 0;//0 == send to admin
                var x = JSON.stringify(vm.contact);

                contactResource.send.sendMessage(x,
                   //s-a trimis cu succes
                   function (response) {
                      
                       vm.contact.message = '';
                       vm.contact.category = '';
                       vm.contact.receiver = -1;
                       vm.messageContact = response.message;
                       vm.sent = response.status;
                       $rootScope.isLoading = false;
                   },

                  //nu s-a trimis
                   function (error) {
                       vm.messageContact = error.message;
                       vm.sent = error.status;
                       $rootScope.isLoading = false;
                   });

               
            }
        }

    }

}());