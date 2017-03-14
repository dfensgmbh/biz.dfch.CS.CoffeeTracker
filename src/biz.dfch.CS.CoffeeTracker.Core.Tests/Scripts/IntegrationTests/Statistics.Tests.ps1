. ..\Functions\Delete-Entities.ps1
. ..\Functions\Get-Token.ps1
. ..\Functions\CRUD-User.ps1
. ..\Functions\CRUD-Coffee.ps1

Describe "StatisticsTest" -Tags "StatisticsTest" {
	$entityPrefix = "StatisticsTest";
	$baseuri = "CoffeeTracker/api/CoffeeOrders"

	# Admin Creds
	$adminName = "admin@example.com";
	$adminPw = "123456";
	$adminToken = Get-Token -UserName $adminName -Password $adminPw;

	# Create new User and Get Token
	$normalUserName = "$entityPrefix{0}@example.com" -f [Guid]::NewGuid();
	$normalUserPw = "123456";
	$normalUser = CRUD-User -UserName $normalUserName -Password $normalUserPw -Create;
	$normalUserToken = Get-Token -UserName $normalUserName -Password $normalUserPw;
	$normalUserOrders = 10;

	# Create second new User and Get Token
	$secondUserName = "$entityPrefix{0}@example.com" -f [Guid]::NewGuid();
	$secondUserPw = "123456";
	$secondUser = CRUD-User -UserName $secondUserName -Password $secondUserPw -Create;
	$secondUserToken = Get-Token -UserName $secondUserName -Password $secondUserPw;
	$secondUserOrders = 4;

	# Create Coffee
	$coffeeName = "$entityPrefix-{0}" -f [Guid]::NewGuid();
	$coffeeBrand = "$entityPrefix-BRAND-{0}" -f [Guid]::NewGuid();
	$coffeeStock = 50;
	$coffee = CRUD-Coffee -Name $coffeeName -Brand $coffeeBrand -Stock $coffeeStock -Token $adminToken -Create;

	# Create arbitrary different coffee
	$differentCoffeeName = "$entityPrefix-{0}" -f [Guid]::NewGuid();
	$differentCoffeeBrand = "$entityPrefix-{0}" -f [Guid]::NewGuid();
	$differentCoffeeStock = 5;
	$differentCoffee = CRUD-Coffee -Name $differentCoffeeName -Brand $differentCoffeeBrand -Stock $differentCoffeeStock -Token $adminToken -Create;

	# Create headers for normal user for requests below
	$authString = "bearer {0}" -f $normalUserToken;
	$normalUserHeaders = [System.Collections.Generic.Dictionary[[String],[String]]]::New();
	$normalUserHeaders.Add("Authorization", $authString);

	# Create headers for second user for requests below
	$authString = "bearer {0}" -f $secondUserToken;
	$secondUserHeaders = [System.Collections.Generic.Dictionary[[String],[String]]]::New();
	$secondUserHeaders.Add("Authorization", $authString);
	
	## Needed for tests later
	$timeBeforeTestDataCreationRequests = [DateTimeOffset]::Now;

	# Create CoffeeOrders test data
	for($i = 0; $i -lt $normalUserOrders; $i++)
	{
		$coffeeOrderName = "$entityPrefix-{0}" -f [Guid]::NewGuid();
		
		$coffeeOrderRequestBody = @{
			Name = $coffeeOrderName
			UserId = $normalUser.Id
			CoffeeId = $coffee.Id
		}
		
		Invoke-RestMethod -Method Post -Uri $baseuri -Headers $normalUserHeaders -Body $coffeeOrderRequestBody;
	}
	## Needed for tests later
	$timeAfterTestDataCreationRequests = [DateTimeOffset]::Now;

	## Needed for tests later
	$timeBeforeSecondTestDataCreationRequests = [DateTimeOffset]::Now;

	# Create CoffeOrders test data for second user
	for($i = 0; $i -lt $secondUserOrders; $i++)
	{
		$coffeeOrderName = "$entityPrefix-{0}" -f [Guid]::NewGuid();
		
		$coffeeOrderRequestBody = @{
			Name = $coffeeOrderName
			UserId = $secondUser.Id
			CoffeeId = $differentCoffee.Id
		}
		
		Invoke-RestMethod -Method Post -Uri $baseuri -Headers $secondUserHeaders -Body $coffeeOrderRequestBody;
	}

	## Needed for tests later
	$timeBeforeAfterTestDataCreationRequests = [DateTimeOffset]::Now;

	Context "StatisticTests" {
		
		It "Warmup" -Test {
			$true | Should Be $true;
		}

		It "CoffeeConsumption-ReturnsCountOfCoffeeTheUserOrdered" -Test {
			# Arrange
			$requestUri = "$baseUri/GetCoffeeConsumptionByUser";

			# Act
			$result = Invoke-RestMethod -Method Post -Uri $requestUri -Headers $normalUserHeaders;

			# Assert
			$result.value | Should Be $normalUserOrders;
		}

		It "CoffeeConsumption-ReturnsCoffeesOrderdSpecifiedByTime" -Test {
			# Arrange
			$requestUri = "$baseUri/GetCoffeeConsumptionByUser";
			$coffeeOrderName = "$entityPrefix-{0}" -f [Guid]::NewGuid();
		
			## Create CoffeeOrder which should not be counted from the api
			$coffeeOrderRequestBody = @{
				Name = $coffeeOrderName
				UserId = $normalUser.Id
				CoffeeId = $coffee.Id
			}

			Invoke-RestMethod -Method Post -Uri $baseuri -Headers $normalUserHeaders -Body $coffeeOrderRequestBody;
		
			# prepare request
			$requestBody = @{
				From = $timeBeforeTestDataCreationRequests
				Until = $timeAfterTestDataCreationRequests
			}

			$currentTestRequestHeaders = $normalUserHeaders;
			$currentTestRequestHeaders.Add("Content-Type","application/json")

			$requestBodyJson = $requestBody | ConvertTo-Json;

			# Act
			$result = Invoke-RestMethod -Method Post -Uri $requestUri -Body $requestBodyJson -Headers $currentTestRequestHeaders;

			# Assert
			$result.value | Should Be $normalUserOrders;
		}

		It "CoffeeConsumption-ReturnsMostOrderedCoffee" -test {
			# Arrange
			$requestUri = "$baseUri/GetMostOrderedCoffee";

			# Act 
			$response = Invoke-RestMethod -Method Post -Uri $requestUri -Headers $normalUserHeaders;
			$result = $response.value;

			# Assert
			$result.Id | Should Be $coffee.Id;
		}

		It "CoffeeConsumption-ReturnsMostOrderedCoffeeOfSpecifiedTime" -test {
			# Arrange
			$requestUri = "$baseUri/GetMostOrderedCoffee";

			$requestBody = @{
				From = $timeBeforeSecondTestDataCreationRequests;
				Until = $timeAfterSecondTestDataCreationRequests;
			}

			$currentRequestHeaders = $normalUserHeaders;
			$currentRequestHeaders.Add("Content-Type","application/json");

			$requestBodyJson = $requestBody | ConvertTo-Json;

			# Act
			$response = Invoke-RestMethod -Method Post -Uri $requestUri -Body $requestBodyJson -Headers $currentRequestHeaders;
			$result = $response.value;
			
			# Assert
			$result.Id | Should Be $differentCoffee.Id;
		}

		It "FavouriteCoffees-ReturnMostOrdersCoffeesOfAllUsers" -test {
			# Arrange
			$requestUri = "$baseUri/GetMostOrderedCoffee";
			
			# Create arbitrary coffeeorder of different coffee
			$orderOfDifferentCoffeeName = "$entityPrefix-{0}" -f [Guid]::NewGuid();

			$coffeeOrderRequestBody = @{
				Name = $orderOfDifferentCoffeeName
				UserId = $normalUser.Id
				CoffeeId = $differentCoffee.Id
			}

			Invoke-RestMethod -Method Post -Uri $baseuri -Headers $normalUserHeaders -Body $coffeeOrderRequestBody;
		
			# Act
			$response = Invoke-RestMethod -Method Post -Uri $requestUri -Headers $normalUserHeaders;
			$result = $response.value;

			# Assert
			$result | Should Not Be $null;
			$result.Id | Should Be $coffee.Id;
		}

		It "FavouriteCoffee-ReturnMostOrderedCoffeeOfASpecifiedUser" -test {
			$requestUri = "$baseUri/GetMostOrderedCoffee";

			# Arrange
			$requestBody = @{
				Name = $secondUserName
			};

			$requestBodyJson = $requestBody | ConvertTo-Json;
			$currentTestRequestHeaders = $secondUserHeaders;
			$currentTestRequestHeaders.Add("Content-Type","application/json")

			# Act
			$response = Invoke-RestMethod -Method Post -Uri $requestUri -Headers $currentTestRequestHeaders -Body $requestBodyJson;
			$result = $response.value;

			# Assert
			$result.Id | Should Be $differentCoffee.Id;
		}

		It "FavouriteCoffee-ReturnMostOrderedCoffeeOfCurrentUser" -test {
			# Arrange
			$requestUri = "$baseUri/GetMostOrderedCoffee";

			# Act
			$response = Invoke-RestMethod -Method Post -Uri $requestUri -Headers $secondUserHeaders;
			$result = $response.value;

			# Assert
			$result.Id | Should Be $differentCoffee.Id;
		}

		It "FavouriteCoffee-OfOtherSpecifiedUserThanRequestedUserThrows403" -test {
			$requestUri = "$baseUri/GetMostOrderedCoffee";

			# Arrange
			$requestBody = @{
				Name = $secondUserName
			};

			# Act / Assert
			{ $response = Invoke-RestMethod -Method Post -Uri $requestUri -Headers $normalUserheaders -Body $requestBody; } | Should Throw "403";
		}
	}
}

#
# Copyright 2017 d-fens GmbH
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
# http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
#
