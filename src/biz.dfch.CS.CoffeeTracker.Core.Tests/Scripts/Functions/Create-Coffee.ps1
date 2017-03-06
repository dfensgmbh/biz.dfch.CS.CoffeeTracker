function Create-Coffee{
	PARAM
	(
		[Parameter(Mandatory = $true, Position = 0)]
		[ValidateNotNullOrEmpty()]
		[string] $Name
		,
		[Parameter(Mandatory = $true, Position = 1)]
		[ValidateNotNullOrEmpty()]
		[string] $Brand
		,
		[Parameter(Mandatory = $true, Position = 2)]
		[ValidateNotNullOrEmpty()]
		[string] $Token
		,
		[Parameter(Mandatory = $false, Position = 3)]
		[ValidateScript({ $Price -ge 0 })]
		[ValidateScript({ $Price -le [decimal]::MaxValue })]
		[decimal] $Price
		,
		[Parameter(Mandatory = $false, Position = 4)]
		[ValidateRange(0, [long]::MaxValue)]
		[long] $Stock
		,
		[Parameter(Mandatory = $false, Position = 5)]
		[DateTimeOffset] $LastDelivery
	)
	Process
	{
		$uri = "CoffeeTracker/api/Coffees";

		if(!$Price)
		{
			$Price = 0;
		}

		if(!$Stock)
		{
			$Stock = 0;
		}

		if(!$LastDelivery)
		{
			$LastDelivery = [DateTime]::Now;
		}

		$authString = "bearer {0}" -f $Token;

		$headers = [System.Collections.Generic.Dictionary[[String],[String]]]::New();
		$headers.Add("Authorization", $authString);

		$coffeeBody = @{
				Name = $Name
				Brand = $Brand
				Price = $Price
				Stock = $Stock
				LastDelivery = $LastDelivery
		}

		$coffee = Invoke-RestMethod -Method Post -Uri $Uri -Headers $headers -Body $coffeeBody;
			
#		throw "Coffee could not be created."

		$OutputParameter = $coffee;
	}
	End
	{
		return $OutputParameter;
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
