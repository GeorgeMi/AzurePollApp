(function () {
    "use strict";
    angular
        .module("contactManagement")
        .controller("ContactController", ["contactResource", "$cookies", ContactController]);

    function ContactController(contactResource, $cookies) {
        var vm = this;
        vm.sent = false;

        vm.contact = {
            category: "Message",
            message: ""
        }
        var x = JSON.stringify(vm.contact);

        vm.sendMessage = function () {

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
        }

    }

}());