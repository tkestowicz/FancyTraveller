function onGoogleMapsApiReady() {
    angular.bootstrap(document.getElementsByTagName("body"), ['fancyTraveller']);
}

var app = angular.module('fancyTraveller', ['ui.bootstrap', 'ui.map', 'ui.event']);

app.controller('suggestingCities', function ($scope, routeService) {

    routeService.listOfAvailableCitites().then(function (response) {
        $scope.cities = response.data;
    }, $scope.handleInternalError);
});

app.controller('route', function($scope, $http, routeService) {

    $scope.loader = $scope.disabledSendBtn = $scope.internalError = false;

    $scope.block = function() {
        $scope.loader = $scope.disabledSendBtn = true;
    };

    $scope.unblock = function() {
        $scope.loader = false;
        $scope.disabledSendBtn = undefined;
    };

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
        distance: 0,
        visitedCities: []
    };

    $scope.findRouteFor = function () {
        $scope.result.show.summary = false;
        
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

        $scope.block();

        routeService.findShortestRoute(query).then(function (response) {
            $scope.unblock();
            $scope.displayResult(response.data);

        }, $scope.handleInternalError);
    };

    $scope.handleInternalError = function (response) { // onError
        $scope.unblock();
        $scope.internalError = true;

        console.log(response);
    };

    $scope.displayResult = function(result) {
        $scope.result.show.map = $scope.result.show.summary = true;

        $scope.result.distance = result.Distance;

        $scope.result.positions = {
            source: new google.maps.LatLng(result.SourceCity.Location.Latitude, result.SourceCity.Location.Longitude),
            destination: new google.maps.LatLng(result.DestinationCity.Location.Latitude, result.DestinationCity.Location.Longitude)
        };

        $scope.result.visitedCities = result.VisitedCities;
    };

    $scope.removeCityToSkipById = function (cityId) {
        for (var index in $scope.query.citiesToSkip)
            if ($scope.query.citiesToSkip[index] !== undefined && $scope.query.citiesToSkip[index].Id === cityId)
                $scope.query.citiesToSkip.splice(index, 1);
    };
});

app.controller('placesToSkipManagement', function($scope) {

   $scope.removeCityToSkipByIndex = function (indexOfTheCity) {

       if ($scope.query.citiesToSkip[indexOfTheCity] !== undefined)
            $scope.query.citiesToSkip.splice(indexOfTheCity, 1);
   };

    $scope.addCityToSkip = function (selectedCity, ctrl) {

        var isValid = function (city) {
            var source = $scope.query.sourceCity || {};
            var destination = $scope.query.destinationCity || {};

            return (source.Name !== city.Name && destination.Name !== city.Name);
        };

        var valid = isValid(selectedCity);
        ctrl.$setValidity('blacklist', valid);

        if (valid && $scope.query.citiesToSkip.indexOf(selectedCity) === -1)
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

app.factory('routeService', function ($http) {

    var citiesPath = '/api/AvailableCities/';
    var shortestPathUrl = '/api/FindShortestRoute/';

    return {
        listOfAvailableCitites: function () {
            return $http.get(citiesPath);
        },
        findShortestRoute: function(query) {
            return $http.post(shortestPathUrl, query);
        }
    };
});

app.directive('differentSourceAndLocation', function () {
    return {
        // restrict to an attribute type.
        restrict: 'A',

        // element must have ng-model attribute.
        require: 'ngModel',

        // scope = the parent scope
        // elem = the element the directive is on
        // attr = a dictionary of attributes on the element
        // ctrl = the controller for ngModel.
        link: function (scope, elem, attr, ctrl) {

            var compareCities = function(source, destination) {
                return JSON.stringify(source) !== JSON.stringify(destination);
            };

            var validation = function(value) {
                // test and set the validity after update.
                var valid = compareCities(value, scope.$eval(attr.cityToCompare));
                ctrl.$setValidity('differentSourceAndLocation', valid);

                if (valid)
                    scope.$eval(attr.dependsOn).$setValidity('differentSourceAndLocation', valid);

                // if it's valid, return the value to the model, 
                // otherwise return undefined.
                return value;
            };

           // add a parser that will process each time the value is 
            // parsed into the model when the user updates it.
            ctrl.$parsers.unshift(validation);

            // add a formatter that will process each time the value 
            // is updated on the DOM element.
            ctrl.$formatters.unshift(validation);
        }
    };
});