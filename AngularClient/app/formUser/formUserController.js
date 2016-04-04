(function ()  {
    "use strict";
    angular
        .module("formManagement")
        .controller("FormUserController", ["formResource", "$cookies", FormUserController]);

    function FormUserController(formResource, $cookies) {
        var vm = this;
        vm.page_nr = 0; //numarul paginii
        vm.per_page = 10; //numarul de elemente de pe pagina
        vm.Prev = false; // se afiseaza "prev page" la paginare
        vm.Next = true; // se afiseaza "next page" la paginare


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

        });


        vm.deleteForm = function (formID) {
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
        }

        vm.changeID = function (id) {
            //alert("cookie");
            $cookies.put('my_poll_result', id);
            return 'my_poll_result';
        }

        vm.chosePerPage = function (id) {
            //schimba numarul de elemente de pe pagina
            vm.per_page = id;
            vm.page_nr = 0;
            var param = { page_nr: 0, per_page: id };

            formResource.getForms.getForms(param, function (data) {

                vm.userForms = data;

                if (vm.userForms.length < vm.per_page) {
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

            formResource.getForms.getForms(param, function (data) {
                vm.userForms = data;

                if (vm.userForms.length < vm.per_page) {
                    vm.Next = false;
                }
                else {
                    vm.Next = true;
                }

            });
        }
    }
}());
