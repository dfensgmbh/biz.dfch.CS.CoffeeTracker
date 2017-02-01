. .\DeleteEntities.ps1;

$baseUri = "http://CoffeeTracker/api/Coffees";
$entityPrefix = "CoffeeIntegrationTest";

Describe "CoffeesController" -Tags "CoffeesController" {
	
	Context "Create-Coffee" {
		BeforeEach {
			$name = "$entityPrefix-{0}" -f [guid]::NewGuid();
			$brand = "Test-Brand-{0}" -f [guid]::NewGuid();

			$body = @{
				Name = $name
				Brand = $brand
				Price = 0.00
				Stock = 0
				LastDelivery = [DateTime]::Now
			}
		}
	
		It "Warmup" -Test {
			$true | Should Be $true;
		}

		It "Warmup-DeleteEntitiesCalled" -Test {
			$called = DeleteEntities -EntityName "Coffees" -OdataComparison "1 eq 2";

			$called | Should Be $true;
		}

		It "Create-CoffeeSucceeds" -Test {
			# Arrange
			$nameCheck = "*{0}*" -f $name;
			$brandCheck = "*{0}*" -f $brand;

			# Act
			$result = Invoke-RestMethod -Method Post -Uri $baseUri -Body $body;

			# Assert
			$result | Should Not Be $null;
			$result | Should BeLike $nameCheck;
			$result | Should BeLike $brandCheck;
		}

		It "Create-CoffeeWithoutNameThrows" -test {
			# Arrange
			$body.Remove("Name");

			# Act / Assert
			{ Invoke-RestMethod -Method Post -Uri $baseUri -Body $body } | Should Throw "400";
			
		}

		It "Create-CoffeeWithoutBrandThrows" -test {
			# Arrange
			$body.Remove("Brand");

			# Act / Assert
			{ Invoke-RestMethod -Method Post -Uri $baseUri -Body $body } | Should Throw "400";
			
		}

	}
	Context "Update-Coffee" {
		BeforeEach {
			$name = "$entityPrefix-{0}" -f [guid]::NewGuid();
			$brand = "TEST-Brand-{0}" -f [guid]::NewGuid();

			$newName = "$entityPrefix-{0}" -f [guid]::NewGuid();
			$newBrand = "TEST-Brand-{0}" -f [guid]::NewGuid();


			$body = @{
				Name = $name
				Brand = $brand
				Price = "0.00"
				Stock = 0
				LastDelivery = [DateTime]::Now
			}

			$newBody = @{
				"odata.metadata" = 'CoffeeTracker/api/$metadata#Coffees/@Element'
				Name = $newName
				Brand = $NewBrand
				Price = "0.00"
				Stock = 0
				LastDelivery = [DateTime]::Now
			}
		}

		It "Warmup" -Test {
			$true | Should Be $true;
		}

		It "Update-CoffeePutSucceeds" -Test {
			# Arrange
			$nameCheck = "*{0}*" -f $name;
			$brandCheck = "*{0}*" -f $brand;
			
			$newNameCheck = "*{0}*" -f $newName;
			$newBrandCheck = "*{0}*" -f $newBrand;

			$resultAdd = Invoke-RestMethod -Method Post -Uri $baseUri -Body $body;

			$putUri = "$baseUri({0}L)" -f $resultAdd.Id;

			$newBodyJson = ConvertTo-Json -InputObject $newBody;

			# Act
			Invoke-RestMethod -Method Put -Uri $putUri -Body $newBodyJson -ContentType "application/json;odata=verbose";

			# Assert
			$result = Invoke-RestMethod -Method Get -Uri $putUri;

			$result | Should Not Be $null;
			$result | Should Not BeLike $nameCheck;
			$result | Should Not BeLike $brandCheck;
			$result | Should BeLike $newNameCheck;
			$result | Should BeLike $newBrandCheck;
		}

		It "Update-CoffeePutChangeIdThrows" -test {
			# Arrange
			$resultAdd = Invoke-RestMethod -Method Post -Uri $baseUri -Body $body;

			$putUri = "$baseUri({0}L)" -f $resultAdd.Id;

			$newBody["Id"] = $resultAdd.Id - 1;
			$newBodyJson = ConvertTo-Json -InputObject $newBody;

			# Act / Assert
			{ Invoke-RestMethod -Method Put -Uri $putUri -Body $newBodyJson -ContentType "application/json;odata=verbose" } | Should Throw;
		}

		It "Update-CoffeePatchSucceeds" -Test {
			# Arrange
			$nameCheck = "*{0}*" -f $name;
			$brandCheck = "*{0}*" -f $brand;
			
			$newNameCheck = "*{0}*" -f $newName;
			$newBrandCheck = "*{0}*" -f $newBrand;

			$resultAdd = Invoke-RestMethod -Method Post -Uri $baseUri -Body $body;

			$putUri = "$baseUri({0}L)" -f $resultAdd.Id;

			$newBodyJson = ConvertTo-Json -InputObject $newBody;

			# Act
			Invoke-RestMethod -Method Patch -Uri $putUri -Body $newBodyJson -ContentType "application/json;odata=verbose";

			# Assert
			$result = Invoke-RestMethod -Method Get -Uri $putUri;

			$result | Should Not Be $null;
			$result | Should Not BeLike $nameCheck;
			$result | Should Not BeLike $brandCheck;
			$result | Should BeLike $newNameCheck;
			$result | Should BeLike $newBrandCheck;
		}

		It "Update-CoffeePatchChangeIdThrows" -test {
			# Arrange
			$resultAdd = Invoke-RestMethod -Method Post -Uri $baseUri -Body $body;

			$putUri = "$baseUri({0}L)" -f $resultAdd.Id;

			$newBody["Id"] = $resultAdd.Id - 1;
			$newBodyJson = ConvertTo-Json -InputObject $newBody;

			# Act / Assert
			{ Invoke-RestMethod -Method Patch -Uri $putUri -Body $newBodyJson -ContentType "application/json;odata=verbose" } | Should Throw;
		}

		It "Update-CoffeePatchChangeNameSucceeds" -test {
			# Arrange
			$resultAdd = Invoke-RestMethod -Method Post -Uri $baseUri -Body $body;

			$putUri = "$baseUri({0}L)" -f $resultAdd.Id;

			$changedEntity = @{
				Name = $newName
				Brand = $brand
				"odata.metadata" = 'CoffeeTracker/api/$metadata#Coffees/@Element'
			}
			
			$changedEntityJson = ConvertTo-Json -InputObject $changedEntity;

			# Act 
			Invoke-RestMethod -Method Patch -Uri $putUri -Body $changedEntityJson -ContentType "application/json;odata=verbose";
			
			# Assert
			$result = Invoke-RestMethod -Method Get -Uri $putUri

			$result.Name | Should Be $newName;
			$result.Id | Should Be $resultAdd.Id;
		}
	}
	Context "Get-Coffee" {
		BeforeEach{
			$name = "$entityPrefix-GetTests-{0}" -f [guid]::NewGuid();
			$brand = "Test-Brand-{0}" -f [guid]::NewGuid();

			$body = @{
				Name = $name
				Brand = $brand
				Price = 0.00
				Stock = 0
				LastDelivery = [DateTime]::Now
			}
			
			$entity = Invoke-RestMethod -Method Post -Uri $baseUri -Body $body;
		}

		It "Get-CoffeeWithIdSucceeds" -test {
			# Arrange
			$getUri = "{0}({1})" -f $baseUri, $entity.Id;

			# Act
			$result = Invoke-RestMethod -Method Get -Uri $getUri;

			# Assert
			$result | Should Not Be $null;
			$result.Id | Should Be $entity.Id;
			$result.Name | Should Be $entity.Name;
			$result.Brand | Should Be $entity.Brand;
		}
	}
	AfterAll {
		Write-Host -ForegroundColor Magenta "Check if test data was deleted..."
		It "Warmup-AfterAll" -test {
			$true | Should Be $true;
		}

		It "Delete-TestDataSucceeded" -test {
			# Arrange
			$queryOption = "startswith(Name, '{0}')" -f $entityPrefix;
			$getUri = '{0}?$filter={1}' -f $baseUri, $queryOption;

			# Act
			DeleteEntities -EntityName "Coffees" -OdataComparison $queryOption;

			# Assert
			$result = Invoke-RestMethod -Method Get -Uri $getUri;
			$result.value.Count | Should Be 0;
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
