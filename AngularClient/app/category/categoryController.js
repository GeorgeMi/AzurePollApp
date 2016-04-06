(function () {
    "use strict";
    angular
        .module("categoryManagement")
        .controller("CategoryController", ["categoryResource", "$cookies","$rootScope", CategoryController]);

    //data for categories


    function CategoryController(categoryResource, $cookies, $rootScope) {
        var vm = this;
        $rootScope.isLoading = true;//loading gif
        
        vm.category =  {
            name: ''
        };
       
        categoryResource.get.getCategories(function (data) {
            vm.categories = data;
            
           
        },$rootScope.isLoading = false);

        vm.addCategory = function () {
            $rootScope.isLoading = true;

            categoryResource.add.addCategory(vm.category, function (data) {

                categoryResource.get.getCategories(function (data) {
                    vm.category.name = '';
                    vm.categories = data;
                    $rootScope.isLoading = false;
                });
            });
        }

        vm.deleteCategory = function (categoryID) {
            var r = confirm("Are you sure that you want to permanently delete this category?");

            if (r == true) {
                $rootScope.isLoading = true;
                var param = { cat_id: categoryID };
                var i;
                categoryResource.delete.deleteCategory(param,
                    function (data) {

                        /*  categoryResource.get.getCategories(function (data) {
                              vm.categories = data;
                          });*/
                        for (i = 0; i < vm.categories.length ; i++) {

                            if (vm.categories[i].CategoryID === categoryID) {
                                vm.categories.splice(i, 1);
                            }
                        }
                        $rootScope.isLoading = false;
                    });
            }
        }

        vm.viewCategoryForms = function (id) {
            //alert("cookie");
            $cookies.put('category_id', id);
            return 'category_forms';
        }
    }
}());
