(function () {
    "use strict";
    angular
        .module("userManagement")
        .controller("UserController", ["userResource", UserController]);

    function UserController(userResource) {
        var vm = this;

        vm.page_nr = 0; //numarul paginii
        vm.per_page = 10; //numarul de elemente de pe pagina
        vm.Prev = false; // se afiseaza "prev page" la paginare
        vm.Next = true; // se afiseaza "next page" la paginare

        var param = { page_nr: vm.page_nr, per_page: vm.per_page };
       
        userResource.get.getUsers(param,function (data) {
            vm.users = data;

            if (vm.users.length < vm.per_page) {
                vm.Next = false;
            }
            else {
                vm.Next = true;
            }

        });

        vm.deleteUser = function (userID) {
            var r = confirm("Are you sure that you want to permanently delete this user?");

            if (r == true) {

                var param = { user_id: userID };
                var i;

                userResource.delete.deleteUser(param,
                    function (data) {

                        for (i = 0; i < vm.users.length ; i++) {

                            if (vm.users[i].UserID === userID) {
                                vm.users.splice(i, 1);
                            }
                        }

                    });
            }
        }

        vm.promote = function (userID) {
            var param = { user_id: userID };
            var i;

            userResource.promote.promoteUser(param,
                function (data) {

                    for (i = 0; i < vm.users.length ; i++) {

                        if (vm.users[i].UserID === userID) {
                            vm.users[i].Role = 'admin';
                        }
                    }
                });
        }

        vm.demote = function (userID) {
            var param = { user_id: userID };
            var i;

            userResource.demote.demoteUser(param,
                function (data) {

                    for (i = 0; i < vm.users.length ; i++) {

                        if (vm.users[i].UserID === userID) {
                            vm.users[i].Role='user';
                        }
                    }
                });
        }

        vm.chosePerPage = function (id) {
            //schimba numarul de elemente de pe pagina
            vm.per_page = id;
            vm.page_nr = 0;
            var param = { page_nr: 0, per_page: id };
            userResource.get.getUsers(param, function (data) {

                vm.users = data;

                if (vm.users.length < vm.per_page) {
                    vm.Next = false;
                }
                else {
                    vm.Next = true;
                }
            });
        }

        vm.chosePageNr = function (id) {
            //schimba numarul paginii
            vm.page_nr = id;
            var param = { page_nr: id, per_page: vm.per_page };

            if (vm.page_nr <= 0) {
                vm.Prev = false;
            }
            else {
                vm.Prev = true;
            }
            userResource.get.getUsers(param, function (data) {
                vm.users = data;

                if (vm.users.length < vm.per_page) {
                    vm.Next = false;
                }
                else {
                    vm.Next = true;
                }

            });
        }
    }
}());
