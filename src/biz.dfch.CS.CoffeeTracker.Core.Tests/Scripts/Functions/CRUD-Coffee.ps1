function CRUD-Coffee{
	PARAM
	(
		[Parameter(Mandatory = $false, Position = 0)]
		[string] $Name
		,
		[Parameter(Mandatory = $false, Position = 1)]
		[string] $Brand
		,
		[Parameter(Mandatory = $false, Position = 2)]
		[string] $Token
		,
		[Parameter(Mandatory = $false, Position = 3)]
		[decimal] $Price
		,
		[Parameter(Mandatory = $false, Position = 4)]
		[long] $Stock
		,
		[Parameter(Mandatory = $false, Position = 5)]
		[DateTimeOffset] $LastDelivery
		,
		[Parameter(Mandatory = $false, ParameterSetName = 'Update')]
		[string] $NewName
		,
		[Parameter(Mandatory = $false, ParameterSetName = 'Update')]
		[string] $NewBrand
		,
		[Parameter(Mandatory = $false, ParameterSetName = 'Create')]
		[switch] $Create
		,
		[Parameter(Mandatory = $false, ParameterSetName = 'Update')]
		[switch] $Update
		,
		[Parameter(Mandatory = $false, ParameterSetName = 'Delete')]
		[switch] $Delete
	)
	Process
	{
		if(!!$LastDelivery)
		{
			$LastDeliveryJson = $LastDelivery | ConvertTo-Json;
		}

		$uri = "CoffeeTracker/api/Coffees";

		$authString = "bearer {0}" -f $Token;

		$headers = [System.Collections.Generic.Dictionary[[String],[String]]]::New();
		$headers.Add("Authorization", $authString);

		if($PSCmdlet.ParameterSetName -eq 'Create')
		{
			$coffeeBody = @{}

			if($Name)
			{
				$coffeeBody["Name"] = $Name;
			}
			if($Brand)
			{
				$coffeeBody["Brand"] = $Brand;
			}
			if($Price)
			{
				$coffeeBody["Price"] = $Price;
			}
			if($Stock)
			{
				$coffeeBody["Stock"] = $Stock;
			}
			if($LastDelivery)
			{
				$coffeeBody["LastDelivery"] = $LastDelivery;
			}

			$result = Invoke-RestMethod -Method Post -Uri $Uri -Headers $headers -Body $coffeeBody;
		}
		elseif($PSCmdlet.ParameterSetName -eq 'Update')
		{
			$filter = '$filter';
			$getUri = "{0}?$filter=Name eq '{1}'" -f $uri, $Name;

			$coffeeResult = Invoke-RestMethod -Method Get -Uri $getUri -Headers $headers;
			$coffee = $coffeeResult.value;
			
			$body = [System.Collections.Generic.Dictionary[[String],[String]]]::New();

			if($Name)
			{
				$body.Add("Name", $Name);
			}
			if($Brand)
			{
				$body.Add("Brand", $Brand);
			}
			if($Price)
			{
				$body.Add("Price", $Price);
			}
			if($Stock)
			{
				$body.Add("Stock", $Stock);
			}
			if($LastDelivery)
			{
				$body.Add("LastDelivery", $LastDeliveryJson);
			}
			if($NewName)
			{
				$body["Name"] = $NewName;
			}
			if($NewBrand)
			{
				$body["Brand"] = $NewBrand;
			}

			$coffeeBodyJson = $body | ConvertTo-Json;

			# Json conversion bug, datetimeoffset needs to be reformatted
			$coffeeBodyJson = $coffeeBodyJson.Replace('\"\', '')
			$coffeeBodyJson = $coffeeBodyJson.Replace('\\/\"','\/')

			$headers.Add("Content-Type", "application/json;odata=verbose")
			$updateUri = "$uri({0})" -f $coffee.Id;

			$result = Invoke-RestMethod -Method Patch -Uri $updateUri -Body $coffeeBodyJson -Headers $headers;
		}
		elseif($PSCmdlet.ParameterSetName -eq 'Delete')
		{
			. .\Delete-Entities.ps1;
			$result = Delete-Entities -EntityName "Coffees" -ODataComparison "Name eq '$Name'";
		}
			
		$OutputParameter = $result;
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
