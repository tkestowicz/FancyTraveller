function onGoogleMapsApiReady() {
    angular.bootstrap(document.getElementsByTagName("body"), ['fancyTraveller']);
}

var app = angular.module('fancyTraveller', ['ui.bootstrap', 'ui.map', 'ui.event']);

app.controller('suggestingCities', function ($scope, citiesRepository) {
    $scope.selected = undefined;

    citiesRepository.listOfAvailableCitites().then(function (data) {
        $scope.cities = data;
    });
});

app.controller('map', ['$scope', function ($scope) {
    var p1 = new google.maps.LatLng(51.51121389999999, -0.1198244);
    var p2 = new google.maps.LatLng(45.764043, 4.835659);

    $scope.mapOptions = {
        center: p1,
        zoom: 4,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    $scope.onMapIdle = function () {
        if ($scope.myMarkers === undefined) {
            var marker = new google.maps.Marker({
                map: $scope.mapWithTheRoute,
                position: p1
            });
            var marker2 = new google.maps.Marker({
                map: $scope.mapWithTheRoute,
                position: p2
            });
            $scope.myMarkers = [marker, marker2, ];

            var line = new google.maps.Polyline({
                path: [p1, p2],
                strokeColor: "#FF0000",
                strokeOpacity: 0.3,
                strokeWeight: 10,
                map: $scope.mapWithTheRoute
            });
        }
    };

    $scope.markerClicked = function (marker) {
        $scope.currentMarker = marker;
        $scope.currentMarkerLat = marker.getPosition().lat();
        $scope.currentMarkerLng = marker.getPosition().lng();
    };

    
}]);



app.factory('citiesRepository', function citiesRepository($http) {

    var citiesPath = '/api/AvailableCities/';

    return {
        listOfAvailableCitites: function () {
            return $http.get(citiesPath).then(function (result) {
                return result.data;
            });
        }
    };
});