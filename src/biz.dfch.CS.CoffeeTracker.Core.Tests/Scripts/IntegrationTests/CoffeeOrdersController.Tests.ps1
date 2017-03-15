. ../Functions/Delete-Entities.ps1
. ../Functions/Get-Token.ps1
. ../Functions/CRUD-User.ps1
. ../Functions/CRUD-Coffee.ps1

$usedEntitieSets = @("Coffees", "Users", "CoffeeOrders");
$entityPrefix = "CoffeeOrdersIntegrationTest";
$baseUri = "CoffeeTracker/api/";

Describe "CoffeeOrdersController" -Tags "CoffeeOrdersController" {
	$adminName = "Admin@Example.com";
	$adminPassword = "123456";
	$adminToken = Get-Token -UserName $adminName -Password $adminPassword;
	
	BeforeEach {
		# Create user
		$userName = "$entityPrefix-{0}@Example.com" -f [guid]::NewGuid();;
		$userPassword = "123456";

		$user = CRUD-User -UserName $userName -Password $userPassword -Create;
		$token = Get-Token -UserName $userName -Password $userPassword;

		# Create second user
		$userSecondName = "$entityPrefix-{0}@Example.com" -f [guid]::NewGuid();;
		$userSecondPassword = "123456";

		$Seconduser = CRUD-User -UserName $userSecondName -Password $userSecondPassword -Create;
		$SecondUsertoken = Get-Token -UserName $userSecondName -Password $userSecondPassword;

		# Create Coffee
		$coffeeName = "$entityPrefix-{0}" -f [guid]::NewGuid();
		$coffeeBrand = "Test-Brand-{0}" -f [guid]::NewGuid();
		$coffeeStock = 10;

		$coffee = CRUD-Coffee -Name $coffeeName -Brand $coffeeBrand -Stock $coffeeStock -Token $adminToken -Create;
	}

	Context "Create-CoffeeOrder" {
		<#
		BeforeEach {
			$uri = "{0}{1}" -f $baseUri, "CoffeeOrders"
			$name = "$entityPrefix-{0}" -f [guid]::NewGuid();

			$authString = "bearer {0}" -f $token;

			$headers = [System.Collections.Generic.Dictionary[[String],[String]]]::New();
			$headers.Add("Authorization", $authString);

			$body = @{
				Name = $name
				UserId = $user.Id
				CoffeeId = $coffee.Id
			}
		}

		It "Warmup" -Test {
			$true | Should Be $true;
		}

		It "Create-CoffeeOrderDecreasesStockFromCoffeeSucceeds" -test {
			# Arrange
			# N/A

			# Act
			$result = Invoke-RestMethod -Method Post -Uri $uri -Body $body -Headers $headers;

			# Assert
			$coffeeUri = "{0}{1}({2})" -f $baseUri, "Coffees", $coffee.Id;
			$resultCoffee = Invoke-RestMethod -Method Get -Uri $coffeeUri -Headers $headers;
			$expectedStock = $coffeeStock - 1;

			$result | Should Not Be $null;
			$result.UserId | Should Be $user.Id;
			$result.CoffeeId | Should Be $coffee.Id;
			$resultCoffee.Stock | Should Be $expectedStock;
		}

		It "Create-CoffeeOrderWithCoffeeNotOnStockThrows400" -test {
			# Arrange
			CRUD-Coffee -Name $coffeeName -Brand $coffeeBrand -Stock 0 -Token $adminToken -Update;

			# Act / Assert
			{ Invoke-RestMethod -Method Post -Uri $uri -Body $body -Headers $headers; } | Should Throw "400";
		}

		It "Create-CoffeeOrderAsOtherUserThrows403" -test {
			# Arrange
			$otherUserName = "$entityPrefix{0}@Example.com" -f [Guid]::NewGuid();
			$otherUserPassword = "123456";
			
			$otherUser = CRUD-User -UserName $otherUserName -Password $otherUserPassword -Create;
			$body["UserId"] = $otherUser.Id;

			# Act / Assert
			{ Invoke-RestMethod -Method Post -Uri $uri -Body $body -Headers $headers; } | Should Throw "403";
		}

		It "Create-CoffeeOrderWithoutCoffeeIdThrows400" -test {
			# Arrange
			$body.Remove("CoffeeId");

			# Act / Assert
			{ $result = Invoke-RestMethod -Method Post -Uri $uri -Body $body -Headers $headers } | Should Throw "400";
		}

		It "Create-CoffeeOrderWithoutUserIdThrows400" -test {
			# Arrange
			$body.Remove("UserId");

			# Act / Assert
			{ $result = Invoke-RestMethod -Method Post -Uri $uri -Body $body -Headers $headers } | Should Throw "400";
		}

		It "Create-CoffeeOrderAsOtherUserThrows403" -test {
			# Arrange
			$body["UserId"] = $secondUser.Id;

			# Act / Assert
			{ $result = Invoke-RestMethod -Method Post -Uri $uri -Body $body -Headers $headers } | Should Throw "403";
		}


		#>
	}

	Context "Update-CoffeeOrder" {
		BeforeEach {
			$coffeeOrdersUpdateUri = "{0}{1}" -f $baseUri, "CoffeeOrders"

			$name = "$entityPrefix-{0}" -f [guid]::NewGuid();

			$authString = "bearer {0}" -f $token;

			$headers = [System.Collections.Generic.Dictionary[[String],[String]]]::New();
			$headers.Add("Authorization", $authString);

			$body = @{
				Name = $name
				UserId = $user.Id
				CoffeeId = $coffee.Id;
			}

			$coffeeOrder = Invoke-RestMethod -Method Post -Uri $coffeeOrdersUpdateUri -Body $body -Headers $headers;
		}
		
		It "Warmup" -Test {
			$true | Should Be $true;
		}

		It "Update-CoffeeOrderCoffeeForeignKeySucceeds" -test {
			# Arrange
			$coffeeOrdersUpdateUri = "{0}({1})" -f $coffeeOrdersUpdateUri, $coffeeOrder.Id;

			# # Create Coffee
			$newCoffeeName = "$entityPrefix-{0}" -f [guid]::NewGuid();
			$newCoffeeBrand = "Test-Brand-{0}" -f [guid]::NewGuid();

			$newCoffee = CRUD-Coffee -Name $newCoffeeName -Brand $newCoffeeBrand -Token $adminToken -Create;

			$body["CoffeeId"] = $newCoffee.Id;

			$updatedCoffeeOrderBodyJson = $body | ConvertTo-Json;

			$authString = "bearer {0}" -f $token;

			$headers = [System.Collections.Generic.Dictionary[[String],[String]]]::New();
			$headers.Add("Authorization", $authString);
			$headers.Add("Content-Type", "application/json");

			# Act
			Invoke-RestMethod -Method Put -Uri $coffeeOrdersUpdateUri -Body $updatedCoffeeOrderBodyJson -Headers $headers;

			# Assert
			$result = Invoke-RestMethod -Method Get -Uri $coffeeOrdersUpdateUri -Headers $headers;
			$result | Should Not Be $null;
			$result.UserId | Should Be $user.Id;
			$result.CoffeeId | Should Be $newCoffee.Id;
		}

	<#
		It "Update-CoffeeOrderChangeCoffeeOrderIdThrows" -test {
			# Arrange
			$uri = "{0}{1}({2}L)" -f $baseUri, "CoffeeOrders", $coffeeOrder.Id;
			$coffeeOrder.Id = $coffeeOrder.Id + 1;
			$coffeeOrderJson = $coffeeOrder | ConvertTo-Json;

			# Act / Assert
			{ Invoke-RestMethod -Method Put -Uri $uri -Body $coffeeOrderJson -Headers $headers } | Should Throw;
		}

	#>
	}
	AfterAll {
		Write-Host -ForegroundColor Magenta "Delete Test data..."

		foreach($entitySet in $usedEntitieSets) {

			$queryOption = "startswith(Name, '{0}')" -f $entityPrefix;
			$getUri = '{0}{1}?$filter={2}' -f $baseUri, $entitySet ,$queryOption;

			$adminName = "Admin@Example.com";
			$adminPW = "123456";
			$adminToken = Get-Token -UserName $adminName -Password $adminPW;

			Delete-Entities -EntityName $entitySet -OdataComparison $queryOption -Token $adminToken;
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
