. ./DeleteEntities.ps1

$usedEntitieSets = @("Coffees", "Users", "CoffeeOrders");
$entityPrefix = "CoffeeOrdersIntegrationTest";
$baseUri = "CoffeeTracker/api/";

Describe "CoffeeOrdersController" -Tags "CoffeeOrdersController" {
		BeforeEach {
			# Create user
			$userName = "$entityPrefix-{0}" -f [guid]::NewGuid();
			$userPassword = "1234";
			$userUri = "{0}{1}" -f $baseUri, "Users";

			$userBody = @{
				Name = $userName
				Password = $userPassword
			}

			$user = Invoke-RestMethod -Method Post -Uri $userUri -Body $userBody
			if (!$user) {
				throw "User could not be created.";
			}

			# Create Coffee
			$coffeeName = "$entityPrefix-{0}" -f [guid]::NewGuid();
			$coffeeBrand = "Test-Brand-{0}" -f [guid]::NewGuid();
			$coffeeUri = "{0}{1}" -f $baseUri, "Coffees";

			$coffeeBody = @{
				Name = $coffeeName
				Brand = $coffeeBrand
				Price = 0.00
				Stock = 0
				LastDelivery = [DateTime]::Now
			}

			$coffee = Invoke-RestMethod -Method Post -Uri $coffeeUri -Body $coffeeBody;
			if (!$coffee) {
				throw "Coffee could not be created."
			}
		}
	Context "Create-CoffeeOrder" {
		BeforeEach {
			$uri = "{0}{1}" -f $baseUri, "CoffeeOrders"
			$name = "$entityPrefix-{0}" -f [guid]::NewGuid();

			$body = @{
				Name = $name
				UserId = $user.Id
				CoffeeId = $coffee.Id;
				Created = [DateTime]::Now;
			}
		}
		
		It "Warmup" -Test {
			$true | Should Be $true;
		}

		It "Create-CoffeeOrderSucceeds" -test {
			# Arrange
			# N/A

			# Act
			$result = Invoke-RestMethod -Method Post -Uri $uri -Body $body;

			# Assert
			$result | Should Not Be $null;
			$result.UserId | Should Be $user.Id;
			$result.CoffeeId | Should Be $coffee.Id;
		}

		It "Create-CoffeeOrderWithoutCoffeeIdThrows" -test {
			# Arrange
			$body.Remove("CoffeeId");

			# Act / Assert
			{ $result = Invoke-RestMethod -Method Post -Uri $uri -Body $body } | Should Throw "400";
		}

		It "Create-CoffeeOrderWithoutUserIdThrows" -test {
			# Arrange
			$body.Remove("UserId");

			# Act / Assert
			{ $result = Invoke-RestMethod -Method Post -Uri $uri -Body $body } | Should Throw "400";
		}
	}
	Context "Update-CoffeeOrder" {
		BeforeEach {
			$uri = "{0}{1}" -f $baseUri, "CoffeeOrders"
			$name = "$entityPrefix-{0}" -f [guid]::NewGuid();

			$body = @{
				Name = $name
				UserId = $user.Id
				CoffeeId = $coffee.Id;
				Created = [DateTime]::Now;
			}

			$coffeeOrder = Invoke-RestMethod -Method Post -Uri $uri -Body $body;
		}
		
		It "Warmup" -Test {
			$true | Should Be $true;
		}
		It "Update-CoffeeOrderCoffeeForeignKeySucceeds" -test {
			# Arrange

			# # Create Coffee
			$newCoffeeName = "$entityPrefix-{0}" -f [guid]::NewGuid();
			$newCoffeeBrand = "Test-Brand-{0}" -f [guid]::NewGuid();
			$newCoffeeUri = "{0}{1}" -f $baseUri, "Coffees";

			$newCoffeeBody = @{
				Name = $coffeeName
				Brand = $coffeeBrand
				Price = 0.00
				Stock = 0
				LastDelivery = [DateTime]::Now
			}

			$newCoffee = Invoke-RestMethod -Method Post -Uri $newCoffeeUri -Body $newCoffeeBody;

			$body.Add("odata.metadata", 'CoffeeTracker/api/$metadata#CoffeeOrders/@Element');
			$body.CoffeeId = $newCoffee.Id;

			$uri = "{0}{1}({2}L)" -f $baseUri, "CoffeeOrders", $coffeeOrder.Id;
			$coffeeOrderJson = $body | ConvertTo-Json;

			# Act
			Invoke-RestMethod -Method Put -Uri $uri -Body $coffeeOrderJson -ContentType "application/json;odata=verbose";

			# Assert
			$result = Invoke-RestMethod -Method Get -Uri $uri;
			$result | Should Not Be $null;
			$result.UserId | Should Be $user.Id;
			$result.CoffeeId | Should Be $newCoffee.Id;
		}
		It "Update-CoffeeOrderChangeCoffeeOrderIdThrows" -test {
			# Arrange
			$uri = "{0}{1}({2}L)" -f $baseUri, "CoffeeOrders", $coffeeOrder.Id;
			$coffeeOrder.Id = $coffeeOrder.Id + 1;
			$coffeeOrderJson = $coffeeOrder | ConvertTo-Json;

			# Act / Assert
			{ Invoke-RestMethod -Method Put -Uri $uri -Body $coffeeOrderJson } | Should Throw;
		}
	}
	AfterAll {
		Write-Host -ForegroundColor Magenta "Check if test data was deleted..."
		It "Warmup-AfterAll" -test {
			$true | Should Be $true;
		}

		foreach($entitySet in $usedEntitieSets) {

			It "Delete-$entitySet-TestDataSucceeded" -test {
				# Arrange
				$queryOption = "startswith(Name, '{0}')" -f $entityPrefix;
				$getUri = '{0}{1}?$filter={2}' -f $baseUri, $entitySet ,$queryOption;

				# Act
				DeleteEntities -EntityName $entitySet -OdataComparison $queryOption;

				# Assert
				$result = Invoke-RestMethod -Method Get -Uri $getUri;
				$result.value.Count | Should Be 0;
			}
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
