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

	# Create Coffee
	$coffeeName = "$entityPrefix-{0}" -f [Guid]::NewGuid();
	$coffeeBrand = "$entityPrefix-BRAND-{0}" -f [Guid]::NewGuid();
	$coffee = CRUD-Coffee -Name $coffeeName -Brand $coffeeBrand -Token $adminToken -Create;

	# Create headers for requests below
	$authString = "bearer {0}" -f $normalUserToken;
	$headers = [System.Collections.Generic.Dictionary[[String],[String]]]::New();
	$headers.Add("Authorization", $authString);

	# Create CoffeeOrders test data
	for($i = 0; $i -lt 10; $i++)
	{
		$coffeeOrderName = "$entityPrefix-{0}" -f [Guid]::NewGuid();
		
		$coffeeOrderRequestBody = @{
			Name = $coffeeOrderName
			UserId = $normalUser.Id
			CoffeeId = $coffeeName.Id
		}
		
		Invoke-RestMethod -Method Post -Uri $baseuri -Headers $headers -Body $coffeeOrderRequestBody;
	}

	Context "CoffeeConsumption" {
		
		It "Warmup" -Test {
			$true | Should Be $true;
		}

	}
}

#
# Copyright $year$ d-fens GmbH
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
