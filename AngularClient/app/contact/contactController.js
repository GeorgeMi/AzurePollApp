(function () {
    "use strict";
    angular
        .module("contactManagement")
        .controller("ContactController", ["contactResource", "$cookies", ContactController]);

    function ContactController(contactResource, $cookies) {
        var vm = this;

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

                   vm.contact.title = '';
                   vm.contact.category = '';
                   vm.messageContact = 'Message sent successfully';

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