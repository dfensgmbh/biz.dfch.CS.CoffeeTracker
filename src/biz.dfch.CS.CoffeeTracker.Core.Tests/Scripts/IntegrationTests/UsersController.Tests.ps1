. ..\Functions\Delete-Entities.ps1
. ..\Functions\CRUD-User.ps1
. ..\Functions\Get-Token.ps1


$baseUri = "http://CoffeeTracker/api/Users";
$entityPrefix = "UserIntegrationTest";

Describe "UsersController" -Tags "UsersController" {
	Context "Create-User" {
		BeforeEach {
			$name = "$entityPrefix-{0}@example.com" -f [guid]::NewGuid();
			$password = "123456";
		}
	
		It "Warmup" -Test {
			$true | Should Be $true;
		}

		it "Create-UserSucceeds" -test {
			# Arrange
			# N/A

			# Act
			$result = CRUD-User -UserName $name -Password $password -Create;

			# Assert
			$result | Should Not Be $null;
			$result.Name = $name;
		}

		it "Create-UserWithWeakPasswordThrows400" -test {
			# Arrange
			$weakPassword = "123";

			# Act/Assert
			{ CRUD-User -UserName $name -Password $weakPassword -Create; } | Should Throw "400";
		}

		it "Create-UserWithoutBodyThrows400" -test {
			# Arrange
			# N/A 

			# Act / Assert
			{ $result = Invoke-RestMethod -Method Post -Uri $baseUri } | Should Throw "400";
		}

		it "Create-UserWithInvalidMailThrows400" -test {
			# Arrange
			$invalidMail = "ThisIsNotAEmailAddress";

			# Act / Assert
			{ CRUD-User -UserName $invalidMail -Password $password -Create; } | Should Throw "400";
		}
	}
	Context "Update-User" {
		BeforeEach{
			$name = "$entityPrefix-{0}@example.com" -f [guid]::NewGuid();
			$password = "123456";

			$entityAdded = CRUD-User -UserName $name -Password $password -Create;
			
			$token = Get-Token -UserName $name -Password $password;

			$newName = "$entityPrefix-{0}@example.com" -f [guid]::NewGuid();
			$newPassword = "568789";

			$entityAddedUri = "{0}({1})" -f $baseUri, $entityAdded.Id;
		}

		It "Warmup" -test {
			$true | Should Be $true;
		}

		It "Update-UsersChangeNameSucceeds" -test {
			# Arrange
			# N/A

			# Act
			CRUD-User -UserName $name -NewUserName $newName -Token $token;

			# Assert
			$newToken = Get-Token -UserName $newName -Password $password; 

			$result = CRUD-User -UserName $newName -Token $newToken -Read;

			$result.Name | Should Not Be $name;
			$result.Password | Should Not Be $password;
			$result.Name | Should Be $newName;
		}

		It "Update-UsersChangePasswordSucceeds" -test {
			# Arrange
			# N/A

			# Act
			CRUD-User -UserName $name -Password $newPassword -Token $token;

			# Assert
			$newToken = Get-Token -UserName $name -Password $newPassword; 

			$result = CRUD-User -UserName $name -Token $newToken -Read;

			$result.Name | Should Be $name;
			$result.Password | Should Not Be $password;
		}

		It "Update-UsersChangePasswordAndNameSucceeds" -test {
			# Arrange
			# N/A

			# Act
			CRUD-User -UserName $name -NewUserName $newName -Password $newPassword -Token $token;

			# Assert
			$newToken = Get-Token -UserName $newName -Password $newPassword; 

			$result = CRUD-User -UserName $newName -Token $newToken -Read;

			$result.Name | Should Not Be $name;
			$result.Password | Should Not Be $password;
			$result.Name | Should Be $newName;
		}

		It "Update-UsersChangePasswordWithWeakPasswordThrows400" -test {
			# Arrange
			$weakPassword = "123";

			# Act/Assert
			{ CRUD-User -UserName $name -NewUserName $newName -Password $weakPassword -Token $token; } | Should Throw "400";
		}

		It "Update-UsersChangeNameToAlreadyExistingNameThrows403" -test {
			# Arrange
			$AlreadyExistingUser = CRUD-User -UserName $newName -Password $password -Create;

			# Act/Assert
			{ CRUD-User -UserName $name -NewUserName $AlreadyExistingUser.Name -Token $token; } | Should Throw "403";
		}

		It "Update-UsersWithInvalidTokenThrows401" -test {
			# Arrange
			$invalidToken = "ThatShouldDefinetlyBeInvalid";

			# Act/Assert
			{ CRUD-User -UserName $name -NewUserName $NewName -Token $invalidToken; } | Should Throw "401";
		}

		It "Update-UsersAsOtherUserThrows403" -test {
			# Arrange
			$otherUser = CRUD-User -UserName $newName -Password $password -Create;
			$newPassword = "123456789";
			
			$otherUserUri = "{0}({1})" -f $baseUri, $otherUser.Id;
			$otherUser.Name = "anotherName@Example.com";
			$otherUserJson = $otherUser | ConvertTo-Json;

			$authString = "bearer {0}" -f $Token;
			$headers = [System.Collections.Generic.Dictionary[[String],[String]]]::New();
			$headers.Add("Authorization", $authString);
			$headers.Add("Content-Type", "application/json;odata=verbose")
			
			# Act/Assert
			{ $result = Invoke-RestMethod -Method Put -Uri $otherUserUri -Body $otherUserJson -Headers $headers; } | Should Throw "403";
		}

		It "Update-UserPutWithoutBodyThrows400" -test {
			# Arrange
			$authString = "bearer {0}" -f $Token;
			$headers = [System.Collections.Generic.Dictionary[[String],[String]]]::New();
			$headers.Add("Authorization", $authString);
			$headers.Add("Content-Type", "application/json;odata=verbose")

			# Act / Assert
			{ Invoke-RestMethod -Method Put -Uri $entityAddedUri -Headers $headers; } | Should Throw "400";
		}

		It "Update-UsersWithInvalidMailThrows400" -test {
			# Arrange
			$invalidMail = "ThatShouldDefinetlyBeInvalid";

			# Act/Assert
			{ CRUD-User -UserName $name -NewUserName $invalidMail -Token $token; } | Should Throw "400";
		}
	}
	AfterAll {
		Write-Host -ForegroundColor Magenta "Check if test data was deleted..."
		$adminToken = Get-Token -Username 'Admin@Example.com' -Password "123456";

		$queryOption = "startswith(Name, '{0}')" -f $entityPrefix;
		$getUri = '{0}?$filter={1}' -f $baseUri, $queryOption;

		Delete-Entities -EntityName "Users" -OdataComparison $queryOption -Token $adminToken;

		$authString = "bearer {0}" -f $adminToken;
		$headers = [System.Collections.Generic.Dictionary[[String],[String]]]::New();
		$headers.Add("Authorization", $authString);

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
