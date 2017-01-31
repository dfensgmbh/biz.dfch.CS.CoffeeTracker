$here = Split-Path -Parent $MyInvocation.MyCommand.Path

Describe "UsersController" -Tags "UsersController" {
	
	. "$here\DeleteEntities.ps1"
	
	$baseUri = "http://CoffeeTracker/api/Users";
	$entityPrefix = "CoffeeIntegrationTest";

	Context "Create-User" {
		
		BeforeEach {
			$name = "$entityPrefix-{0}" -f [guid]::NewGuid();
			$password = "1234";

			$body = @{
				Name = $name
				Password = $password
			}
		}
	
		It "Warmup" -Test {
			$true | Should Be $true;
		}

		it "Create-UserSucceeds" -test {
			# Arrange
			# N/A

			# Act
			$result = Invoke-RestMethod -Method Post -Uri $baseUri -Body $body

			# Assert
			$result | Should Not Be $null;
			$result.Name = $name;
		}

		it "Create-UserWithoutNameThrows400" -test {
			# Arrange
			$body.Remove("Name");

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
			$password = "1234";

			$newName = "$entityPrefix-{0}" -f [guid]::NewGuid();
			$newBrand = "5687";

			$body = @{
				Name = $name
				Password = $password
			}

			$newBody = @{
				"odata.metadata" = 'CoffeeTracker/api/$metadata#users/@Element'
				Name = $newName
				Password = $newPassword
			}
		}

		It "Warmup" -test {
			$true | Should Be $true;
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
