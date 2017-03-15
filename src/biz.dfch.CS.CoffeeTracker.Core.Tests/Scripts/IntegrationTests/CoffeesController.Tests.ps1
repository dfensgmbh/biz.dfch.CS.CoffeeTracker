. ..\Functions\Delete-Entities.ps1;
. ..\Functions\Get-Token.ps1;
. ..\Functions\CRUD-Coffee.ps1;
. ..\Functions\CRUD-User.ps1;

$baseUri = "http://CoffeeTracker/api/Coffees";
$entityPrefix = "CoffeeIntegrationTest";

Describe "CoffeesController" -Tags "CoffeesController" {
	
	$adminName = "Admin@Example.com";
	$adminPassword = "123456";
	$adminToken = Get-Token -UserName $adminName -Password $adminPassword;

	$normalUserName = "$entityPrefix-{0}@Example.com" -f [guid]::NewGuid();
	$normalUserPassword = "123456";
	$normalUser = CRUD-User -UserName $normalUserName -Password $normalUserPassword -Create;
	$normalUserToken = Get-Token -UserName $normalUserName -Password $normalUserPassword;

	Context "Create-Coffee" {
		BeforeEach {
			$name = "$entityPrefix-{0}" -f [guid]::NewGuid();
			$brand = "Test-Brand-{0}" -f [guid]::NewGuid();
		}

		It "Warmup" -Test {
			$true | Should Be $true;
		}

		It "Create-CoffeeAsAdminSucceeds" -Test {
			# Arrange
			# N/A

			# Act
			$result = CRUD-Coffee -Name $name -Brand $brand -Token $adminToken -Create;

			# Assert
			$result | Should Not Be $null;
			$result.Name | Should Be $name;
			$result.Brand | Should Be $brand;
		}

		It "Create-CoffeeAsAdminWithAllPropertiesSucceeds" -Test {
			# Arrange
			$price = 2.10;
			$stock = 25;
			$lastDelivery = [DateTimeOffSet]::Now;

			# Act
			$result = CRUD-Coffee -Name $name -Brand $brand -Price $price -Stock $stock -LastDelivery $lastDelivery -Token $adminToken -Create;

			# Assert
			$result | Should Not Be $null;
			$result.Name | Should Be $name;
			$result.Brand | Should Be $brand;
			$result.Price | Should Be $price;
			$result.stock | Should Be $stock;
		}

		It "Create-CoffeeAsAdminWithoutNameThrows400" -test {
			# Arrange
			$uri = "CoffeeTracker/api/Coffees";
			$authString = "bearer {0}" -f $adminToken;

			$headers = [System.Collections.Generic.Dictionary[[String],[String]]]::New();
			$headers.Add("Authorization", $authString);

			$coffeeBody = @{}
			$coffeeBody["Brand"] = $brand;

			# Act / Assert
			{ Invoke-RestMethod -Method Post -Uri $Uri -Headers $headers -Body $coffeeBody; } | Should Throw "400";
			
		}
		
		It "Create-CoffeeAsAdminWithoutBrandThrows400" -test {
			# Arrange
			$uri = "CoffeeTracker/api/Coffees";
			$authString = "bearer {0}" -f $adminToken;

			$headers = [System.Collections.Generic.Dictionary[[String],[String]]]::New();
			$headers.Add("Authorization", $authString);

			$coffeeBody = @{}
			$coffeeBody["Name"] = $name;

			# Act / Assert
			{ Invoke-RestMethod -Method Post -Uri $Uri -Headers $headers -Body $coffeeBody; } | Should Throw "400";
			
		}

		It "Create-CoffeeAsNonAdminThrows403" -test {
			# Arrange
			# N/A

			# Act / Assert
			{ CRUD-Coffee -Name $name -Brand $brand -Token $normalUserToken -Create; } | Should Throw "403";
			
		}

		it "Create-CoffeeWithInvalidTokenThrows401" -test {
			# Arrange
			$invalidToken = "ShouldBeDefinetlyInvalid";

			# Act / Assert
			{ CRUD-Coffee -Name $name -Brand $brand -Token $invalidToken -Create } | Should Throw "401";
		}

		it "Create-CoffeeWithoutTokenThrows401" -test {
			# Arrange
			# N/A

			# Act / Assert
			{ CRUD-Coffee -Name $name -Brand $brand -Create } | Should Throw "401";
		}
	}
	Context "Update-Coffee" {
		BeforeEach {
			$name = "$entityPrefix-{0}" -f [guid]::NewGuid();
			$brand = "TEST-Brand-{0}" -f [guid]::NewGuid();

			$newName = "$entityPrefix-{0}" -f [guid]::NewGuid();
			$newBrand = "TEST-Brand-{0}" -f [guid]::NewGuid();
		}
		It "Warmup" -Test {
			$true | Should Be $true;
		}
		It "Update-CoffeePutChangeNameAndBrandSucceeds" -Test {
			# Arrange
			$coffee = CRUD-Coffee -Name $name -Brand $brand -Token $adminToken -Create;

			$coffee.Name = $newName;
			$coffee.Brand = $newBrand;

			$authString = "bearer {0}" -f $adminToken;
			$headers = [System.Collections.Generic.Dictionary[[String],[String]]]::New();
			$headers.Add("Authorization", $authString);

			$coffeeBodyJson = $coffee | ConvertTo-Json;

			$uri = "CoffeeTracker/api/Coffees";
			$updateUri = "$uri({0})" -f $coffee.Id;
			
			$headers.Add("Content-Type", "application/json;odata=verbose")

			# Act
			$result = Invoke-RestMethod -Method Put -Uri $updateUri -Body $coffeeBodyJson -Headers $headers;

			# Assert
			$response = Invoke-RestMethod -Method Get -Uri $updateUri -Headers $headers;
			$result = $response.d;

			$result | Should Not Be $null;
			$result.Name | Should Be $newName;
			$result.Brand | Should Be $newBrand;
		}

		It "Update-CoffeePatchChangeNameAndBrandSucceeds" -Test {
			# Arrange
			$coffee = CRUD-Coffee -Name $name -Brand $brand -Token $adminToken -Create;

			# Act
			CRUD-Coffee -Name $name -NewName $newName -Brand $brand -NewBrand $newBrand -Token $adminToken -Update;

			# Assert
			$uri = "CoffeeTracker/api/Coffees";
			$updateUri = "$uri({0})" -f $coffee.Id;
			
			$authString = "bearer {0}" -f $adminToken;
			$headers = [System.Collections.Generic.Dictionary[[String],[String]]]::New();
			$headers.Add("Authorization", $authString);

			$result = Invoke-RestMethod -Method Get -Uri $updateUri -Headers $headers;

			$result | Should Not Be $null;
			$result.Name | Should Be $newName;
			$result.Brand | Should Be $newBrand;
		}

		It "Update-CoffeePatchChangeNameSucceeds" -Test {
			# Arrange
			$coffee = CRUD-Coffee -Name $name -Brand $brand -Token $adminToken -Create;

			# Act
			CRUD-Coffee -Name $name -NewName $newName -Brand $brand -Token $adminToken -Update;

			# Assert
			$uri = "CoffeeTracker/api/Coffees";
			$updateUri = "$uri({0})" -f $coffee.Id;
			
			$authString = "bearer {0}" -f $adminToken;
			$headers = [System.Collections.Generic.Dictionary[[String],[String]]]::New();
			$headers.Add("Authorization", $authString);

			$result = Invoke-RestMethod -Method Get -Uri $updateUri -Headers $headers;

			$result | Should Not Be $null;
			$result.Name | Should Be $newName;
			$result.Brand | Should Be $brand;
		}

		It "Update-CoffeePatchChangeBrandSucceeds" -Test {
			# Arrange
			$coffee = CRUD-Coffee -Name $name -Brand $brand -Token $adminToken -Create;

			# Act
			CRUD-Coffee -Name $name -Brand $brand -NewBrand $newBrand -Token $adminToken -Update;

			# Assert
			$uri = "CoffeeTracker/api/Coffees";
			$updateUri = "$uri({0})" -f $coffee.Id;
			
			$authString = "bearer {0}" -f $adminToken;
			$headers = [System.Collections.Generic.Dictionary[[String],[String]]]::New();
			$headers.Add("Authorization", $authString);

			$result = Invoke-RestMethod -Method Get -Uri $updateUri -Headers $headers;

			$result | Should Not Be $null;
			$result.Name | Should Be $name;
			$result.Brand | Should Be $newBrand;
		}

		It "Update-CoffeePatchChangePriceAndStockAndLastDeliverySucceeds" -Test {
			# Arrange
			$coffee = CRUD-Coffee -Name $name -Brand $brand -Token $adminToken -Create;
			$price = 4.20;
			$stock = 42;
			$lastDelivery = [DateTimeOffset]::Now.ToString('yyyy-MM-ddTHH:mm:sszzz');

			# Act
			CRUD-Coffee -Name $name -Brand $brand -Price $price -Stock $stock -LastDelivery $lastDelivery -Token $adminToken -Update;

			# Assert
			$uri = "CoffeeTracker/api/Coffees";
			$updateUri = "$uri({0})" -f $coffee.Id;
			
			$authString = "bearer {0}" -f $adminToken;
			$headers = [System.Collections.Generic.Dictionary[[String],[String]]]::New();
			$headers.Add("Authorization", $authString);

			$result = Invoke-RestMethod -Method Get -Uri $updateUri -Headers $headers;

			$result | Should Not Be $null;
			$result.Name | Should Be $name;
			$result.Brand | Should Be $brand;
			$result.Price | Should Be $price;
			$result.Stock | Should Be $stock;
		}

		It "Update-CoffeeChangeNameAndBrandToExistingNameAndBrandThrows400" -test {
			# Arrange
			$coffeeToBeChanged = CRUD-Coffee -Name $name -Brand $brand -Token $adminToken -Create;
			$coffee = CRUD-Coffee -Name $newName -Brand $newBrand -Token $adminToken -Create;

			# Act / Assert
			{ CRUD-Coffee -Name $name -NewName $newName -Brand $brand -NewBrand $newBrand -Token $adminToken -Update } | Should Throw "400";
		}

		It "Update-CoffeeChangeNameAndBrandAsNonAdminThrows403" -test {
			# Arrange
			$coffee = CRUD-Coffee -Name $name -Brand $brand -Token $adminToken -Create;

			# Act / Assert
			{ CRUD-Coffee -Name $name -NewName $newName -Brand $brand -NewBrand $newBrand -Token $normalUserToken -Update } | Should Throw "403";
		}

		It "Update-CoffeeChangeNameAndBrandNotLoggedInThrows401" -test {
			# Arrange
			$coffee = CRUD-Coffee -Name $newName -Brand $newBrand -Token $adminToken -Create;

			# Act / Assert
			{ CRUD-Coffee -Name $name -NewName $newName -Brand $brand -NewBrand $newBrand -Token $token } | Should Throw "401";
		}
	}

	Context "Get-Coffee" {
		BeforeEach{
			$name = "$entityPrefix-GetTests-{0}" -f [guid]::NewGuid();
			$brand = "Test-Brand-{0}" -f [guid]::NewGuid();

			$result = CRUD-Coffee -Name $name -Brand $brand -Token $adminToken -Create;
		}

		It "Get-CoffeeWithIdSucceeds" -test {
			# Arrange
			$getUri = "{0}({1})" -f $baseUri, $entity.Id;

			$authString = "bearer {0}" -f $adminToken;

			$headers = [System.Collections.Generic.Dictionary[[String],[String]]]::New();
			$headers.Add("Authorization", $authString);


			# Act
			$result = Invoke-RestMethod -Method Get -Uri $getUri -Headers $headers;

			# Assert
			$result | Should Not Be $null;
			$result.Id | Should Be $entity.Id;
			$result.Name | Should Be $entity.Name;
			$result.Brand | Should Be $entity.Brand;
		}
	}
	AfterAll {
		Write-Host -ForegroundColor Magenta "Check if test data was deleted..."
		$queryOption = "startswith(Name, '{0}')" -f $entityPrefix;
		$getUri = '{0}?$filter={1}' -f $baseUri, $queryOption;

		$adminName = "Admin@Example.com";
		$adminPassword = "123456";
		$adminToken = Get-Token -UserName $adminName -Password $adminPassword;

		$authString = "bearer {0}" -f $adminToken;

		$headers = [System.Collections.Generic.Dictionary[[String],[String]]]::New();
		$headers.Add("Authorization", $authString);

		Delete-Entities -EntityName "Coffees" -OdataComparison $queryOption -Token $adminToken;

		$result = Invoke-RestMethod -Method Get -Uri $getUri -Headers $headers;
		if($result.value.Count -gt 0)
		{
			Write-Host -ForegroundColor Red "Test-data was not deleted!";
		}
		else
		{
			Write-Host -ForegroundColor Green "Test-data deleted successfully!";
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
