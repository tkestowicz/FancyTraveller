'use strict;'

var cacheService = function()
{

	function contains(key)
	{
		return (get(key) !== undefined && get(key) !== null);
	}

	function add(key, value)
	{
		sessionStorage.setItem(key, value);
	}

	function get(key)
	{
		return sessionStorage.getItem(key);
	}

	function remove(key)
	{
		sessionStorage.remove(key);
	}

	function clear()
	{
		sessionStorage.clear();
	}

	return {
		contains : contains,
		add : add,
		remove : remove,
		get : get,
		clear : clear
	};
}();

var googleMapsService = function(cache)
{

	var geoLocationUrlPattern = "http://maps.googleapis.com/maps/api/geocode/json?address=#location#&sensor=false";

	var httpHandler = new XMLHttpRequest();

	httpHandler.onreadystatechange = (function (requestHandler)
	{
		return function(){ return parseResponse(requestHandler); }
	})(httpHandler);

	var returnedLocation = {};

	function parseResponse(requestHandler)
	{
		if(requestHandler.readyState == 4 && requestHandler.status == 200)
		{

			var jsonResponse = JSON.parse(requestHandler.responseText);

			returnedLocation = { 
					latitude : jsonResponse.results[0].geometry.location.lat, 
					longitude : jsonResponse.results[0].geometry.location.lng 
				};
		}
	}

	function askRemoteServiceForTheLocation(city)
	{
		httpHandler.open("GET", geoLocationUrlPattern.replace("#location#", city), false);
		httpHandler.send("");

		return returnedLocation;

	}

	function getLocationForTheCity(city)
	{
		try{

			if(!cache.contains(city))
			{
				var location = askRemoteServiceForTheLocation(city);

				cache.add(city, JSON.stringify(location));

				return location;
			}
			else
				return JSON.parse(cache.get(city));

		}
		catch(e)
		{
			console.log("Something went wrong: " + e.message);
			return { latitude: 0, longitude: 0};
		}
	}

	return {
		getLocationForTheCity : getLocationForTheCity
	};
}(cacheService);

