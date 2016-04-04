(function () {
    "use strict";
    angular
        .module("formManagement")
        .controller("FormSearchController", ["formResource", "$cookies", "$rootScope", FormSearchController]);

    function FormSearchController(formResource, $cookies, $rootScope) {
        var vm = this;
        vm.searchText = '';
        vm.page_nr = 0; //numarul paginii
        vm.per_page = 1; //numarul de elemente de pe pagina
        vm.Prev = false; // se afiseaza "prev page" la paginare
        vm.Next = false; // se afiseaza "next page" la paginare
        $rootScope.isLoading = false; //loading gif

        vm.Search = function () {
            if (vm.searchText != '') {
                vm.searchText = vm.searchText.trim();
            }

            if (vm.searchText != '') {
                $rootScope.isLoading = true; //loading gif
                var param = { searchedText: vm.searchText, page_nr: vm.page_nr, per_page: vm.per_page };

                formResource.search.searchForms(param, function (data) {
                    vm.forms = data;
                    if (vm.forms.length < vm.per_page) {
                        vm.Next = false;
                    }
                    else {
                        vm.Next = true;
                    }

                });

                $rootScope.isLoading = false; //loading gif
            }
           
        }

        vm.chosePerPage = function (id) {
            //schimba numarul de elemente de pe pagina
            vm.per_page = id;
            vm.page_nr = 0;
            var param = { searchedText: vm.searchText, page_nr: vm.page_nr, per_page: vm.per_page };

            formResource.search.searchForms(param, function (data) {
                vm.forms = data;
                if (vm.forms.length < vm.per_page) {
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

    }
}());