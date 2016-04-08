(function () {
    "use strict";
    angular
        .module("contactManagement")
        .controller("ContactAdminRedirectController", ["contactResource", "$cookies", "$rootScope", ContactAdminRedirectController]);

    function ContactAdminRedirectController(contactResource, $cookies, $rootScope) {
        var vm = this;
        vm.sent = false;
        $rootScope.isLoading = false;
        vm.receiverUsername = $cookies.get('receiver_username');
        
        vm.contact = {
            category: "Message",
            message: "",
            receiver: $cookies.get('receiver_id')
        }
        var x = JSON.stringify(vm.contact);

        vm.sendUserMessage = function () {

            if (vm.contact.message != '') {

                $rootScope.isLoading = true;
               
                var x = JSON.stringify(vm.contact);

                contactResource.send.sendMessage(x,
                   //s-a trimis cu succes
                   function (data) {

                       vm.contact.message = '';
                       vm.contact.category = '';
                       vm.messageContact = 'Message sent successfully';
                       vm.sent = "success";
                       $rootScope.isLoading = false;
                   },

                  //nu s-a trimis
                   function (response) {
                       if (response.data.error) {
                           vm.messageContact = response.data.error;
                           vm.sent = "fail";
                       }
                       else {

                       }
                       $rootScope.isLoading = false;
                   });
            }
        }

    }

}());