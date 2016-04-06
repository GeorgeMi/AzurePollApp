(function () {
    "use strict";
    angular
        .module("formManagement")
        .controller("FormUserController", ["formResource", "$cookies","$rootScope", FormUserController]);

    function FormUserController(formResource, $cookies, $rootScope) {
        var vm = this;
        vm.page_nr = 0; //numarul paginii
        vm.per_page = 10; //numarul de elemente de pe pagina
        vm.Prev = false; // se afiseaza "prev page" la paginare
        vm.Next = true; // se afiseaza "next page" la paginare
        $rootScope.isLoading = true; //loading gif

        //  var param = { username: $cookies.get('username') };

        var param = { page_nr: vm.page_nr, per_page: vm.per_page };

        formResource.getForms.getForms(param, function (data) {

            vm.userForms = data;

            if (vm.userForms.length < vm.per_page) {
                vm.Next = false;
            }
            else {
                vm.Next = true;
            }
            $rootScope.isLoading = false;
        });


        vm.deleteForm = function (formID) {
            $rootScope.isLoading = true;

            var param = { form_id: formID };
            var i;
            // alert(formID);

            formResource.delete.deleteForm(param,
                function (data) {

                    for (i = 0; i < vm.userForms.length ; i++) {

                        if (vm.userForms[i].Id === formID) {
                            vm.userForms.splice(i, 1);
                        }
                    }
                });
            $rootScope.isLoading = false;
        }

        vm.changeID = function (id) {
            //alert("cookie");
            $cookies.put('my_poll_result', id);
            return 'my_poll_result';
        }


        vm.itemsPerPage = vm.per_page;
        vm.chosePerPage = function () {
            //schimba numarul de elemente de pe pagina

            if (vm.itemsPerPage != vm.per_page) {

                $rootScope.isLoading = true;
                vm.per_page = vm.itemsPerPage;
                vm.page_nr = 0;
                var param = { page_nr: 0, per_page: vm.itemsPerPage };
                formResource.get.getForms(param, function (data) {

                    vm.forms = data;

                    if (vm.forms.length < vm.per_page) {
                        vm.Next = false;
                    }
                    else {
                        vm.Next = true;
                    }

                    if (vm.page_nr <= 0) {
                        vm.Prev = false;
                    }
                    else {
                        vm.Prev = true;
                    }
                });
                $rootScope.isLoading = false;
            }
        }

        vm.chosePageNr = function (id) {
            //schimba numarul paginii
            $rootScope.isLoading = true;
            vm.page_nr = id;
            var param = { searchedText: vm.searchText, page_nr: vm.page_nr, per_page: vm.per_page };

            if (vm.page_nr <= 0) {
                vm.Prev = false;
            }
            else {
                vm.Prev = true;
            }

            formResource.search.searchForms(param, function (data) {
                vm.forms = data;
                if (vm.forms.length < vm.per_page) {
                    vm.Next = false;
                }
                else {
                    vm.Next = true;
                }

                if (vm.page_nr <= 0) {
                    vm.Prev = false;
                }
                else {
                    vm.Prev = true;
                }

            });
        }
        $rootScope.isLoading = false;

    }
}());
