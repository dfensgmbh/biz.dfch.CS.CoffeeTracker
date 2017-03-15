. ..\Functions\Delete-Entities.ps1
. ..\Functions\Get-Token.ps1
. ..\Functions\CRUD-User.ps1
. ..\Functions\CRUD-Coffee.ps1

Describe "StatisticsTest" -Tags "StatisticsTest" {
	Write-Host -ForegroundColor Magenta "Create test data pls wait";
	Write-Host -ForegroundColor Magenta "This process will take at least 30 seconds...";
	$entityPrefix = "StatisticsTest";
	$baseuri = "CoffeeTracker/api/CoffeeOrders"
	$dateTimeFormat = "yyyy-MM-ddTHH:mm:sszzz";

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

	# Create headers for admin for requests below
	$authString = "bearer {0}" -f $adminToken;
	$adminHeaders = [System.Collections.Generic.Dictionary[[String],[String]]]::New();
	$adminHeaders.Add("Authorization", $authString);

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
	Start-Sleep -Seconds 5;

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
	Start-Sleep -Seconds 10;
	$timeAfterTestDataCreationRequests = [DateTimeOffset]::Now;

	## Needed for tests later
	Start-Sleep -Seconds 2;
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
	Start-Sleep -Seconds 10;
	$timeAfterSecondTestDataCreationRequests = [DateTimeOffset]::Now;
	Write-Host -ForegroundColor Magenta "Done! starting tests";

	Context "CoffeeConsumptionOfAllUsers" {
	<#
		It "Warmup" -Test {
			$true | Should Be $true;
		}

		It "CoffeeConsumption-ReturnsCountOfAllOrderedCoffees" -test {
			# Arrange
			$requestUri = "$baseUri/GetCoffeeConsumption";
			$coffeeOrders = Invoke-RestMethod -Method Get -Uri $baseUri -Headers $adminHeaders;
			$coffeeOrdersCount = $coffeeOrders.Count;

			# Act
			$response = Invoke-RestMethod -Method Post -Uri $requestUri -Headers $normalUserHeaders;
			$result = $response.value;

			# Assert
			$result | Should Be $coffeeOrdersCount;
		}

		It "CoffeeConsumption-ReturnsCountOfAllOrderedCoffeesOfSpecifiedTime" -test {
			# Arrange
			$requestUri = "$baseUri/GetCoffeeConsumption";

			$requestBody = @{
				From = $timeBeforeTestDataCreationRequests.ToString($dateTimeFormat)
				Until = $timeAfterTestDataCreationRequests.ToString($dateTimeFormat)
			}

			$currentTestRequestHeaders = $normalUserHeaders;
			$currentTestRequestHeaders.Add("Content-Type","application/json")

			$requestBodyJson = $requestBody | ConvertTo-Json;

			# Act
			$response = Invoke-RestMethod -Method Post -Uri $requestUri -Headers $currentTestRequestHeaders -Body $requestBodyJson;
			$result = $response.value;

			# Assert
			$result | Should Be $normalUserOrders;
		}
		#>
	}
	Context "CoffeeConsumptionByCoffee" {
		It "Warmup" -Test {
			$true | Should Be $true;
		}

		It "CoffeeConsumptionByCoffee-ReturnsCoffeeOrderCountOfSpecifiedCoffee" -test {
			# Arrange
			$requestUri = "$baseUri/CoffeeConsumptionByCoffee";

			$requestBody = @{
				Name = $coffee.Name
				Brand = $coffee.Brand
			};

			$requestBodyJson = $requestBody | ConvertTo-Json;

			$currentRequestHeaders = $normalUserHeaders;
			$currentRequestHeaders.Add("Content-Type","application/json");

			# Act
			$response = Invoke-RestMethod -Method Post -Uri $requestUri -Headers $currentRequestHeaders -Body $requestBodyJson;
			$result = response.value;

			# Assert
			$result | Should Be $normalUserOrders;
		}

		It "CoffeeConsumptionByCoffee-ReturnsCoffeeOrderCountOfSpecifiedCoffeeAndSpecifiedTime" -test {
			# Arrange
			$requestUri = "$baseUri/CoffeeConsumptionByCoffee";

			$requestBody = @{
				Name = $coffee.Name
				Brand = $coffee.Brand
				From = $timeBeforeSecondTestDataCreationRequests
				Until = $timeAfterSecondTestDataCreationRequests
			};

			$requestBodyJson = $requestBody | ConvertTo-Json;

			$currentRequestHeaders = $normalUserHeaders;
			$currentRequestHeaders.Add("Content-Type","application/json");

			# Act
			$response = Invoke-RestMethod -Method Post -Uri $requestUri -Headers $currentRequestHeaders -Body $requestBodyJson;
			$result = response.value;

			# Assert
			$result | Should Be $normalUserOrders;
		}
		#>
	}

	Context "CoffeeConsumptionByUser" {
		It "Warmup" -Test {
			$true | Should Be $true;
		}

		It "CoffeeConsumptionByUser-ReturnsCountOfCoffeeTheUserOrdered" -Test {
			# Arrange
			$requestUri = "$baseUri/GetCoffeeConsumptionByUser";

			# Act
			$result = Invoke-RestMethod -Method Post -Uri $requestUri -Headers $normalUserHeaders;

			# Assert
			$result.value | Should Be $normalUserOrders;
		}

		It "CoffeeConsumptionByUser-ReturnsCoffeesOrderdByUserOfASpecifiedTime" -Test {
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
				From = $timeBeforeTestDataCreationRequests.ToString($dateTimeFormat)
				Until = $timeAfterTestDataCreationRequests.ToString($dateTimeFormat)
			}

			$currentTestRequestHeaders = $normalUserHeaders;
			$currentTestRequestHeaders.Add("Content-Type","application/json")

			$requestBodyJson = $requestBody | ConvertTo-Json;

			# Act
			$result = Invoke-RestMethod -Method Post -Uri $requestUri -Body $requestBodyJson -Headers $currentTestRequestHeaders;

			# Assert
			$result.value | Should Be $normalUserOrders;
		}
		<#
		#>
	}
	Context "MostOrderedCoffee" {
		<#
		It "Warmup" -Test {
			$true | Should Be $true;
		}

		It "MostOrderedCoffee-ReturnsMostOrderedCoffeeOfAllUsers" -test {
			# Arrange
			$requestUri = "$baseUri/GetMostOrderedCoffee";

			# Act 
			$response = Invoke-RestMethod -Method Post -Uri $requestUri -Headers $normalUserHeaders;
			$result = $response.value;

			# Assert
			$result.Id | Should Be $coffee.Id;
		}

		It "MostOrderedCoffee-ReturnsMostOrderedCoffeeOfSpecifiedTimeOfAllUsers" -test {
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

		It "MostOrderedCoffee-ReturnMostOrderedCoffeesOfAllUsers" -test {
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
		#>
	}
	Context "MostOrderedCoffeeByUser" {
	<#
		It "Warmup" -Test {
			$true | Should Be $true;
		}

		It "MostOrderedCoffeeByUser-ReturnMostOrderedCoffeeOfASpecifiedUser" -test {
			$requestUri = "$baseUri/GetMostOrderedCoffeeByUser";

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

		It "MostOrderedCoffeeByUser-ReturnMostOrderedCoffeeOfCurrentUser" -test {
			# Arrange
			$requestUri = "$baseUri/GetMostOrderedCoffeeByUser";

			# Act
			$response = Invoke-RestMethod -Method Post -Uri $requestUri -Headers $secondUserHeaders;
			$result = $response.value;

			# Assert
			$result.Id | Should Be $differentCoffee.Id;
		}

		It "MostOrderedCoffeeByUser-OfOtherSpecifiedUserThanRequestedUserThrows403" -test {
			$requestUri = "$baseUri/GetMostOrderedCoffeeByUser";

			# Arrange
			$requestBody = @{
				Name = $secondUserName
			};

			# Act / Assert
			{ $response = Invoke-RestMethod -Method Post -Uri $requestUri -Headers $normalUserheaders -Body $requestBody; } | Should Throw "403";
		}
		#>
	}
	AfterAll {
		Write-Host -ForegroundColor Magenta "Delete Test-Data...";
		$entityPrefix = "StatisticsTest";
		$queryOption = "startswith(Name, '$entityPrefix')";

		$adminName = "admin@example.com";
		$adminPw = "123456";
		$adminToken = Get-Token -UserName $adminName -Password $adminPw;

		Delete-Entities -EntityName "CoffeeOrders" -OdataComparison $queryOption -Token $adminToken;
		Delete-Entities -EntityName "Coffees" -OdataComparison $queryOption -Token $adminToken;
		Delete-Entities -EntityName "Users" -OdataComparison $queryOption -Token $adminToken;
		Write-Host -ForegroundColor Green "TestData Deleted!";
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
