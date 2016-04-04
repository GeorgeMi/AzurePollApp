(function ()  {
    "use strict";
    angular
        .module("formManagement")
        .controller("FormCategoryController", ["formResource", "$cookies", FormCategoryController]);
       

    function FormCategoryController(formResource, $cookies) {
        var vm = this;

        vm.page_nr = 0; //numarul paginii
        vm.per_page = 1; //numarul de elemente de pe pagina
        vm.Prev = false; // se afiseaza "prev page" la paginare
        vm.Next = true; // se afiseaza "next page" la paginare

        var param = { category_id: $cookies.get('category_id'), page_nr: vm.page_nr, per_page: vm.per_page };

        formResource.getCategoryForms.getCategoryForms(param,function (data) {
            vm.forms = data;

            if (vm.forms.length < vm.per_page)
            {
                vm.Next = false;
            }
            else
            {
                vm.Next = true;
            }

        });

        vm.viewResults = function (id) {
            //alert("cookie");
            $cookies.put('my_poll_result', id);
            return 'my_poll_result';
        }

        vm.chosePerPage = function (id) {
            //schimba numarul de elemente de pe pagina
            vm.per_page = id;
            vm.page_nr = 0;
            var param = { category_id: $cookies.get('category_id'), page_nr: 0, per_page: id };

            formResource.getCategoryForms.getCategoryForms(param, function (data) {

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
            var param = { category_id: $cookies.get('category_id'), page_nr: id, per_page: vm.per_page };

            if (vm.page_nr <= 0) {
                vm.Prev = false;
            }
            else {
                vm.Prev = true;
            }

            formResource.getCategoryForms.getCategoryForms(param, function (data) {
                vm.forms = data;

                if (vm.forms.length < vm.per_page) {
                    vm.Next = false;
                }
                else {
                    vm.Next = true;
                }

            });
        }

    }
}());
