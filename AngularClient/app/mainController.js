(function () {
    "use strinct";
    angular
        .module("app")
        .controller("MainController", ["userAccount", "$cookies", "$routeParams" ,"$location","$rootScope", MainController])



    function MainController(userAccount, $cookies, $routeParams, $location, $rootScope) {

        var vm = this;

        vm.isLoggedIn = true;
        $rootScope.isLoading = false; //loading gif
        vm.messageSuccessRegistration = '';
        vm.messageFailedRegistration = '';
        vm.messageLogIn = '';
        vm.message = '';
        vm.role = 'admin';

        //data for login
        vm.userData = {
            username: $cookies.get('username'),
            password: ''
        };

        //data for registration
        vm.userDataRegistration = {
            username: '',
            password: '',
            email: ''
        };

        //app pages
        vm.pages = {
            home: true,
            my_polls: false,
            my_poll_result: false,
            categories: false,
            new_poll: false,
            manage_users: false,
            manage_polls: false,
            manage_categories: false,
            vote_poll: false,
            voted_polls: false,
            category_forms: false,
            contact: false,
            search_polls: false
        };

        vm.confirm_password = '';

        vm.tokenDataRegistration = $cookies.get('token');


        //------------------register-----------------------
        vm.registerUser = function () {


            if (vm.userDataRegistration.email != ''
               && vm.userDataRegistration.username != ''
                && vm.userDataRegistration.password != '') {

                vm.userDataRegistration.email = vm.userDataRegistration.email.replace(/ /g, '');
                vm.userDataRegistration.username = vm.userDataRegistration.username.replace(/ /g, '');
                vm.userDataRegistration.password = vm.userDataRegistration.username.replace(/ /g, '');
            }
            
            if (vm.userDataRegistration.email != ''
                && vm.userDataRegistration.username != ''
                 && vm.userDataRegistration.password != '') {

                if (vm.confirm_password == vm.userDataRegistration.password) {
                    //start loading
                    $rootScope.isLoading = true;

                    userAccount.registration.registerUser(vm.userDataRegistration,

                        function (data) {
                            //inregistrarea a avut succes
                            vm.messageSuccessRegistration = data.success;
                            vm.userData.username = vm.userDataRegistration.username;
                            vm.userData.password = vm.userDataRegistration.password;
                            //  vm.login();

                            $rootScope.isLoading = false;
                        },

                        function (response) {
                            //inregistrarea nu a avut succes
                            vm.isLoggedIn = false;
                            $rootScope.isLoading = false;

                            if (response.data.error) {
                                vm.messageFailedRegistration = response.data.error;
                            }

                            //validation errors
                            if (response.data.modelState) {

                                for (var key in response.data.modelState) {
                                    vm.messageFailedRegistration += response.data.modelState[key] + "\r\n";
                                }
                            }
                        });
                }
                else {

                    vm.messageFailedRegistration = "Passwords don't match";
                }
            }
        }

        //-----------------login with username and password-----------------------
        vm.login = function () {

            if (vm.userData.password != '' && vm.userData.username != '') {

                vm.userData.password = vm.userData.password.replace(/ /g, '');
                vm.userData.username = vm.userData.username.replace(/ /g, '');
            }

            if (vm.userData.password != '' && vm.userData.username != '') {

                //start loading
                $rootScope.isLoading = true;
                userAccount.login.loginUser(vm.userData,

                    function (data) {
                        if (data.error) {
                            vm.isLoggedIn = false;
                            vm.password = "";
                            vm.messageLogIn = data.error;

                        } else {
                            vm.isLoggedIn = true;
                            vm.password = "";
                            vm.token = data.token;
                            vm.role = data.role;
                            //always load on home page
                            vm.changePage('home');

                            var expireDate = new Date();
                            expireDate.setDate(expireDate.getDate() + 1);

                            $cookies.put("token", data.token, { 'expires': expireDate });
                            $cookies.put("username", vm.userData.username, { 'expires': expireDate });
                            $cookies.put("role", vm.role, { 'expires': expireDate });

                        }
                        //stop loading
                        $rootScope.isLoading = false;

                    }, function (response) {
                        //inregistrarea nu a avut succes
                        vm.isLoggedIn = false;
                        vm.password = "";
                        vm.messageLogIn = response.data.error;

                        //stop loading
                        $rootScope.isLoading = false;
                    })
            }
        }

        //------------------login with token-----------------------
        vm.registerToken = function () {

            userAccount.tokenRegistration.registerToken(
                function (data) {

                    if (data.error) {
                        //token invalid
                        vm.isLoggedIn = false;
                        vm.messageLogIn = data.error;

                    } else {
                        //token acceptat
                        vm.isLoggedIn = true;

                        //always load on home page
                        vm.changePage('home');

                        var expireDate = new Date();
                        expireDate.setDate(expireDate.getDate() + 1);
                        vm.role = data.role;

                        $cookies.put("token", vm.tokenDataRegistration, { 'expires': expireDate });
                        $cookies.put("username", vm.userData.username, { 'expires': expireDate });
                        $cookies.put("role", vm.role, { 'expires': expireDate });

                    }

                })

        }

        //------------------logout-----------------------
        vm.logout = function () {
            $cookies.remove("token");
            $cookies.remove("role");

            vm.isLoggedIn = false;

            vm.pages.home = false;
            vm.pages.categories = false;
            vm.pages.my_poll_result = false;
            vm.pages.my_polls = false;
            vm.pages.new_poll = false;
            vm.pages.manage_users = false;
            vm.pages.manage_polls = false;
            vm.pages.manage_categories = false;
            vm.pages.vote_poll = false;
            vm.pages.voted_polls = false;
            vm.pages.category_forms = false;
            vm.pages.contact = false;
            vm.pages.search_polls = false;

        }

        //------------------change page-----------------------

        vm.changePage = function (mypage) {
            //inchide sideBar la smartphone
            $(".mobileSideBarVisible").addClass("sideBarHidden");

            vm.pages.home = false;
            vm.pages.categories = false;
            vm.pages.category_forms = false;
            vm.pages.my_polls = false;
            vm.pages.my_poll_result = false;
            vm.pages.new_poll = false;
            vm.pages.manage_users = false;
            vm.pages.manage_polls = false;
            vm.pages.manage_categories = false;
            vm.pages.vote_poll = false;
            vm.pages.voted_polls = false;
            vm.pages.contact = false;
            vm.pages.search_polls = false;


            if (mypage == 'home') {
                vm.pages.home = true;
            }
            else if (mypage == 'categories') {
                vm.pages.categories = true;
            }
            else if (mypage == 'my_polls') {
                vm.pages.my_polls = true;
            }
            else if (mypage == 'new_poll') {
                vm.pages.new_poll = true;
            }
            else if (mypage == 'manage_users') {
                vm.pages.manage_users = true;
            }
            else if (mypage == 'manage_polls') {
                vm.pages.manage_polls = true;
            }
            else if (mypage == 'manage_categories') {
                vm.pages.manage_categories = true;
            }
            else if (mypage == 'vote_poll') {
                vm.pages.vote_poll = true;
            }
            else if (mypage == 'my_poll_result') {
                vm.pages.my_poll_result = true;
            }
            else if (mypage == 'voted_polls') {
                vm.pages.voted_polls = true;
            }
            else if (mypage == 'category_forms') {
                vm.pages.category_forms = true;
            }
            else if (mypage == 'contact') {
                vm.pages.contact = true;
            }
            else if (mypage == 'search_polls') {
                vm.pages.search_polls = true;
            }


        }


        //------------------verify mail---------------------
        if ($location.search().verifymail) {
            //start loading
            $rootScope.isLoading = true;

            var param = { user_id: $location.search().verifymail };

            userAccount.verifyMail.verifyMail(param,

                        function (data) {
                            //validarea a avut succes
                            vm.messageSuccessRegistration = data.success;

                        },

                        function (response) {

                            //validarea nu a avut succes
                            if (response.data.error) {
                                vm.messageFailedRegistration = response.data.error;
                            }

                        });

            //stop loading
            $rootScope.isLoading = false;
        }
            //change page using url
        else if ($location.url()) {
            vm.changePage($location.url());
        }


    
    };
})();