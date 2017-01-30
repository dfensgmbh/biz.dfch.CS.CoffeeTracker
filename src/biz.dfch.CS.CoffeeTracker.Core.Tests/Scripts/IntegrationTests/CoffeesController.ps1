function ExtractIdFromSingleEntity([string]$responseBody) {
	$splittedResponse = $responseBody.Split("=");
	return $splittedResponse[7].Remove($splittedResponse[7].Length - 1);
}

Describe "CoffeesController" -Tags "CoffeesController" {
	
	$baseUri = "http://CoffeeTracker/api/Coffees";
	$entityPrefix = "CoffeeIntegrationTest";

	Context "Create-Coffee" {
		
		BeforeAll {

		}
		
		BeforeEach {
			$name = "$entityPrefix-{0}" -f [guid]::NewGuid();
			$brand = "Test-Brand-{0}" -f [guid]::NewGuid();

			$body = @{
				name = $name
				brand = $brand
				price = 0.00
				stock = 0
				lastdelivery = [DateTime]::Now
			}
		}
	
		It "Warmup" -Test {
			$true | Should Be $true;
		}

		It "Add-CoffeeSucceeds" -Test {
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

	}
	Context "Update-Coffee" {
		BeforeEach {
			$name = "$entityPrefix-{0}" -f [guid]::NewGuid();
			$brand = "TEST-Brand-{0}" -f [guid]::NewGuid();

			$newName = "$entityPrefix-{0}" -f [guid]::NewGuid();
			$newBrand = "TEST-Brand-{0}" -f [guid]::NewGuid();


			$body = @{
				name = $name
				brand = $brand
				price = 0.00
				stock = 0
				lastdelivery = [DateTime]::Now
			}

			$newBody = @{
				name = $newName
				brand = $NewBrand
				price = 0.00
				stock = 0
				lastdelivery = [DateTime]::Now
				"odata.type" = "CoffeeTracker.Coffee"
			}

			$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
			$headers.Add("Content-Type", 'application/json')
		}

		It "Warmup" -Test {
			$true | Should Be $true;
		}

		It "Update-CoffeePutSucceeds" -Test {
			# Arrange
			$nameCheck = "*{0}*" -f $name;
			$brandCheck = "*{0}*" -f $bran
			
			$newNameCheck = "*{0}*" -f $newName;
			$newBrandCheck = "*{0}*" -f $newBrand;

			$resultAdd = Invoke-RestMethod -Method Post -Uri $baseUri -Body $body;

			$addedEntityId = ExtractIdFromSingleEntity $resultAdd;
			$putUri = "$baseUri({0}L)" -f $addedEntityId;


			# Act
			$result = Invoke-RestMethod -Method Put -Uri $putUri -Body $newBody -Headers $headers;

			# Assert
			$result | Should Not Be $null;
			$result | Should Not BeLike $nameCheck;
			$result | Should Not BeLike $brandCheck;
			$result | Should BeLike $newNameCheck;
			$result | Should BeLike $newBrandCheck;
		}
	}

		AfterAll {
			
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
