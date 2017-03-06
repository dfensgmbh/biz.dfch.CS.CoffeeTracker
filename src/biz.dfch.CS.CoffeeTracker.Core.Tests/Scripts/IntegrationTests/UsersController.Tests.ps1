. ..\Functions\DeleteEntities.ps1
. ..\Functions\CRUD-User.ps1


$baseUri = "http://CoffeeTracker/api/Users";
$entityPrefix = "UserIntegrationTest";

Describe "UsersController" -Tags "UsersController" {
	Context "Create-User" {
		
		BeforeEach {
			$name = "$entityPrefix-{0}" -f [guid]::NewGuid();
			$password = "123456";
		}
	
		It "Warmup" -Test {
			$true | Should Be $true;
		}

		it "Create-UserSucceeds" -test {
			# Arrange
			# N/A

			# Act
			$result = CRUD-User -UserName $name -Password $password;

			# Assert
			$result | Should Not Be $null;
			$result.Name = $name;
		}

		it "Create-UserWithoutNameThrows400" -test {
			# Arrange
			# N/A 

			# Act / Assert
			{ $result = Invoke-RestMethod -Method Post -Uri $baseUri -Body $body } | Should Throw "400";
		}

		it "Create-UserWithoutPasswordThrows400" -test {
			# Arrange
			$body.Remove("Password");

			# Act / Assert
			{ $result = Invoke-RestMethod -Method Post -Uri $baseUri -Body $body } | Should Throw "400";
		}
	}
	Context "Update-User" {
		BeforeEach{
			$name = "$entityPrefix-{0}" -f [guid]::NewGuid();
			$password = "123456";

			$newName = "$entityPrefix-{0}" -f [guid]::NewGuid();
			$newPassword = "568789";

			$body = @{
				Name = $name
				Password = $password
			}

			$newBody = @{
				"odata.metadata" = 'CoffeeTracker/api/$metadata#users/@Element'
				Name = $newName
				Password = $newPassword
			}

			$entityAdded = Invoke-RestMethod -Method Post -Uri $baseUri -Body $body;
			$entityAddedUri = "{0}({1})" -f $baseUri, $entityAdded.Id;
		}

		It "Warmup" -test {
			$true | Should Be $true;
		}

		It "Update-UsersChangeNameAndPasswordSucceeds" -test {
			# Arrange
			$newBodyJson = $newBody | ConvertTo-Json;

			# Act
			Invoke-RestMethod -Method Put -Uri $entityAddedUri -Body $newBodyJson -ContentType "application/json;odata=verbose";

			# Assert
			$result = Invoke-RestMethod -Method Get -Uri $entityAddedUri;

			$result.Name | Should Not Be $name;
			$result.Password | Should Not Be $password;
			$result.Name | Should Be $newName;
			$result.Password | Should Be $newPassword;
		}

		It "Update-CoffeePutWithoutNameThrows" -test {
			# Arrange
			$newBody.Remove("Name");
			$newBodyJson = $newBody | ConvertTo-Json;

			# Act / Assert
			{ Invoke-RestMethod -Method Put -Uri $entityAddedUri -Body $newBodyJson } | Should Throw;
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
			DeleteEntities -EntityName "Users" -OdataComparison $queryOption;

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
