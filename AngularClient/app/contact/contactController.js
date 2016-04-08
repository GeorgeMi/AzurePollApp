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
                   function (data) {
                      
                       vm.contact.message = '';
                       vm.contact.category = '';
                       vm.contact.receiver = -1;
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