function onGoogleMapsApiReady() {
    angular.bootstrap(document.getElementsByTagName("body"), ['fancyTraveller']);
}

var app = angular.module('fancyTraveller', ['ui.bootstrap', 'ui.map', 'ui.event']);

app.controller('suggestingCities', function ($scope, citiesRepository) {

    citiesRepository.listOfAvailableCitites().then(function (data) {
        $scope.cities = data;
    });
});

app.controller('route', function($scope, $http) {

    $scope.query = {
        sourceCity: undefined,
        destinationCity: undefined,
        citiesToSkip: []
    };

    $scope.result = {
        show: {
            map: false,
            summary: false
        },
        positions: {
            source: undefined,
            destination: undefined
        },
        distance: 0
    };

    $scope.findRouteFor = function () {
        $scope.result.show.summary = false;
        
        var shortestPathUrl = '/api/FindShortestRoute/';

        var selectCititesIds = function() {
            var result = [];
            for (var city in $scope.query.citiesToSkip)
                if ($scope.query.citiesToSkip.hasOwnProperty(city))
                    result.push($scope.query.citiesToSkip[city].Id);

            return result;
        };

        var query = {
            sourceCity: $scope.query.sourceCity,
            destinationCity: $scope.query.destinationCity,
            citiesToSkip: selectCititesIds()
        };
        
        $http.post(shortestPathUrl, query).then(function(response) {

            $scope.displayResult(response.data);

        }, function(response) {
            // TODO: error handling
            console.log(response);
        });
    };

    $scope.displayResult = function(result) {
        $scope.result.show.map = $scope.result.show.summary = true;

        $scope.result.distance = result.Distance;

        $scope.result.positions = {
            source: new google.maps.LatLng(result.SourceCity.Location.Latitude, result.SourceCity.Location.Longitude),
            destination: new google.maps.LatLng(result.DestinationCity.Location.Latitude, result.DestinationCity.Location.Longitude)
        };
    };  
});

app.controller('placesToSkipManagement', function($scope) {

   $scope.removeCityToSkip = function (indexOfTheCity) {

       if ($scope.query.citiesToSkip[indexOfTheCity] !== undefined)
            $scope.query.citiesToSkip.splice(indexOfTheCity, 1);
    };

    $scope.addCityToSkip = function (selectedCity) {
      
        if ($scope.query.citiesToSkip.indexOf(selectedCity) === -1)
            $scope.query.citiesToSkip.push(selectedCity);

        document.getElementById('blacklistInput').value = null;
    };
});

app.controller('map', ['$scope', function ($scope) {

    var markersArray = [];
    var line = new google.maps.Polyline({});

    $scope.mapOptions = {
        zoom: 4,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    $scope.$watch("result.show.summary", function (val) {
        window.setTimeout(function () {
            google.maps.event.trigger($scope.mapWithTheRoute, 'resize');
        }, 100);

        line.setMap(null);

        if ($scope.result.positions.source === undefined || $scope.result.positions.destination === undefined) return;

        (function clearOverlays() {
            for (var i = 0; i < markersArray.length; i++) {
                markersArray[i].setMap(null);
            }
            markersArray.length = 0;
        }());

        var source = new google.maps.Marker({
            map: $scope.mapWithTheRoute,
            position: $scope.result.positions.source
        });

        var destination = new google.maps.Marker({
            map: $scope.mapWithTheRoute,
            position: $scope.result.positions.destination
        });

        $scope.myMarkers = [source, destination];

        line = new google.maps.Polyline({
            path: [$scope.result.positions.source, $scope.result.positions.destination],
            strokeColor: "#FF0000",
            strokeOpacity: 0.3,
            strokeWeight: 10,
            map: $scope.mapWithTheRoute,
        });

        markersArray.push(source);
        markersArray.push(destination);
        
        google.maps.event.trigger($scope.mapWithTheRoute, 'resize');
        $scope.mapWithTheRoute.setCenter(new google.maps.LatLng(source.position.lat(), source.position.lng()));
    });  
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