function onGoogleMapsApiReady() {
    angular.bootstrap(document.getElementsByTagName("body"), ['fancyTraveler']);
}

var app = angular.module('fancyTraveler', ['ui.bootstrap', 'ui.map']);

app.controller('suggestingCities', function ($scope, citiesRepository) {
    $scope.selected = undefined;

    citiesRepository.listOfAvailableCitites().then(function (data) {
        $scope.cities = data;
    });
});

app.controller('map', ['$scope', function ($scope) {
    $scope.mapOptions = {
        center: new google.maps.LatLng(35.784, -78.670),
        zoom: 15,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
}]);

app.factory('citiesRepository', function citiesRepository($http) {

    var citiesPath = 'availableCities.txt';

    return {
        listOfAvailableCitites: function() {
            return $http.get(citiesPath).then(function(result) {
                return result.data;
            });
        }
        };
});