var fancyParser = function(gMaps)
{

	var citiesToParse = [
		['Amsterdam',[]],
		['Antwerp',[161]],
		['Athens',[3082,2766]],
		['Barcelona',[1639,1465,3312]],
		['Berlin',[648,723,2552,1899]],
		['Bern',[875,704,2627,913,986]],
		['Brussels',[209,46,3021,1419,782,655]],
		['Calais',[385,211,2976,1399,936,854,212]],
		['Cologne',[281,237,2562,1539,575,583,219,431]],
		['Copenhagen',[904,861,3414,2230,743,1392,1035,730]],
		['Edinburgh',[1180,1005,3768,2181,1727,1643,996,792,1206,1864]],
		['Frankfurt',[471,427,2382,1284,570,424,409,621,190,799,1395]],
		['Geneva',[1014,840,2692,758,1141,155,674,820,765,1531,1536,585]],
		['Genoa',[1310,1136,2242,946,1188,448,1090,1213,1061,1552,1922,881,568]],
		['Hamburg',[455,450,2758,1856,291,906,586,798,446,321,1555,497,1082,1378]],
		['Le Havre',[669,453,3494,1336,1189,767,407,284,576,1531,1074,758,757,1233,1082]],
		['Lisbon',[2300,2126,4578,1266,3165,2179,2080,2060,2294,3115,2879,2544,2024,2212,2666,1894]],
		['London',[494,337,3099,1512,1059,975,328,123,538,1196,656,727,867,1253,887,406,2210]],
		['Luxembourg',[371,280,2744,1137,767,429,233,414,195,1106,1206,249,484,887,761,512,2165,538]],
		['Lyon',[995,821,2774,644,1289,317,671,755,830,1586,1552,640,162,541,1137,692,1784,884,493]],
		['Madrid',[1782,1608,3940,628,2527,1541,1562,1542,1776,2597,2372,1906,1386,1574,2409,1376,638,1704,1647,1272]],
		['Marseille',[1323,1149,2997,515,1584,598,999,1083,1208,1914,1860,1004,443,431,1465,1020,1781,1192,816,328,1143]],
		['Milan',[1154,980,2280,1102,1168,347,934,1057,905,1671,1883,725,412,156,1222,1087,2368,1215,731,494,1730,587]],
		['Munich',[876,832,2210,1349,604,436,811,998,592,1204,1743,383,591,707,755,1038,2515,1075,583,753,1877,1034,551]],
		['Naples',[2068,1894,2784,1704,1806,1261,1848,1971,1819,2585,2664,1639,1326,758,2136,1991,2970,1996,1645,1299,2332,1189,914,1202]],
		['Nice',[1435,1261,2570,685,1610,638,1277,1195,1265,2014,2015,1085,483,204,1565,1132,1951,1347,1106,440,1313,227,360,911,962]],
		['Paris',[514,340,3146,1125,1094,556,294,274,508,1329,1082,592,546,1006,880,211,1786,414,379,481,1268,809,850,827,1764,921]],
		['Prague',[973,870,2198,1679,354,766,911,1082,659,1033,1872,552,954,1007,1235,1305,2945,1204,746,1116,2307,1397,876,363,1603,1274,1094]],
		['Rome',[1835,1661,2551,1471,1573,897,1615,1738,1586,2352,2467,1406,1093,525,1903,1758,2737,1799,1400,1066,2099,856,681,969,233,729,1531,1370]],
		['Rotterdam',[80,100,2826,1565,697,802,146,311,254,813,1100,444,940,1236,486,553,2226,432,361,921,1708,1249,1080,827,1994,1361,440,858,1761]],
		['Strasbourg',[683,544,2581,1072,801,232,488,627,402,1158,1412,212,371,667,709,667,2212,744,220,428,1700,814,511,371,1425,868,456,638,1192,644]],
		['Stuttgart',[703,659,2428,1263,636,350,641,792,395,1178,1534,205,505,688,729,832,2377,866,322,667,1891,948,532,218,1446,892,621,773,1213,676,165]],
		['The Hague',[56,139,3061,1589,712,825,170,352,283,960,1142,463,964,510,560,577,2250,473,390,945,1732,1273,1104,870,2018,1385,464,1028,1785,47,668,673]],
		['Turin',[1264,1090,2252,892,1172,312,1044,1110,1015,1527,1786,835,304,186,1332,1047,2158,1118,860,355,1520,434,139,691,934,207,836,991,701,1190,621,642,1167]],
		['Venice',[1449,1275,2001,1327,1108,642,1229,1352,1072,1708,2146,1020,707,381,1259,1382,2593,1478,1026,789,1955,812,295,504,799,585,1145,798,566,1335,806,680,1399,434]],
		['Vienna',[1196,1180,1186,1989,666,907,1134,1346,915,1345,2098,725,1055,983,896,1496,3255,912,885,1217,2617,1414,887,458,1401,1187,1285,312,1168,1169,829,676,1193,1026,610]],
		['Zurich',[861,687,2449,1036,863,123,641,764,612,1378,1631,432,278,641,929,767,2302,963,438,404,1664,721,305,313,1219,665,557,676,986,787,218,227,811,444,600,784]]
		];

	var output = {};

	function parseDistanceBetweenCities(indexOfSourceCity, indexOfDestinationCity)
	{
		var indexOfNameElement = 0;
		var distanceBetweenCities = {};

		distanceBetweenCities.sourceCity = {};
		distanceBetweenCities.destinationCity = {};

		distanceBetweenCities.sourceCity.name = citiesToParse[indexOfSourceCity][indexOfNameElement];
		distanceBetweenCities.sourceCity.location = gMaps.getLocationForTheCity(distanceBetweenCities.sourceName);
		distanceBetweenCities.destinationCity.name = citiesToParse[indexOfDestinationCity][indexOfNameElement];
		distanceBetweenCities.destinationCity.location = gMaps.getLocationForTheCity(distanceBetweenCities.destinationName);
		distanceBetweenCities.distance = citiesToParse[indexOfDestinationCity][1][indexOfSourceCity];

		return distanceBetweenCities;
	}

	function parseDistancesForSingleCity(indexOfTheCity)
	{
		var parsedDistances = [];

		for(var cityToCompare = indexOfTheCity; cityToCompare < citiesToParse.length; cityToCompare++)
		{
			if(cityToCompare == indexOfTheCity) continue;

			parsedDistances.push(parseDistanceBetweenCities(indexOfTheCity, cityToCompare));
		}

		return parsedDistances;
	}

	function parseDistancesToJson()
	{
		output = [];

		for(var indexOfTheCity in citiesToParse)
		{
			// last element already has connections to all other cities
			if(indexOfTheCity == citiesToParse.length - 1)
				break;

			output.push(parseDistancesForSingleCity(indexOfTheCity));
		}

		// flatten array into one dimension
		output = [].concat.apply([], output);

		return output;
	}

	return {

		parseDistancesToJson : parseDistancesToJson()

	};
}(googleMapsService);

window.onload = function()
{
	console.log(fancyParser.parseDistancesToJson);

	document.getElementsByTagName("body").item(0).innerText = JSON.stringify(fancyParser.parseDistancesToJson);
};